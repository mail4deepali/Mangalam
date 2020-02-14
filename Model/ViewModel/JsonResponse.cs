using System;
using System.Collections.Generic;
using System.Text;

namespace MMB.Mangalam.Web.Model.ViewModel
{
    public class JsonResponse<T>
    {
        public JsonResponse()
        {
            
        }

        public JsonResponse(bool isSuccess, string message, T data)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
        }

        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

       

    }
}
