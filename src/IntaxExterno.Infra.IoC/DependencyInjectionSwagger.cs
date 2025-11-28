using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;

namespace IntaxExterno.Infra.IoC;

public static class DependencyInjectionSwagger
{
    public static IServiceCollection AddInfrastructureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "IntaxExterno API",
                Version = "v1",
                Description = "IntaxExterno Backend - API para disponibilização aos clientes",
                Contact = new OpenApiContact
                {
                    Name = "Developers",
                    Email = "developers@intax.com",
                    Url = new Uri("https://intax.com")
                },
                License = new OpenApiLicense
                {
                    Name = "Use under LICX",
                    Url = new Uri("https://intax.com/license")
                }
            });

            c.CustomSchemaIds(type => type.FullName!.Replace('+', '.'));

            const string xmlFilename = "IntaxExterno.Api.xml";
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                BearerFormat = "JWT",
                Description = "Enter valid application token. \r\n\r\n" +
                "JWT Authorization header using the Bearer scheme. \r\n\r\n" +
                "Enter just the token in the text input below. \r\n\r\n" +
                "Example: eyJhbGciOiJFUzIsImtpZCI6Ijg.Yi05YTkzLWNjMzI1OGQ1.eyJodHRwOWxzb2FwLm9yZy93cy",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Scheme = "Bearer",
                Type = SecuritySchemeType.Http,
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,

                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }

    public static IApplicationBuilder UseInfrastructureSwagger(this IApplicationBuilder app)
    {
        app
        .UseSwagger()
        .UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "IntaxExterno API v1");
            c.DisplayRequestDuration();
            c.EnableFilter();
        });

        return app;
    }
}
