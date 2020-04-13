using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Music4Me.Common.Http
{
    public class HttpClientRequestException : HttpRequestException
    {
        public HttpStatusCode StatusCode { get; }

        public HttpClientRequestException(HttpStatusCode statusCode, string message) : base(message)
        {
            this.StatusCode = statusCode;
        }
    }
}
