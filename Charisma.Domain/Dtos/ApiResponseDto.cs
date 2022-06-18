using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Charisma.Domain.Dtos
{
    public class StatusResponse
    {
        public int Status { get; set; }
        public bool Ok { get; set; }
        public string? Message { get; set; }
    }

    public class ApiResponseDto
    {
        public static ApiResponse<object> Success(object? data = null, int status = 200, string? message = null)
        {
            return new ApiResponse<object>
            {
                Response = new StatusResponse
                {
                    Status = status,
                    Ok = true,
                    Message = message
                },
                Data = data
            };
        }

        public static ApiResponse<object> Respons(int status, bool ok, string message, object? data = null)
        {
            return new ApiResponse<object>
            {
                Response = new StatusResponse
                {
                    Status = status,
                    Ok = ok,
                    Message = message
                },
                Data = data
            };
        }

        public static ApiResponse<object> Error(int status, string message, object? data = null)
        {
            return new ApiResponse<object>
            {
                Response = new StatusResponse
                {
                    Status = status,
                    Ok = false,
                    Message = message
                },
                Data = data
            };
        }
    }

    public class ApiResponse<T>
    {
        public StatusResponse? Response { get; set; }

        public T? Data { get; set; }
    }

    public static class ApiResponseExtention
    {
        public static ApiResponse<T> Success<T>(this T data, int status = 200, string? message = null)
        {
            return new ApiResponse<T>
            {
                Response = new StatusResponse
                {
                    Status = status,
                    Ok = true,
                    Message = message
                },
                Data = data
            };
        }
    }
}
