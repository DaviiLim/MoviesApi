using Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace Domain.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleAsync(context, ex);
            }
        }

        private static Task HandleAsync(HttpContext context, Exception ex)
        {
            var statusCode = ex switch
            {
                EmailNotFoundException => HttpStatusCode.NotFound,
                EmailAlreadyExistsException => HttpStatusCode.Conflict,
                MovieNotFoundException => HttpStatusCode.NotFound,
                TitleAlreadyExistsException => HttpStatusCode.Conflict,
                InvalidCredentialsException => HttpStatusCode.Unauthorized,
                ForbiddenUserVoteException => HttpStatusCode.Forbidden,
                UserHasNotVotedForMovieException => HttpStatusCode.BadRequest,

                BusinessException => HttpStatusCode.BadRequest,
                _ => HttpStatusCode.InternalServerError
            };

            var response = new
            {
                error = ex.Message
            };

            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            return context.Response.WriteAsync(
                JsonSerializer.Serialize(response)
            );
        }
    }
}
