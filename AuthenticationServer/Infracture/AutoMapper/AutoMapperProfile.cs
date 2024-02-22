using AutoMapper;
using Security.Models.ProcessModels;
using Security.Models.RequestModels;

namespace AuthenticationServer.Infracture.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ManualSignupReq, UserModel>();
        }
    }
}
