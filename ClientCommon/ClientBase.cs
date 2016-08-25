using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Google.Protobuf;
using ServiceCommon;

namespace ClientCommon
{
    public abstract class ClientBase
    {
        protected ClientBase(string hostUrl, string apiPath)
        {
            HostUrl = hostUrl;
            ApiPath = apiPath;
            HttpClient = new HttpClient();
        }

        private string HostUrl { get; set; }

        protected HttpClient HttpClient { get; set; }

        protected string ApiPath { get; private set; }

        protected async Task<OperationResult> PutAsync(IMessage message, string apiPath)
        {
            using (var stream = new MemoryStream())
            {
                message.WriteTo(stream);
                stream.Seek(0, SeekOrigin.Begin);
                var operationResult = new OperationResult();
                var responseMessage = await HttpClient.PutAsync(GetRequestUri(apiPath), new StreamContent(stream));
                if ((int)responseMessage.StatusCode >= 300)
                    operationResult.AddError(await responseMessage.Content.ReadAsStringAsync());
                return operationResult;
            }
        }

        protected async Task<HttpResponseMessage> GetAsync(string apiPath)
        {
            return await HttpClient.GetAsync(GetRequestUri(apiPath));
        }

        public async Task<HttpResponseMessage> PostAsync(string apiPath, HttpContent content)
        {
            return await HttpClient.PostAsync(GetRequestUri(apiPath), content);
        }

        protected Uri GetRequestUri(string path)
        {
            return new UriBuilder(HostUrl) { Path = path }.Uri;
        }
    }
}
