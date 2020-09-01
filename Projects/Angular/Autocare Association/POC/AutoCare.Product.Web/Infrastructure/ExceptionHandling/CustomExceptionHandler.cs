using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using AutoCare.Product.Application.Infrastructure.ExceptionHelpers;
using AutoCare.Product.Infrastructure.ExceptionHandler;

namespace AutoCare.Product.Web.Infrastructure.ExceptionHandling
{
    public class CustomExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            HttpStatusCode httpStatusCode;
            string errorMessage = String.Empty;

            if (context.Exception is ChangeRequestExistException)
            {
                httpStatusCode = HttpStatusCode.Conflict;
                if (String.IsNullOrWhiteSpace(context.Exception.Message))
                {
                    errorMessage = "ChangeRequest already exists";
                }
                else
                {
                    errorMessage = context.Exception.Message;
                }

            }
            else if (context.Exception is UnauthorizedAccessException)
            {
                httpStatusCode = HttpStatusCode.Unauthorized;
                if (String.IsNullOrWhiteSpace(context.Exception.Message))
                {
                    errorMessage = "Unauthorized access";
                }
                else
                {
                    errorMessage = context.Exception.Message;
                }
            }
            else if (context.Exception is NoRecordFound)
            {
                httpStatusCode = HttpStatusCode.NoContent;
                if (String.IsNullOrWhiteSpace(context.Exception.Message))
                {
                    errorMessage = "No Record Found";
                }
                else
                {
                    errorMessage = context.Exception.Message;
                }
            }
            else if (context.Exception is RecordAlreadyExist)
            {
                httpStatusCode = HttpStatusCode.Conflict;
                if (String.IsNullOrWhiteSpace(context.Exception.Message))
                {
                    errorMessage = "Record already exists";
                }
                else
                {
                    errorMessage = context.Exception.Message;
                }
            }
            else
            {
                httpStatusCode = HttpStatusCode.InternalServerError;
                if (String.IsNullOrWhiteSpace(context.Exception.Message))
                {
                    errorMessage = "Internal server error";
                }
                else
                {
                    errorMessage = context.Exception.Message;
                }
            }

            context.Result = new TextPlainErrorResult()
            {
                StatusCode = httpStatusCode,
                Request = context.ExceptionContext.Request,
                Content = errorMessage
            };
        }

        private class TextPlainErrorResult : IHttpActionResult
        {
            public HttpRequestMessage  Request { get; set; }
            public  string Content { get; set; }

            public HttpStatusCode StatusCode { get; set; }

            public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                HttpResponseMessage response = new HttpResponseMessage(StatusCode);
                response.Content = new StringContent(Content);
                response.RequestMessage = Request;
                return Task.FromResult(response);
            }
        }
    }
}