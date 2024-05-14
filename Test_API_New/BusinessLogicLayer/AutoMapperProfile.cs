using AutoMapper;
using Test_API_New.BusinessLogicLayer.DataTransferObject;
using Test_API_New.DataAccessLayer.Entities;

namespace Test_API_New.BusinessLogicLayer
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserRequestDTO, User>();
                //.ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<User, UserResponseDTO>();
        }
    }
}
