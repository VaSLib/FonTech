using FonTech.DAL.DependencyInjection;
using FonTech.Application.DependencyInjection;
using Serilog;
using FonTech.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSwagger();



builder.Host.UseSerilog((context,configuration)=>configuration.ReadFrom.Configuration(context.Configuration));



builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddApplication();
    
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("swagger/v1/swagger.json", "FonTech Swagger v 1.0");
        c.SwaggerEndpoint("swagger/v2/swagger.json", "FonTech Swagger v 2.0");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.MapControllers();
app.Run();
