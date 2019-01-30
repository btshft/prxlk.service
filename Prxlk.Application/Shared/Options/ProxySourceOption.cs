using System;

namespace Prxlk.Application.Shared.Options
{
    public class ProxySourceOption
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public TimeSpan Refresh { get; set; }
    }
}