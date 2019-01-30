using AutoMapper;
using Prxlk.Domain.Models;

namespace Prxlk.Application.Features.ProxyParse
{
    public class ProxyParseMappingProfile : Profile
    {
        public ProxyParseMappingProfile()
        {
            CreateMap<ProxyInsertCommand, Proxy>()
                .ConvertUsing(c => new Proxy(c.Ip, c.Port, c.Protocol, c.Country));
        }
    }
}