using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace MMB.Mangalam.Web.Service
{
    public class APIResponse<T>
    {
        public bool IsSuccess { get; set; }
        public bool IsFail { get { return !IsSuccess; } }
        public HttpStatusCode StatusCode { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
        public List<string> allMessages { get; set; }
        public dynamic translations { get; set; }

        public APIResponse(bool isSuccess, HttpStatusCode statusCode, string message = "")
        {
            this.IsSuccess = isSuccess;
            this.StatusCode = statusCode;
            this.Message = message;
        }

        public APIResponse()
        {

        }
    }
}
