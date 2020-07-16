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
            CandidateDetails candidate ;
            CandidateImageLogger candidateImage;
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
                        null as image , DATE_PART('year', Age(CURRENT_DATE,c. date_of_birth)) as age ,
                        c.occupation as occupation, c.phone_number, c.gender_id, c.religion_id,
                        c.caste_id, c.education_id, c.family_type_id, adr.address_line_1, adr.address_line_2, adr.taluka_id, 
                        adr.district_id, adr.state_id, c.user_id, c.marital_status_id , null as language,
                        null as otheroccupation, adr.id as address_id, e.education_degree as education, 
                        f.family_type, t.taluka_name as taluka, d.district_name as district, 
                        st.state_name as state, ms.marital_status , null as languages  from candidate c 
                        join gender ge on c.gender_id = ge.id 
                        join religion r on c.religion_id = r.id
                        left join caste ca on c.caste_id = ca.id 
                        left join highest_education e on c.education_id = e.id
                        left join family_type f on c.family_type_id = f.id
                        left join marital_status ms on c.marital_status_id = ms.id
                        left join address adr on c.address_id = adr.id
                        left join taluka t on adr.taluka_id = t.id
                        left join district d on adr.district_id = d.id
                        left join state st on adr.state_id = st.id
                        where c.user_id = @p0";

                    candidate = connection.Query<CandidateDetails>(query, new { p0 = user.id }).FirstOrDefault();


                    if (candidate!= null)
                    {
                        candidate.languages = connection.Query<string>("select name from language l join candidate_language_map clm on l.id = clm.language_id where clm.candidate_id = @p0", new { p0 = candidate.id }).ToArray();
                        candidateImage = connection.Query<CandidateImageLogger>("SELECT * FROM candidate_image_logger where is_profile_pic = true and is_deleted = false and candidate_id = @p0 and is_approved = true", new { p0 = candidate.id }).FirstOrDefault();

                        if (candidateImage != null && candidate.id == candidateImage.candidate_id && candidateImage.is_profile_pic == true )
                        {
                            if (files != null && files.Length > 0)
                            {

                                foreach (FileInfo file in files)
                                {
                                    if (file.Name == candidateImage.image_name)
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


                        response.Data.candidate = candidate;
                    }

                    
                    response.Data.user = user;
                    if (response.Data.candidate != null)
                    {
                        response.Data.otherPhotosCount = connection.Query("SELECT id FROM candidate_image_logger where is_from_other_three_photos = true and is_deleted = false and user_id = @p0 and candidate_id = @p1 ", new { p0 = user.id, p1 = response.Data.candidate.id }).Count();
                        var count = connection.Query("SELECT id FROM candidate_image_logger where is_profile_pic = true and user_id = @p0 and candidate_id = @p1 ", new { p0 = user.id, p1 = response.Data.candidate.id }).Count();
                        
                        if (count >= 1)
                        {
                            response.Data.hasProfilePhoto = true;
                        }
                        else
                        {
                            response.Data.hasProfilePhoto = false;
                        }
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
