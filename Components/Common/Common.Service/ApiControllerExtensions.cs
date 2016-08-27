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
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using Google.Protobuf;
using log4net;

namespace Common.Service
{
    public static class ApiControllerExtensions
    {
        public static async void PerformOperation(this ApiController apiController, ILog log, object ticket, 
                                                                       string operationDescription, 
                                                                       Func<Stream, IOperationResult> operationAction)
        {
            try
            {
                log.DebugForTicket(ticket, $"Requested: {operationDescription}");
                var stream = await apiController.Request.Content.ReadAsStreamAsync();
                var operationResult = operationAction(stream);
                if (operationResult.Success)
                {
                    log.DebugForTicket(ticket, $"{operationDescription} completed.");
                    return;
                }
                var message = log.ErrorForTicket(ticket, $"{operationDescription} failed: {operationResult.ErrorsAsString()}.");
                throw new HttpResponseException(await CreateResponse(apiController, HttpStatusCode.BadRequest, message));
            }
            catch (InvalidProtocolBufferException e)
            {
                var message = log.ErrorForTicket(ticket, $"ProtocolBuffer parsing fails for {operationDescription}.", e);
                throw new HttpResponseException(await CreateResponse(apiController, HttpStatusCode.BadRequest, message));
            }
            catch (Exception e)
            {
                var message = log.ErrorForTicket(ticket, operationDescription, e);
                throw new HttpResponseException(await CreateResponse(apiController, HttpStatusCode.BadRequest, message));
            }
        }

        public static HttpResponseMessage CreateHttpResponseMessageFor(this ApiController apiController, IMessage protobufMessage)
        {
            var response = apiController.Request.CreateResponse(HttpStatusCode.OK);
            var stream = new MemoryStream();
            protobufMessage.WriteTo(stream);
            stream.Seek(0, SeekOrigin.Begin);
            response.Content = new StreamContent(stream);
            response.Content.Headers.ContentLength = stream.Length;
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            return response;
        
        }
        private static async Task<HttpResponseMessage> CreateResponse(ApiController apiController, HttpStatusCode httpStatusCode, string message)
        {
            return await new Task<HttpResponseMessage>(() => apiController.Request.CreateResponse(httpStatusCode, message));
        }
    }
}
