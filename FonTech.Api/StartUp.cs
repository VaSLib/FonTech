using Asp.Versioning;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace FonTech.Api;

public static class StartUp
{
    /// <summary>
    /// Подключение Swagger
    /// </summary>
    /// <param name="services"></param>
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddApiVersioning()
            .AddApiExplorer(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
            });
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo()
            {
                Version = "v1",
                Title = "FonTech.Api",
                Description = "This is version 1.0",
                TermsOfService = new Uri("https://www.youtube.com/@ITHomester"),
                Contact = new OpenApiContact()
                {
                    Name = "Test Contact",
                    Url = new Uri("https://www.youtube.com/@ITHomester")
                }
            });

            option.SwaggerDoc("v2", new OpenApiInfo()
            {
                Version = "v2",
                Title = "FonTech.Api",
                Description = "This is version 2.0",
                TermsOfService = new Uri("https://www.youtube.com/@ITHomester"),
                Contact = new OpenApiContact()
                {
                    Name = "Test Contact",
                    Url = new Uri("https://www.youtube.com/@ITHomester")
                }
            });

            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                In = ParameterLocation.Header,
                Description = "Введите пожалуйста валидный токен",
                Name = "Авторизация",
                Type = SecuritySchemeType.Http,
                BearerFormat ="JWT",
                Scheme ="Bearer"
            });

            option.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme()
                    {
                        Reference= new OpenApiReference()
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Name ="Bearer",
                        In = ParameterLocation.Header
                    },
                    Array.Empty<string>()
                }
            });

            var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

            option.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,xmlFileName));
        });
    }
}
