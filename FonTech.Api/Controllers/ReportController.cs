// Ignore Spelling: Fon Api

using Asp.Versioning;
using FonTech.Domain.Dto.Report;
using FonTech.Domain.Interfaces.Services;
using FonTech.Domain.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FonTech.Api.Controllers;

[Authorize]
[ApiController]
[ApiVersion("1.0")] 
[Route("api/v{version:apiVersion}/[controller]")]
public class ReportController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
    }


    /// <summary>
    /// Получение отчета пользователя
    /// </summary>
    /// <param name="dto"></param>
    /// <remarks>
    /// Request for create report:
    /// 
    ///     GET
    ///     {
    ///         "id":1
    ///     }
    /// </remarks>
    /// <response code="200">Если отчеты были найдены</response>
    /// <response code="400">Если отчеты не были найдены</response>
    [HttpGet("reposrts/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<ReportDto>>> GetUserReports(long userId)
    {
        var response = await _reportService.GetReportsAsync(userId);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    /// <summary>
    /// Получение отчета с указанием идентификатора
    /// </summary>
    /// <param name="dto"></param>
    /// <remarks>
    /// Request for create report:
    /// 
    ///     GET
    ///     {
    ///         "id":1
    ///     }
    /// </remarks>
    /// <response code="200">Если отчет был найден</response>
    /// <response code="400">Если отчет не был найден</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<ReportDto>>> GetReport(long  id)
    {
        var response = await _reportService.GetReportByIdAsync(id);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    /// <summary>
    /// Удаление отчета с указанием идентификатора
    /// </summary>
    /// <param name="dto"></param>
    /// <remarks>
    /// Request for create report:
    /// 
    ///     Delete
    ///     {
    ///         "id":1
    ///     }
    /// </remarks>
    /// <response code="200">Если отчет удалился</response>
    /// <response code="400">Если отчет не был удален</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<ReportDto>>> Delete(long id)
    {
        var response = await _reportService.DeleteReportAsync(id);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
    /// <summary>
    /// Создание отчета
    /// </summary>
    /// <param name="dto"></param>
    /// <remarks>
    /// Request for create report:
    /// 
    ///     POST
    ///     {
    ///         "name": "Report #1",
    ///         "description": "Test report",
    ///         "userId":1
    ///     }
    /// </remarks>
    /// <response code="200">Если отчет создался</response>
    /// <response code="400">Если отчет не был создан</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<ReportDto>>> Create([FromBody]CreateReportDto dto)
    {
        var response = await _reportService.CreateReportAsync(dto);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    /// <summary>
    /// Обновление отчета с указанием основных свойств
    /// </summary>
    /// <param name="dto"></param>
    /// <remarks>
    /// Request for create report:
    /// 
    ///     PUT
    ///     {
    ///         "id":1,
    ///         "name": "Report #2",
    ///         "description": "Test report2"
    ///     }
    /// </remarks>
    /// <response code="200">Если отчет был найден</response>
    /// <response code="400">Если отчет не был найден</response>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<ReportDto>>> Update([FromBody] UpdateReportDto dto)
    {
        var response = await _reportService.UpdateReportAsync(dto);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
}
