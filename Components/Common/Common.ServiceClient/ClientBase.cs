#region Copyright notice and license
// Copyright 2016 github.com/satr.  All rights reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are
// met:
//
//     * Redistributions of source code must retain the above copyright
// notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above
// copyright notice, this list of conditions and the following disclaimer
// in the documentation and/or other materials provided with the
// distribution.
//     * Neither the name of github.com/satr nor the names of its
// contributors may be used to endorse or promote products derived from
// this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
// OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
// LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
// THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
#endregion
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Google.Protobuf;

namespace Common.ServiceClient
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
