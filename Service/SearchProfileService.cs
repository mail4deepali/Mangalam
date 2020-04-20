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
using FluentValidation;
using MMB.Mangalam.Web.Service.Constants;
using Dapper.Contrib.Extensions;
using MMB.Mangalam.Web.Service.Helper;
using MMB.Mangalam.Web.Model.ViewModel;
using System.Linq;
using System.IO;

namespace MMB.Mangalam.Web.Service
{

    public class SearchProfileService
    {
        ConnectionStringService? _ConnectionStringService = null;
        SecurityService? _SecurityService = null;

        private readonly AppSettings _appSettings;
     
        public SearchProfileService(ConnectionStringService connectionStringService, SecurityService securityService, IOptions<AppSettings> appSettings)
        {
            _ConnectionStringService = connectionStringService;
            _SecurityService = securityService;
            _appSettings = appSettings.Value;
        }

        public JsonResponse<List<CandidateDetails>> SearchCandidates(AgeRangeModel ageRange)
        {
            JsonResponse<List<CandidateDetails>> jsonResponse = new JsonResponse<List<CandidateDetails>>();

            List<CandidateImageLogger> candidateImages = new List<CandidateImageLogger>();
            jsonResponse.Data = new List<CandidateDetails>();
            var folderName = Path.Combine("Resources", "Images");
            var pathofImages = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            DirectoryInfo di = new DirectoryInfo(pathofImages);
            FileInfo[] files = di.GetFiles("*.*");

            try
            {
                using (IDbConnection dbConnection = new NpgsqlConnection(_ConnectionStringService.Value))
                {
                    dbConnection.Open();
                    using (var transaction = dbConnection.BeginTransaction())
                    {
                        ageRange.fromBirthdate = Convert.ToDateTime(ageRange.fromBirthdate.ToString("yyyy/MM/dd"));
                        ageRange.toBirthdate = Convert.ToDateTime(ageRange.toBirthdate.ToString("yyyy/MM/dd"));
                        List<CandidateDetails> candidates = dbConnection.Query<CandidateDetails>(" select c.id, c.first_name, c.last_name,  c.date_of_birth, ge.gender, r.religion_name as religion, ca.caste_name as caste, null as image from candidate c join gender ge on c.gender_id = ge.id join religion r on c.religion_id = r.id  left join caste ca on c.caste_id = ca.id  where date_of_birth between  @p0  and  @p1  and c.id != @p2", new { p0 = ageRange.fromBirthdate, p1 = ageRange.toBirthdate, p2 = ageRange.candidate_id }).ToList();
                        int[] ids = candidates.Select(c => c.id).ToArray();                        
                        //candidateImages = dbConnection.Query<CandidateImageLogger>("SELECT * FROM candidate_image_logger where candidate_id in @p0 ", new { p0 = ids}).ToList();
                        candidateImages = dbConnection.Query<CandidateImageLogger>("SELECT * FROM candidate_image_logger where is_approved = true").ToList();
                        foreach (CandidateDetails candidate in candidates)
                        {
                         
                            foreach (CandidateImageLogger image in candidateImages)
                            {
                                if (candidate.id == image.candidate_id)
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

                            jsonResponse.Data.Add(candidate);

                        }

                       
                        transaction.Commit();

                    }
                }
                jsonResponse.IsSuccess = true;
                jsonResponse.Message = "success";
                return jsonResponse;

            }
            catch (Exception e)
            {

                jsonResponse.IsSuccess = false;
                jsonResponse.Message = "fail";
                return jsonResponse;
            }

        }


    }
}
