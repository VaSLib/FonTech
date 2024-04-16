using AutoMapper;
using FonTech.Application.Resources;
using FonTech.Domain.Dto.Report;
using FonTech.Domain.Entity;
using FonTech.Domain.Enum;
using FonTech.Domain.Interfaces.Repositories;
using FonTech.Domain.Interfaces.Services;
using FonTech.Domain.Interfaces.Validations;
using FonTech.Domain.Result;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace FonTech.Application.Services;

public class ReportService : IReportService
{
    private readonly IBaseRepository<Report> _reportRepository;
    private readonly IBaseRepository<User> _userRepository;
    private readonly IReportValidator _reportValidator;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public ReportService(IBaseRepository<Report> reportRepository, ILogger logger, IBaseRepository<User> userRepository, IReportValidator reportValidator, IMapper mapper)
    {
        _reportRepository = reportRepository;
        _logger = logger;
        _userRepository = userRepository;
        _reportValidator = reportValidator;
        _mapper = mapper;
    }



    ///<inheritdoc/>
    public async Task<CollectionResult<ReportDto>> GetReportsAsync(long userId)
    {
        ReportDto[] reports;
        try
        {
            reports = await _reportRepository.GetAll()
                .Where(r => r.UserId == userId)
                .Select(r => new ReportDto(r.Id, r.Name, r.Description, r.CreatedAt.ToLongDateString()))
                .ToArrayAsync();
        }

        catch (Exception ex)
        {
            _logger.Error(ex, ex.Message);
            return new CollectionResult<ReportDto>()
            {
                ErrorMessage = ErrorMessage.InternalServerError,
                ErrorCode = (int)ErrorCodes.InternalServerError,
            };
        }

        if (!reports.Any())
        {
            _logger.Warning(ErrorMessage.ReportsNotFound, reports.Length);
            return new CollectionResult<ReportDto>()
            {
                ErrorMessage = ErrorMessage.ReportsNotFound,
                ErrorCode = (int)ErrorCodes.ReportsNotFound,
            };
        }

        return new CollectionResult<ReportDto>()
        {
            Data = reports,
            Count = reports.Length
        };
    }

    ///<inheritdoc/>
    public  Task<BaseResult<ReportDto>> GetReportByIdAsync(long id)
    {
        ReportDto? report;

        try
        {
            report = _reportRepository.GetAll()
                .AsEnumerable()
                .Select(r => new ReportDto(r.Id, r.Name, r.Description, r.CreatedAt.ToLongDateString()))
                .FirstOrDefault(x => x.Id == id);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, ex.Message);
            return Task.FromResult(new BaseResult<ReportDto>()
            {
                ErrorMessage = ErrorMessage.InternalServerError,
                ErrorCode = (int)ErrorCodes.InternalServerError,
            });
        }

        if (report == null)
        {
            _logger.Warning($"Отчет с {id} не найден", id);
            return Task.FromResult(new BaseResult<ReportDto>()
            {
                ErrorMessage = ErrorMessage.ReportNotFound,
                ErrorCode = (int)ErrorCodes.ReportNotFound,
            });
        }

        return Task.FromResult(new BaseResult<ReportDto> { Data = report });
    }


    ///<inheritdoc/>
    public async Task<BaseResult<ReportDto>> CreateReportAsync(CreateReportDto dto)
    {
        try
        {
            var user = await _userRepository.GetAll().FirstOrDefaultAsync(u => u.Id == dto.UserId);
            var report =  await _reportRepository.GetAll().FirstOrDefaultAsync(r=>r.Name == dto.Name);
            var result = _reportValidator.CreateValidator(report, user);
            if (!result.IsSuccess)
            {
                return new BaseResult<ReportDto>()
                {
                    ErrorMessage = result.ErrorMessage,
                    ErrorCode = result.ErrorCode,
                };
            }

            report = new Report()
            {
                Name = dto.Name,
                Description = dto.Description,
                UserId = user.Id
            };

            await _reportRepository.CreateAsync(report);

            return new BaseResult<ReportDto>()
            {
                Data = _mapper.Map<ReportDto>(report)
            };


        }
        catch (Exception ex)
        {
            _logger.Error(ex, ex.Message);
            return new BaseResult<ReportDto>()
            {
                ErrorMessage = ErrorMessage.InternalServerError,
                ErrorCode = (int)ErrorCodes.InternalServerError,
            };
        }
    }

    ///<inheritdoc/>
    public async Task<BaseResult<ReportDto>> DeleteReportAsync(long id)
    {
        try
        {
            var report = await _reportRepository.GetAll().FirstOrDefaultAsync(r => r.Id == id);
            var result = _reportValidator.ValidateOnNull(report);

            if (!result.IsSuccess)
            {
                return new BaseResult<ReportDto>()
                {
                    ErrorMessage = result.ErrorMessage,
                    ErrorCode = result.ErrorCode,
                };
            }

            await _reportRepository.RemoveAsync(report);
            return new BaseResult<ReportDto>()
            {
                Data = _mapper.Map<ReportDto>(report)
            };
        }
        catch (Exception ex)
        {
            _logger.Error(ex, ex.Message);
            return new BaseResult<ReportDto>()
            {
                ErrorMessage = ErrorMessage.InternalServerError,
                ErrorCode = (int)ErrorCodes.InternalServerError,
            };
        }
    }

    public async Task<BaseResult<ReportDto>> UpdateReportAsync(UpdateReportDto dto)
    {
        try
        {
            var report = await _reportRepository.GetAll().FirstOrDefaultAsync(r => r.Id == dto.Id);
            var result = _reportValidator.ValidateOnNull(report);

            if (!result.IsSuccess)
            {
                return new BaseResult<ReportDto>()
                {
                    ErrorMessage = result.ErrorMessage,
                    ErrorCode = result.ErrorCode,
                };
            }

            report.Name = dto.Name;
            report.Description = dto.Description;

            await _reportRepository.UpdateAsync(report);
            return new BaseResult<ReportDto>()
            {
                Data = _mapper.Map<ReportDto>(report)
            };
        }
        catch (Exception ex)
        {
            _logger.Error(ex, ex.Message);
            return new BaseResult<ReportDto>()
            {
                ErrorMessage = ErrorMessage.InternalServerError,
                ErrorCode = (int)ErrorCodes.InternalServerError,
            };
        }
    }
}