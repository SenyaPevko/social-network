﻿using System.Net.Http.Headers;
using System.Net;

namespace Core.HttpLogic.HttpResponses.Models
{
    public record BaseHttpResponse
    {
        /// <summary>
        /// Response status
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Response headers
        /// </summary>
        public HttpResponseHeaders Headers { get; set; }

        /// <summary>
        /// Content headers
        /// </summary>
        public HttpContentHeaders ContentHeaders { get; init; }

        /// <summary>
        /// Is the status code successful
        /// </summary>
        public bool IsSuccessStatusCode
        {
            get
            {
                var statusCode = (int)StatusCode;

                return statusCode >= 200 && statusCode <= 299;
            }
        }
    }
}