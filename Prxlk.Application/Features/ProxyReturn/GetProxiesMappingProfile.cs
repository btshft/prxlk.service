using AutoMapper;
using Prxlk.Contracts;
using Prxlk.Domain.Models;

namespace Prxlk.Application.Features.ProxyReturn
{
    public class GetProxiesMappingProfile : Profile
    {
        public GetProxiesMappingProfile()
        {
            CreateMap<Proxy, ProxyTransportModel>();
        }
    }
}