using FluentValidation;
using FonTech.Application.Mapping;
using FonTech.Application.Services;
using FonTech.Application.Validations.FluentValidations.Report;
using FonTech.Domain.Dto.Report;
using FonTech.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using FonTech.Domain.Interfaces.Validations;
using FonTech.Application.Validations;

namespace FonTech.Application.DependencyInjection;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ReportMapping));
        InitService(services);
    }

    private static void InitService(this IServiceCollection services)
    {
        services.AddScoped<IReportValidator,ReportValidator>();

        services.AddScoped<IValidator<CreateReportDto>, CreateReportValidator>();
        services.AddScoped<IValidator<UpdateReportDto>, UpdateReportValidator>();

        services.AddScoped<IReportService, ReportService>();

    }
}
