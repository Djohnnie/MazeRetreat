using System;
using System.Threading.Tasks;
using MazeRetreat.Api.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace MazeRetreat.Api.Middleware
{
    public class RequestUriMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestUriMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, MazeRetreatContext mazeRetreatContext)
        {
            var displayUri = new Uri(context.Request.GetDisplayUrl());
            mazeRetreatContext.RequestUri = displayUri.ToString();
            mazeRetreatContext.HostUri = displayUri.Authority;
            await _next(context);
        }
    }
}