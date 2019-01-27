using System.ComponentModel;

namespace Prxlk.Contracts
{
    public class ProxyRequest
    {
        [DefaultValue(value: 10)]
        public int Count { get; set; }
        
        [DefaultValue(value: 0)]
        public int Offset { get; set; }
        
        public string Key { get; set; }
    }
}