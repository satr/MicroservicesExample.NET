using System;
using System.Net;

namespace ServiceCommon
{
    public class ServiceResponse
    {
        public HttpStatusCode StatusCode { get; private set; }
        public string RespondResult { get; private set; }

        public ServiceResponse(HttpStatusCode statusCode, string respondResult)
        {
            StatusCode = statusCode;
            RespondResult = respondResult;
        }

        public override string ToString()
        {
            return string.Format("Status code: {0} (\"{1}\").{2}Respond text: {3}", 
                                (int)StatusCode, StatusCode, Environment.NewLine, RespondResult);
        }
    }
}