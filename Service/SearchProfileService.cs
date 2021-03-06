﻿using MMB.Mangalam.Web.Model;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using MMB.Mangalam.Web.Model.Helpers;
using Microsoft.Extensions.Options;
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
            FileInfo[] files = di.GetFiles("*.*", SearchOption.AllDirectories);
            List<CandidateDetails> candidates;

            try
            {
                using (IDbConnection dbConnection = new NpgsqlConnection(_ConnectionStringService.Value))
                {
                    dbConnection.Open();
                    using (var transaction = dbConnection.BeginTransaction())
                    {

                        string query = @" select c.id, c.first_name, c.last_name, 
                        c.date_of_birth, ge.gender, r.religion_name as religion, ca.caste_name as caste,
                        null as image , DATE_PART('year', Age(CURRENT_DATE,c. date_of_birth)) as age ,
                        c.occupation as occupation, c.phone_number, c.gender_id, c.religion_id,
                        c.caste_id, c.education_id, c.family_type_id, adr.address_line_1, adr.address_line_2, adr.taluka_id, 
                        adr.district_id, adr.state_id, c.user_id, c.marital_status_id , null as language,
                        null as otheroccupation, adr.id as address_id from candidate c 
                        join gender ge on c.gender_id = ge.id 
                        join religion r on c.religion_id = r.id
                        left join address adr on c.address_id = adr.id
                        left join caste ca on c.caste_id = ca.id ";
                       
                        if (ageRange.state_id != null)
                        {
                            query += @"  join (select ad.id as address_id from address ad join state st on ad.state_id = st.id  where ad.state_id = @p5) as addrs
                                     on c.address_id = addrs.address_id";
                        }


                        query += @" where DATE_PART('year',Age( CURRENT_DATE , c.date_of_birth))  >= @p0 and 
                            DATE_PART('year', Age(CURRENT_DATE,c. date_of_birth)) <= @p1 ";

                        if (ageRange.gender_id != null)
                        {
                            query += " and c.gender_id = @p2 "; 
                        }
                        if (ageRange.caste_id != null)
                        {
                            query += " and c.caste_id = @p3";
                        }
                        if (ageRange.education_id != null)
                        {
                            query += " and c.education_id = @p4";
                        }

                        if(ageRange.candidate_id != null && ageRange.candidate_id != 0)
                        {
                            query += " and c.id != @p6";
                        }

                        if (ageRange.candidate_id != null && ageRange.candidate_id != 0)
                        {
                            candidates = dbConnection.Query<CandidateDetails>(query, new { p0 = ageRange.fromAge, p1 = ageRange.toAge, p2 = ageRange.gender_id, p3 = ageRange.caste_id, p4 = ageRange.education_id, p5 = ageRange.state_id , p6 = ageRange.candidate_id}).ToList();
                        }
                        else
                        {
                            candidates = dbConnection.Query<CandidateDetails>(query, new { p0 = ageRange.fromAge, p1 = ageRange.toAge, p2 = ageRange.gender_id, p3 = ageRange.caste_id, p4 = ageRange.education_id, p5 = ageRange.state_id }).ToList();
                        }
                            int[] ids = candidates.Select(c => c.id).ToArray();                        
                        //candidateImages = dbConnection.Query<CandidateImageLogger>("SELECT * FROM candidate_image_logger where candidate_id in @p0 ", new { p0 = ids}).ToList();
                        candidateImages = dbConnection.Query<CandidateImageLogger>("SELECT * FROM candidate_image_logger where is_approved = true").ToList();

                        foreach (CandidateDetails candidate in candidates)
                        {   
                            candidate.language = dbConnection.Query<int>("select language_id from candidate_language_map  where candidate_id = @p0", new { p0 = candidate.id}).ToArray();

                            foreach (CandidateImageLogger image in candidateImages)
                            {
                                if (candidate.id == image.candidate_id && image.is_approved == true && image.is_profile_pic == true)
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
