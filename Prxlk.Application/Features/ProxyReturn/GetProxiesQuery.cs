using Prxlk.Application.Shared.Messages;
using Prxlk.Application.Shared.Validation;
using Prxlk.Contracts;

namespace Prxlk.Application.Features.ProxyReturn
{
    public class GetProxiesQuery : Query<ProxyEnvelope>, IValidatable
    {
        public int Limit { get; }
        public int? Offset { get; }
        
        public GetProxiesQuery(int limit, int? offset)
        {
            Limit = limit;
            Offset = offset;
        }
    }
}