using Common.Api;
using Common.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebFramework.Api;

namespace WebFramework.Middleware
{
    public static class CustomExceptionHandlerMiddlewareExtentions
    {
        public static void UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomExceptionHandlerMiddleware>();
        }
    }

    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostingEnvironment _env;
        private readonly ILogger<CustomExceptionHandlerMiddleware> _logger;

        public CustomExceptionHandlerMiddleware(RequestDelegate next,
            IHostingEnvironment env,
            ILogger<CustomExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _env = env;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            string message = null;
            HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;
            ApiResultStatusCode apiStatusCode = ApiResultStatusCode.ServerError;

            try
            {
                await _next(context);
            }
            catch (AppException exception)
            {
                _logger.LogError(exception, exception.Message);
                httpStatusCode = exception.HttpStatusCode;
                apiStatusCode = exception.ApiStatusCode;

                if (_env.IsDevelopment())
                {
                    var dic = new Dictionary<string, string>
                    {
                        ["Exception"] = exception.Message,
                        ["StackTrace"] = exception.StackTrace,
                    };
                    if (exception.InnerException != null)
                    {
                        dic.Add("InnerException.Exception", exception.InnerException.Message);
                        dic.Add("InnerException.StackTrace", exception.InnerException.StackTrace);
                    }
                    if (exception.AdditionalData != null)
                        dic.Add("AdditionalData", JsonConvert.SerializeObject(exception.AdditionalData));

                    message = JsonConvert.SerializeObject(dic);
                }
                else
                {
                    message = exception.Message;
                }
                await WriteToResponseAsync();
            }
            catch (SecurityTokenExpiredException exception)
            {
                _logger.LogError(exception, exception.Message);
                SetUnAuthorizeResponse(exception);
                await WriteToResponseAsync();
            }
            catch (UnauthorizedAccessException exception)
            {
                _logger.LogError(exception, exception.Message);
                SetUnAuthorizeResponse(exception);
                await WriteToResponseAsync();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);

                if (_env.IsDevelopment())
                {
                    var dic = new Dictionary<string, string>
                    {
                        ["Exception"] = exception.Message,
                        ["StackTrace"] = exception.StackTrace,
                    };
                    message = JsonConvert.SerializeObject(dic);
                }
                await WriteToResponseAsync();
            }

            async Task WriteToResponseAsync()
            {
                if (context.Response.HasStarted)
                    throw new InvalidOperationException("The response has already started, the http status code middleware will not be executed.");
                
                ApiResult result = new ApiResult(false, apiStatusCode, message);
                if (_env.IsDevelopment())
                {
                    var error = new List<string>();
                    var messageFilter = message.Replace("\\r\\n   ", " || ").Replace("\\\\", "\\").Replace("\"", "").Replace("   ", " ").Split('{')[1].Split('}')[0];
                    foreach (var item in messageFilter.Substring(messageFilter.IndexOf(',') + 1).Split(" || "))
                    {
                        error.Add(item);
                    }
                    result = new ApiResult(false, apiStatusCode, messageFilter.Split(',')[0].Split(':')[1]) { Errors = error };
                }
                var json = JsonConvert.SerializeObject(result);

                context.Response.StatusCode = (int)httpStatusCode;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(json);
            }

            void SetUnAuthorizeResponse(Exception exception)
            {
                httpStatusCode = HttpStatusCode.Unauthorized;
                apiStatusCode = ApiResultStatusCode.UnAuthorized;

                if (_env.IsDevelopment())
                {
                    var dic = new Dictionary<string, string>
                    {
                        ["Exception"] = exception.Message,
                        ["StackTrace"] = exception.StackTrace
                    };
                    if (exception is SecurityTokenExpiredException tokenException)
                        dic.Add("Expires", tokenException.Expires.ToString());

                    message = JsonConvert.SerializeObject(dic);
                }
            }
        }
    }
}
