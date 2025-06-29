using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Asasy.Infrastructure.Extension
{
    public class SwaggerConfigration : IConfigureOptions<SwaggerGenOptions>
    {
        public void Configure(SwaggerGenOptions c)
        {
            c.SwaggerDoc("AuthAPI", new OpenApiInfo { Title = "Auth API", Version = "v1" });
            c.SwaggerDoc("UserAPI", new OpenApiInfo { Title = "User API", Version = "v1" });
            c.SwaggerDoc("SharedAPI", new OpenApiInfo { Title = "Shared API", Version = "v1" });
            c.SwaggerDoc("ClientLogicAPI", new OpenApiInfo { Title = "ClientLogic API", Version = "v1" });
            c.SwaggerDoc("ChatAPI", new OpenApiInfo { Title = "Chat API", Version = "v1" });
            c.SwaggerDoc("MoreAPI", new OpenApiInfo { Title = "More API", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme."
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                            

                    }
                });
            string xmlPath1 = System.IO.Path.Combine(Environment.CurrentDirectory, "Asasy.Domain.xml");
            string xmlPath2 = System.IO.Path.Combine(Environment.CurrentDirectory, "Asasy.xml");

            c.IncludeXmlComments(xmlPath1);
            c.IncludeXmlComments(xmlPath2);

        }
    }

    public class SwaggerUIConfiguration : IConfigureOptions<SwaggerUIOptions>
    {
        public void Configure(SwaggerUIOptions options)
        {

            options.RoutePrefix = "SwaggerPlus";
            options.SwaggerEndpoint("/swagger/AuthAPI/swagger.json", "Auth API V1");
            options.SwaggerEndpoint("/swagger/UserAPI/swagger.json", "User API V1");
            options.SwaggerEndpoint("/swagger/SharedAPI/swagger.json", "Shared API V1");
            options.SwaggerEndpoint("/swagger/ClientLogicAPI/swagger.json", "ClientLogic API V1");
            options.SwaggerEndpoint("/swagger/ChatAPI/swagger.json", "Chat API V1");
            options.SwaggerEndpoint("/swagger/MoreAPI/swagger.json", "More API V1");

        }
    }
}
