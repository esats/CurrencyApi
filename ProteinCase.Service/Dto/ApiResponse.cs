using System;
using System.Collections.Generic;
using System.Text;

namespace ProteinCase.Application.Dto
{
    public class ApiResponse
    {
        public bool Success { get; private set; }
        public int Code { get; private set; }
        public string Message { get; private set; }
        public string Exception { get; private set; }
        public object Data { get; private set; }

        public ApiResponse(int code, string message = "", bool success = false, string exception = "")
        {
            Code = code;
            Message = message;
            Success = success;
            Exception = exception;
        }

        public ApiResponse(object result, string message = "", int code = 200, bool success = true)
        {
            Message = message;
            Data = result;
            Success = success;
            Code = code;
        }
    }
}
