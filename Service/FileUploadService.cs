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
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using MMB.Mangalam.Web.Model.ViewModel;

namespace MMB.Mangalam.Web.Service
{

    public class FileUploadService
    {
        FileUploadStringService? _FileUploadStringService = null;      

        public FileUploadService( FileUploadStringService fileUploadStringService)
        {
            _FileUploadStringService = fileUploadStringService;
        }

        public async System.Threading.Tasks.Task<JsonResponse<string>> UploadFileToAzureBlobStorageAsync(FileUploadModel model)
        {
           JsonResponse<string> jsonResponse = new JsonResponse<string>();

            try
            {
                BlobServiceClient blobServiceClient = new BlobServiceClient(_FileUploadStringService.Value);

                string containerName = model.user.phone_number.ToString() + model.user.first_name + Guid.NewGuid().ToString();

                BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);

                foreach (var file in model.fileDetails)
                {
                    BlobClient blobClient = containerClient.GetBlobClient(file.fileName);
                    await blobClient.UploadAsync(file.fileAsBase64, true);

                }
                jsonResponse.IsSuccess = true;
                jsonResponse.Message = "Uploaded Successfully";

            }
            catch (Exception ex)
            {

                jsonResponse.IsSuccess = false;
                jsonResponse.Message = "File Upload Fail";
            }

            return jsonResponse;

        }


    }
}
