using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Prxlk.Gateway.Features.Throttling.Net
{
    public class IpAddressUtil
    {
        public static bool ContainsIp(string rule, string clientIp)
        {
            var ip = IPAddress.Parse(clientIp);

            var range = new IpAddressRange(rule);
            if (range.Contains(ip))
            {
                return true;
            }

            return false;
        }

        public static bool ContainsIp(IReadOnlyCollection<string> ipRules, string clientIp)
        {
            var ip =  IPAddress.Parse(clientIp);
            if (ipRules != null && ipRules.Any())
            {
                return ipRules
                    .Select(rule => new IpAddressRange(rule))
                    .Any(range => range.Contains(ip));
            }

            return false;
        }
    }
}