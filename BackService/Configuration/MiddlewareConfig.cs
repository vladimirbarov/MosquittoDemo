﻿using Microsoft.AspNetCore.Builder;

namespace BackService
{
    public static class MiddlewareConfig
    {
        /// <summary>
        /// Use swagger UI and endpoint
        /// </summary>
        /// <remarks>
        /// See: https://github.com/drwatson1/AspNet-Core-REST-Service/wiki#documenting-api
        /// </remarks>
        public static IApplicationBuilder UseSwaggerWithOptions(this IApplicationBuilder app)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            SwaggerBuilderExtensions.UseSwagger(app);

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                // Uncomment this line if you want to access the Swagger UI as http://localhost:5000
                // c.RoutePrefix = string.Empty;
                c.SwaggerEndpoint(Constants.Swagger.EndPoint, Constants.Swagger.ApiName);
            });

            return app;
        }
    }
}
