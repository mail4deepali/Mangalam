using MMB.Mangalam.Web.Model;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using MMB.Mangalam.Web.Model.Helpers;
using Microsoft.Extensions.Options;
using MMB.Mangalam.Web.Model.ViewModel;
using System.Linq;
using Microsoft.AspNetCore.Routing.Matching;
using System.IO;

namespace MMB.Mangalam.Web.Service
{

    public class UserService : IUserService
    {
        ConnectionStringService? _ConnectionStringService = null;
        SecurityService? _SecurityService = null;
        private readonly AppSettings _appSettings;
     
        public UserService(ConnectionStringService connectionStringService, SecurityService securityService, IOptions<AppSettings> appSettings)
        {
            _ConnectionStringService = connectionStringService;
            _SecurityService = securityService;
            _appSettings = appSettings.Value;
        }

        public JsonResponse<UserCandidateModel> Authenticate(string userName, string password)
        {
            JsonResponse<UserCandidateModel> response = new JsonResponse<UserCandidateModel>();
            response.Data = new UserCandidateModel();
            response.Data.candidateList = new List<CandidateDetails>();
            List<CandidateDetails> candidates = new List<CandidateDetails>();
            List<CandidateImageLogger> candidateImages;
            var folderName = Path.Combine("Resources", "Images");
            var pathofImages = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            DirectoryInfo di = new DirectoryInfo(pathofImages);
            FileInfo[] files = null;
            if (Directory.Exists(pathofImages))
            {
                files = di.GetFiles("*.*", SearchOption.AllDirectories);

            }
            string hashedPassword = _SecurityService.HashUserNameAndPassword(userName, password);
            User? user = null;
            using (IDbConnection connection = new NpgsqlConnection(_ConnectionStringService.Value))
            {
                user = connection.QueryFirstOrDefault<User>("Select * from user_table where user_name = @p0 and password = @p1", new { p0 = userName, p1 = hashedPassword });
                if (user != null)
                {

                    string query = @" select c.id, c.first_name, c.last_name, 
                        c.date_of_birth, ge.gender, r.religion_name as religion, ca.caste_name as caste,
                        null as image from candidate c 
                        join gender ge on c.gender_id = ge.id 
                        join religion r on c.religion_id = r.id 
                        left join caste ca on c.caste_id = ca.id  where c.user_id = @p0";
                     
                    candidates = connection.Query<CandidateDetails>(query, new { p0 = user.id }).ToList();

                    candidateImages = connection.Query<CandidateImageLogger>("SELECT * FROM candidate_image_logger where is_approved = true").ToList();


                    foreach (CandidateDetails candidate in candidates)
                    {
                        foreach (CandidateImageLogger image in candidateImages)
                        {
                            if (candidate.id == image.candidate_id && image.is_approved == true && image.is_profile_pic == true)
                            {
                                if (files != null && files.Length > 0)
                                {

                                    foreach (FileInfo file in files)
                                    {
                                        if (file.Name == image.image_name)
                                        {
                                            using (FileStream fs = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
                                            {
                                                byte[] ImageData = File.ReadAllBytes(file.FullName);
                                                string base64String = Convert.ToBase64String(ImageData, 0, ImageData.Length);
                                                candidate.image = "data:image/jpeg;base64," + base64String;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                        }

                        response.Data.candidateList.Add(candidate);

                    }

                    response.Data.user = user;
                    response.Data.selectedCandidate = response.Data.candidateList.FirstOrDefault();
                    if (response.Data.selectedCandidate != null)
                    {
                        response.Data.otherPhotosCount = connection.Query("SELECT id FROM candidate_image_logger where is_from_other_three_photos = true and user_id = @p0 and candidate_id = @p1 ", new { p0 = user.id, p1 = response.Data.selectedCandidate.id }).Count();
                    }
                }
            }

            if (user != null)
            {
                // authentication successful so generate jwt token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, user.id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                user.token = tokenHandler.WriteToken(token);

                if (response.Data.candidateList!= null && response.Data.candidateList.Count > 0)
                {
                    response.Data.selectedCandidate = response.Data.candidateList[0];
                }

                response.IsSuccess = true;
                response.Message = "User Authenticated";
                response.Data.user = user;
               // response.Data = user;
                //added this as saw in 1 post but seems not needed
                //using (IDbConnection connection = new NpgsqlConnection(_ConnectionStringService.Value))
                //{
                //    connection.Execute("Update user_table set token = @token where id = @user_id", new { user.token, user_id = user.id });
                //}
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Username or password Incorrect.";
            }
            
            return response;

        }

        public JsonResponse<UserCandidateModel> UpdatePassword(string userName, string password)
        {
            JsonResponse<UserCandidateModel> response = new JsonResponse<UserCandidateModel>();

            using (IDbConnection connection = new NpgsqlConnection(_ConnectionStringService.Value))
            {
                string hashedPassword = _SecurityService.HashUserNameAndPassword(userName, password);
                try
                {
                    int value = connection.Execute("Update user_table set password = @p0, is_user_login_first_time = 'false' where user_name = @p1", new { p0 = hashedPassword, p1 = userName });
                    if (value > 0)
                    {
                        response.IsSuccess = true;
                        response.Message = "Password Updated";
                    }
                }
                catch(Exception ex)
                {
                    response.IsSuccess = false;
                    response.Message = "Password Update Failed" + ex.Message;
                }
            }
            return response;
            
        }

        public IEnumerable<User> GetAll()
        {
            using (IDbConnection connection = new NpgsqlConnection(_ConnectionStringService.Value))
            {
                List<User> users = connection.QuerySingle<List<User>>("Select * from user_table ");
                return users;
            }           
        }

        public User Get(int id)
        {
            using (IDbConnection connection = new NpgsqlConnection(_ConnectionStringService.Value))
            {
                User user = connection.QuerySingle<User>("Select * from user_table where id = @id", new { id });
                return user;
            }
        }

        public bool IsFirstTimeLogin(int id)
        {
            using (IDbConnection connection = new NpgsqlConnection(_ConnectionStringService.Value))
            {
                User user = connection.QuerySingle<User>("Select * from user_table where id = @id and IsUserloginAfterRegistration = False", new { id });
                if(user == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}
