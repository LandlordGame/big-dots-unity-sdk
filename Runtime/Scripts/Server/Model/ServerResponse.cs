using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace SurveyAPI.Service
{
    public class ServerResponse<T> where T : class
    {
        public HttpStatusCode StatusCode { get; private set; }
        public string CustomMessage { get; private set; }
        public T Value { get; private set; }


        public ServerResponse(HttpStatusCode statusCode, string customMessage, T value)
        {
            this.StatusCode = statusCode;
            this.CustomMessage = customMessage;
            this.Value = value;
        }
        public ServerResponse(HttpStatusCode statusCode, string customMessage) : this(statusCode, customMessage, null) { }
        public ServerResponse(HttpStatusCode statusCode) : this(statusCode, null) { }
    }
}
