﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace MiddlewareApp
{
    public class RequestCultureMiddleware
    {
        private readonly RequestDelegate next;
        private readonly RequestCultureOptions options;

        public RequestCultureMiddleware(RequestDelegate next, IOptions<RequestCultureOptions> options)
        {
            this.next = next;
            this.options = options.Value;
        }

        public Task Invoke(HttpContext context)
        {
            CultureInfo requestCulture = null;

            var cultureQuery = context.Request.Query["culture"];
            if (!string.IsNullOrWhiteSpace(cultureQuery))
            {
                requestCulture = new CultureInfo(cultureQuery);
            }
            else
            {
                requestCulture = this.options.DefaultCulture;
            }

            if (requestCulture != null)
            {
                CultureInfo.CurrentCulture = requestCulture;
                CultureInfo.CurrentUICulture = requestCulture;
            }

            return this.next(context);
        }
    }
    public static class RequestCultureMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestCulture(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestCultureMiddleware>();
        }
    }

}