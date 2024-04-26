using FonTech.Domain.Dto.Role;
using FonTech.Domain.Dto.UserRole;
using FonTech.Domain.Entity;
using FonTech.Domain.Result;

namespace FonTech.Domain.Interfaces.Services;
/// <summary>
/// Сервис предназначенный для управление ролей пользователя
/// </summary>
public interface IRoleService
{
    /// <summary>
    /// Создание роли
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<BaseResult<RoleDto>> CreateRoleAsync (CreateRoleDto dto);

    /// <summary>
    /// Удаление роли
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<BaseResult<RoleDto>> DeleteRoleAsync (long id);

    /// <summary>
    /// Обновление роли
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<BaseResult<RoleDto>> UpdateRoleAsync (RoleDto dto);

    /// <summary>
    /// Добавление роли для пользователя
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    Task<BaseResult<UserRoleDto>> AddForUserAsync(UserRoleDto dto );

    /// <summary>
    /// Удаление роли у пользователя
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<BaseResult<UserRoleDto>> DeleteForUserAsync(DeleteUserRoleDto dto );

    /// <summary>
    /// Обновление роли у пользователя
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    Task<BaseResult<UserRoleDto>> UpdateForUserAsync(UpdateUserRoleDto dto );



}
