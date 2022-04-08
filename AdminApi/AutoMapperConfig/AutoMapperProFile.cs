using AdminApi.Context.DomainModel;
using AutoMapper;
using Model;

namespace AdminApi.AutoMapperConfig
{
    public class AutoMapperProFile : MapperConfigurationExpression
    {
        public AutoMapperProFile()
        {
            CreateMap<User, User_Model>().ReverseMap();
            CreateMap<Role, RoleInfo_Model>().ReverseMap();
        }

    }

}
