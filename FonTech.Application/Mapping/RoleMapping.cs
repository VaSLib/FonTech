using AutoMapper;
using FonTech.Domain.Dto.User;
using FonTech.Domain.Entity;

namespace FonTech.Application.Mapping;

public class RoleMapping:Profile
{
    public RoleMapping()
    {
        CreateMap<User, UserDto>().ReverseMap();
    }
}
