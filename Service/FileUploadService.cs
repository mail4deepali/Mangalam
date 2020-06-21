using MMB.Mangalam.Web.Model;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using Dapper.Contrib.Extensions;
using MMB.Mangalam.Web.Service.Helper;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using MMB.Mangalam.Web.Model.ViewModel;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net.Http.Headers;
using Microsoft.Extensions.Primitives;
using System.Data;
using Npgsql;
using Microsoft.AspNetCore.Http;
using Dapper;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace MMB.Mangalam.Web.Service
{

    public class FileUploadService
    {
        ConnectionStringService? _ConnectionStringService = null;

        public FileUploadService(ConnectionStringService? connectionStringService)
        {
            _ConnectionStringService = connectionStringService;
        }

        public async System.Threading.Tasks.Task<JsonResponse<string>> UploadFileToAzureBlobStorageAsync(FileUploadModel model)
        {
            JsonResponse<string> jsonResponse = new JsonResponse<string>();

            //try
            //{
            //    BlobServiceClient blobServiceClient = new BlobServiceClient(_FileUploadStringService.Value);

            //    string containerName = model.user.phone_number.ToString() + model.user.first_name + Guid.NewGuid().ToString();

            //    BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);

            //    foreach (var file in model.fileDetails)
            //    {
            //        BlobClient blobClient = containerClient.GetBlobClient(file.fileName);
            //        await blobClient.UploadAsync(file.fileAsBase64, true);

            //    }
            //    jsonResponse.IsSuccess = true;
            //    jsonResponse.Message = "Uploaded Successfully";

            //}
            //catch (Exception ex)
            //{

            //    jsonResponse.IsSuccess = false;
            //    jsonResponse.Message = "File Upload Fail";
            //}

            return jsonResponse;

        }

        public JsonResponse<string> UploadImages(IFormFileCollection Files, int user_id, int candidate_id, Boolean isProfile)
        {
            CandidateImageLogger candidateImageLogger = new CandidateImageLogger();
            JsonResponse<string> jsonResponse = new JsonResponse<string>();
            string folderName;
            DirectoryInfo di;

            try
            {

                using (IDbConnection dbConnection = new NpgsqlConnection(_ConnectionStringService.Value))
                {
                    dbConnection.Open();

                    using (var transaction = dbConnection.BeginTransaction())
                    {
                        if (Files.Count > 0)
                        {

                            if (isProfile == true)
                            {
                                 folderName = Path.Combine("Resources", "Images", "User", user_id.ToString(), "Candidates", candidate_id.ToString(), "Profile Image");
                                 dbConnection.Query<CandidateImageLogger>("update candidate_image_logger set is_profile_pic = false where user_id = @p0 and candidate_id = @p1 and is_approved = false  and is_profile_pic = true ", new { p0 = user_id, @p1 = candidate_id });

                            }
                            else
                            {
                                 folderName = Path.Combine("Resources", "Images", "User", user_id.ToString(), "Candidates", candidate_id.ToString(), "Other Images");
                            }
                            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                            if (!Directory.Exists(pathToSave))
                            {
                                di = Directory.CreateDirectory(pathToSave);
                            }
                            else
                            {
                                di = new DirectoryInfo(pathToSave);
                            }

                            for (int i = 0; i < Files.Count; i++)
                            {
                                var file = Files[i];
                                if (file.Length > 0)
                                {
                                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                                    var fullPath = Path.Combine(pathToSave, fileName);
                                    var dbPath = Path.Combine(folderName, fileName);
                                    using (var stream = new FileStream(fullPath, FileMode.Create))
                                    {
                                        file.CopyTo(stream);
                                    }

                                    candidateImageLogger.user_id = user_id;
                                    candidateImageLogger.candidate_id = candidate_id;


                                    candidateImageLogger.image_name = file.FileName;
                                    candidateImageLogger.image_path = dbPath;
                                    candidateImageLogger.content_type = file.ContentType;
                                    candidateImageLogger.image_upload_time = DateTime.Now;
                                    candidateImageLogger.is_approved = false;
                                    if (isProfile)
                                    {
                                        candidateImageLogger.is_profile_pic = true;
                                        candidateImageLogger.is_from_other_three_photos = false;                                        
                                    }
                                    else {
                                        candidateImageLogger.is_from_other_three_photos = true;
                                        candidateImageLogger.is_profile_pic = false;

                                    }
                                    dbConnection.Insert<CandidateImageLogger>(candidateImageLogger, transaction);

                                }

                            }
                        }
                        transaction.Commit();

                        jsonResponse.Data = "Profile images uploaded successfully";
                        jsonResponse.IsSuccess = true;
                        jsonResponse.Message = "Success";

                    }
                }

                return jsonResponse;
            }
            catch (System.Exception ex)
            {

                jsonResponse.Data = "Profile images upload fail";
                jsonResponse.IsSuccess = false;
                jsonResponse.Message = "fail";
                return jsonResponse;
            }


        }


        public JsonResponse<CandidateProfileApprovalModel> GetProfileImagesForApproval()
        {
            List<CandidateImageLogger> candidateImages = new List<CandidateImageLogger>();
            JsonResponse<CandidateProfileApprovalModel> jsonResponse = new JsonResponse<CandidateProfileApprovalModel>();
            jsonResponse.Data = new CandidateProfileApprovalModel();
            jsonResponse.Data.profileimages = new List<CandidateImagaeModel>();
            FileInfo[] filesForApproval = new FileInfo[100];
            DirectoryInfo di;
            var folderName = Path.Combine("Resources", "Images");
            
            var pathofImages = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            if (!Directory.Exists(pathofImages))
            {
                di = Directory.CreateDirectory(pathofImages); 
            }
            di = new DirectoryInfo(pathofImages);
            FileInfo[] files = di.GetFiles("*.*", SearchOption.AllDirectories);
            CandidateImagaeModel imagemodel;
            int i = 0;
            try
            {
                using (IDbConnection dbConnection = new NpgsqlConnection(_ConnectionStringService.Value))
                {

                    if (files.Length > 0)
                    {

                        dbConnection.Open();

                        using (var transaction = dbConnection.BeginTransaction())
                        {
                            candidateImages = dbConnection.Query<CandidateImageLogger>("select * from candidate_image_logger where is_approved = false and is_profile_pic = true or is_from_other_three_photos = true ").ToList();
                            foreach (CandidateImageLogger image in candidateImages)
                            {
                                
                                foreach (FileInfo file in files)
                                {
                                    if (file.Name == image.image_name && image.is_approved == false)
                                    {
                                        using (FileStream fs = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
                                        {
                                            // Create a byte array of file stream length
                                            byte[] ImageData = File.ReadAllBytes(file.FullName);
                                            string base64String = Convert.ToBase64String(ImageData, 0, ImageData.Length);
                                            imagemodel = new CandidateImagaeModel();
                                            imagemodel.is_profile = image.is_profile_pic;
                                            imagemodel.is_from_other_three_photos = image.is_from_other_three_photos;                                            
                                            imagemodel.imageLoggedid = image.id;
                                            imagemodel.user = dbConnection.Query<User>("Select * from user_table where id = @id", new { id = image.user_id }).FirstOrDefault();
                                            imagemodel.candidate = dbConnection.Query<Candidate>("Select * from candidate where id = @id", new { id = image.candidate_id }).FirstOrDefault();
                                            imagemodel.image = "data:image/jpeg;base64," + base64String;
                                            jsonResponse.Data.profileimages.Add(imagemodel);
                                            break;
                                        }
                                    }
                                }

                            }
                            transaction.Commit();
                        }
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


        public JsonResponse<CandidateProfileApprovalModel> GetUploadedPhotos(UserCandidateIDModel IDModel)
        {
            
            List<CandidateImageLogger> candidateImages = new List<CandidateImageLogger>();
            JsonResponse<CandidateProfileApprovalModel> jsonResponse = new JsonResponse<CandidateProfileApprovalModel>();
            jsonResponse.Data = new CandidateProfileApprovalModel();
            jsonResponse.Data.profileimages = new List<CandidateImagaeModel>();
            FileInfo[] filesForApproval = new FileInfo[100];
            DirectoryInfo di;
            var folderName = Path.Combine("Resources", "Images");
            var pathofImages = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            if (!Directory.Exists(pathofImages))
            {
                di = Directory.CreateDirectory(pathofImages);
            }
            di = new DirectoryInfo(pathofImages);
            FileInfo[] files = di.GetFiles("*.*", SearchOption.AllDirectories);
            CandidateImagaeModel imagemodel;
            int i = 0;
            try
            {
                using (IDbConnection dbConnection = new NpgsqlConnection(_ConnectionStringService.Value))
                {

                    if (files.Length > 0)
                    {

                        dbConnection.Open();

                        using (var transaction = dbConnection.BeginTransaction())
                        {
                            candidateImages = dbConnection.Query<CandidateImageLogger>("SELECT * FROM candidate_image_logger where is_approved = true and is_from_other_three_photos and user_id = @p0 and candidate_id = @p1 ", new { p0 = IDModel.user_id, p1 = IDModel.candidate_id }).ToList();

                            foreach (CandidateImageLogger image in candidateImages)
                            {
                                foreach (FileInfo file in files)
                                {
                                    if (file.Name == image.image_name)
                                    {
                                        using (FileStream fs = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
                                        {
                                            byte[] ImageData = File.ReadAllBytes(file.FullName);
                                            string base64String = Convert.ToBase64String(ImageData, 0, ImageData.Length);
                                            imagemodel = new CandidateImagaeModel();
                                            imagemodel.is_profile = image.is_profile_pic;
                                            imagemodel.is_from_other_three_photos = image.is_from_other_three_photos;
                                            imagemodel.imageLoggedid = image.id;
                                            imagemodel.user = dbConnection.Query<User>("Select * from user_table where id = @id", new { id = image.user_id }).FirstOrDefault();
                                            imagemodel.candidate = dbConnection.Query<Candidate>("Select * from candidate where id = @id", new { id = image.candidate_id }).FirstOrDefault();
                                            imagemodel.image = "data:image/jpeg;base64," + base64String;
                                            jsonResponse.Data.profileimages.Add(imagemodel);
                                            break;
                                        }
                                    }
                                }

                            }
                            transaction.Commit();
                        }
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


        public JsonResponse<string> updateToApproveProfile(ApprovedImageModel approveModel)
        {
            JsonResponse<string> jsonResponse = new JsonResponse<string>();
            string folderName;
            DirectoryInfo di;

            try
            {

                using (IDbConnection dbConnection = new NpgsqlConnection(_ConnectionStringService.Value))
                {
                    dbConnection.Open();

                    using (var transaction = dbConnection.BeginTransaction())
                    {
                        CandidateImageLogger imageDetails = dbConnection.Query<CandidateImageLogger>("select * from  candidate_image_logger where id = @p0", new { p0 = approveModel.imageLoggedId }).FirstOrDefault();

                        if (imageDetails.is_profile_pic == true)
                        {
                            folderName = Path.Combine("Resources", "Images", "User", imageDetails.user_id.ToString(), "Candidates", imageDetails.candidate_id.ToString(), "Profile Image");

                            var path = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                            if (Directory.Exists(path))
                            {
                                di = new DirectoryInfo(path);

                                foreach (FileInfo fi in di.GetFiles())
                                {
                                    if (fi.Name != imageDetails.image_name)
                                    {
                                        fi.Delete();
                                    }
                                }

                            }

                            dbConnection.Query<CandidateImageLogger>("update candidate_image_logger set is_profile_pic = false where user_id = @p0 and candidate_id = @p1 and is_approved = true and is_profile_pic = true  and id != @p2", new { p0 = imageDetails.user_id, @p1 = imageDetails.candidate_id, @p2 = imageDetails.id });

                            dbConnection.Query<CandidateImageLogger>("update candidate_image_logger set is_approved = true , is_profile_pic = true  where id = @p0", new { p0 = approveModel.imageLoggedId });

                        }
                        else if (imageDetails.is_from_other_three_photos == true)
                        {
                            dbConnection.Query<CandidateImageLogger>("update candidate_image_logger set is_approved = true where id = @p0", new { p0 = approveModel.imageLoggedId });

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


        public JsonResponse<string> DeletePhoto(ApprovedImageModel approveModel)
        {
            JsonResponse<string> jsonResponse = new JsonResponse<string>();
            string folderName;
            DirectoryInfo di;
            folderName = Path.Combine("Resources", "Images");
            var pathofImages = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            di = new DirectoryInfo(pathofImages);
            FileInfo[] files = di.GetFiles("*.*", SearchOption.AllDirectories);

            try
            {

                using (IDbConnection dbConnection = new NpgsqlConnection(_ConnectionStringService.Value))
                {
                    dbConnection.Open();

                    using (var transaction = dbConnection.BeginTransaction())
                    {
                       string image_name = dbConnection.Query<string>("select image_name from  candidate_image_logger where id = @p0", new { p0 = approveModel.imageLoggedId }).FirstOrDefault();


                        foreach (FileInfo file in files)
                        {
                            if (file.Name == image_name)
                            {
                                file.Delete();
                                break;
                            }
                        }

                          dbConnection.Query<CandidateImageLogger>("update candidate_image_logger set is_approved = false, is_from_other_three_photos = false  where id = @p0", new { p0 = approveModel.imageLoggedId });
                                             
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



