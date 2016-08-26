using System;
using System.IO;
using System.Net;
using System.Net.Http;
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

        private static async Task<HttpResponseMessage> CreateResponse(ApiController apiController, HttpStatusCode httpStatusCode, string message)
        {
            return await new Task<HttpResponseMessage>(() => apiController.Request.CreateResponse(httpStatusCode, message));
        }
    }
}
