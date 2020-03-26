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

        public JsonResponse<string> UploadImages(IFormFileCollection Files, int user_id, int candidate_id)
        {
            CandidateImageLogger candidateImageLogger = new CandidateImageLogger();
            JsonResponse<string> jsonResponse = new JsonResponse<string>();
            try
            {

                using (IDbConnection dbConnection = new NpgsqlConnection(_ConnectionStringService.Value))
                {
                    dbConnection.Open();

                    using (var transaction = dbConnection.BeginTransaction())
                    {
                        for (int i = 0; i < Files.Count; i++)

                        {
                            var file = Files[i];
                            var folderName = Path.Combine("Resources", "Images");
                            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
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
                                dbConnection.Insert<CandidateImageLogger>(candidateImageLogger, transaction);

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
            // FileStream stream = File.Open(@"E:\\Test.jpg");
            //return File(stream, "image/jpeg")
            List<CandidateImageLogger> candidateImages = new List<CandidateImageLogger>();
            JsonResponse<CandidateProfileApprovalModel> jsonResponse = new JsonResponse<CandidateProfileApprovalModel>();
            jsonResponse.Data = new CandidateProfileApprovalModel();
            jsonResponse.Data.profileimages = new List<CandidateImagaeModel>();
            FileInfo[] filesForApproval = new FileInfo[100];
            var folderName = Path.Combine("Resources", "Images");
            var pathofImages = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            DirectoryInfo di = new DirectoryInfo(pathofImages);
            FileInfo[] files = di.GetFiles("*.*");
            CandidateImagaeModel imagemodel;
            int i = 0;
            try
            {

                using (IDbConnection dbConnection = new NpgsqlConnection(_ConnectionStringService.Value))
                {
                    dbConnection.Open();

                    using (var transaction = dbConnection.BeginTransaction())
                    {
                        candidateImages = dbConnection.Query<CandidateImageLogger>("select * from candidate_image_logger where is_approved = false  ").ToList();
                        foreach (CandidateImageLogger image in candidateImages)
                        {
                            // string path = Directory.GetCurrentDirectory() + "\\" + image.image_path;
                            // FileStream fileStream = File.Open( path, FileMode.Open);
                            // files[i++] = PhysicalFile(fileStream, "image/jpeg");
                            
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
                                        imagemodel.imageLoggedid = image.id;
                                        imagemodel.user = dbConnection.Query<User>("Select * from user_table where id = @id", new { image.user_id }).FirstOrDefault();
                                        imagemodel.candidate = dbConnection.Query<Candidate>("Select * from candidate where id = @id", new { image.candidate_id }).FirstOrDefault();
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

        public JsonResponse<string> updateToApproveProfile(int imageLogedId)
        {
            JsonResponse<string> jsonResponse = new JsonResponse<string>();
        
            try
            {

                using (IDbConnection dbConnection = new NpgsqlConnection(_ConnectionStringService.Value))
                {
                    dbConnection.Open();

                    using (var transaction = dbConnection.BeginTransaction())
                    {
                        dbConnection.Query<CandidateImageLogger>("update candidate_image_logger set is_approved = true where id = @p0",new { imageLogedId });
                       
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

