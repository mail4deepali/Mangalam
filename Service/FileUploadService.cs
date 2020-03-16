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

namespace MMB.Mangalam.Web.Service
{

    public class FileUploadService
    {
        FileUploadStringService? _FileUploadStringService = null;
        ConnectionStringService? _ConnectionStringService = null;

        public FileUploadService( FileUploadStringService fileUploadStringService, ConnectionStringService? connectionStringService )
        {
            _FileUploadStringService = fileUploadStringService;
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

        public JsonResponse<string> UploadImages(IFormFileCollection Files, int user_id, int candidate_id )
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


    }
}
