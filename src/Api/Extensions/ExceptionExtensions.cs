using Application.UseCases.Base;
using Microsoft.AspNetCore.Diagnostics;
using Serilog;
using System.Net;
using System.Text;

namespace Extensions
{
    public static class ExceptionExtensions
    {
        public static void UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextFeature != null)
                    {
                        string messageError = contextFeature.Error.Message;
                        var request = context.Request.Body;
                        Log.Error(contextFeature.Error, $"Ocorreu uma exception insperada: {messageError}");
                        await context.Response.WriteAsJsonAsync(new List<CustomResponseMessage>
                        {
                            new CustomResponseMessage
                            {
                                Message = "Ocorreu um erro inesperado! Por favor, tente novamente em instantes."
                            }
                        });
                    }
                });
            });
        }
    }
}
