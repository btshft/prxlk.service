using AutoMapper;
using Prxlk.Contracts;
using Prxlk.Domain.Commands;
using Prxlk.Domain.Models;
using Prxlk.Domain.Queries;

namespace Prxlk.Application.MappingProfiles
{
    public class DomainContractsProfile : Profile
    {
        public DomainContractsProfile()
        {
            CreateMap<Proxy, ProxyTransportModel>();
            CreateMap<ProxyRequest, GetProxiesQuery>()
                .ConvertUsing(request => new GetProxiesQuery(
                    request.Count,
                    request.Offset));
            
            CreateMap<ProxyTransportModel, AddProxiesCommand>()
                .ConvertUsing(m => new AddProxiesCommand(
                    m.Ip, m.Port, m.Protocol, m.Country));
        }
    }
}