namespace Prxlk.Contracts
{
    public class GetProxyRequest
    {
        public int Count { get; set; } = 10;
        
        public int Offset { get; set; }
        
        public string Key { get; set; }
    }
}