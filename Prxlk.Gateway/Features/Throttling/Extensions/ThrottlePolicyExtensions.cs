using Prxlk.Gateway.Features.Throttling.Net;

namespace Prxlk.Gateway.Features.Throttling.Extensions
{
    public static class ThrottlePolicyExtensions
    {
        public static bool IsWhitelisted(this ThrottlePolicy policy, string ip)
        {
            return IpAddressUtil.ContainsIp(policy.IpWhitelist, ip);
        }
    }
}