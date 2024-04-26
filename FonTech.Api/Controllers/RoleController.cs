using FonTech.Application.Services;
using FonTech.Domain.Dto.Role;
using FonTech.Domain.Entity;
using FonTech.Domain.Interfaces.Services;
using FonTech.Domain.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace FonTech.Api.Controllers;

[Consumes(MediaTypeNames.Application.Json)]
[Route("api/[controller]")]
[ApiController]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    /// <summary>
    /// Создание роли
    /// </summary>
    /// <param name="dto"></param>
    /// <remarks>
    /// Request for create report:
    /// 
    ///     POST
    ///     {
    ///         "name": "User",
    ///     }
    /// </remarks>
    /// <response code="200">Если роль создался</response>
    /// <response code="400">Если роль не был создан</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<Role>>> Create([FromBody] CreateRoleDto dto)
    {
        var response = await _roleService.CreateRoleAsync(dto);
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
    /// Request for delete report:
    /// 
    ///     Delete
    ///     {
    ///         "id":1
    ///     }
    /// </remarks>
    /// <response code="200">Если роль удалился</response>
    /// <response code="400">Если роль не был удален</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<Role>>> Delete(long id)
    {
        var response = await _roleService.DeleteRoleAsync(id);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    /// <summary>
    /// Обновление роли с указанием основных свойств
    /// </summary>
    /// <param name="dto"></param>
    /// <remarks>
    /// Request for update role:
    /// 
    ///     PUT
    ///     {
    ///         "id":1,
    ///         "name": "Admin"
    ///     }
    /// </remarks>
    /// <response code="200">Если роль обновился</response>
    /// <response code="400">Если роль не была обновлена</response>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<Role>>> Update([FromBody] RoleDto dto)
    {
        var response = await _roleService.UpdateRoleAsync(dto);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    /// <summary>
    /// Добавление роли пользователю
    /// </summary>
    /// <param name="dto"></param>
    /// <remarks>
    /// Request for add role for user:
    /// 
    ///     POST
    ///     {
    ///         "login": "User #1",
    ///         "roleName": "Admin"
    ///     }
    /// </remarks>
    /// <response code="200">Если роль добавлена</response>
    /// <response code="400">Если роль не была добавлена</response>

    [HttpPost("addRole")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<Role>>> AddRoleForUser([FromBody] UserRoleDto dto)
    {
        var response = await _roleService.AddForUserAsync(dto);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
}
