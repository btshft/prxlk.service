using System.ComponentModel;

namespace Prxlk.Contracts
{
    public class ProxyRequest
    {
        public int Count { get; set; } = 10;
        
        public int Offset { get; set; }
        
        public string Key { get; set; }
    }
}