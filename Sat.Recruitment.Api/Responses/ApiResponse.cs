using System.Collections.Generic;
using System.Net;

namespace Sat.Recruitment.Api.Responses
{
    /// <summary>
    /// ApiResponse class. Generic class
    /// </summary>
    /// <typeparam name="T">Generic Type</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// ApiResponse constructor.
        /// </summary>
        public ApiResponse(){}
        /// <summary>
        /// ApiResponse constructor.
        /// </summary>
        /// <param name="data">Data response for the client</param>
        public ApiResponse(T data)
        {
            Result = data;
        }
        public T Result { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<string> ErrorMessages { get; set; } = new List<string>();
    }
}
