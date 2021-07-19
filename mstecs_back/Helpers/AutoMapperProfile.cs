using AutoMapper;
using mstecs_back.Entities;
using mstecs_back.Models.Usuarios;
using mstecs_back.Models.Roles;
namespace mstecs_back.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UsuarioEntitie, UsuarioModel>();
            CreateMap<RegistrarModel, UsuarioEntitie>();
            CreateMap<ActualizarModel, UsuarioEntitie>();
            CreateMap<RolEntitie, RolModel>();
        }
    }
}