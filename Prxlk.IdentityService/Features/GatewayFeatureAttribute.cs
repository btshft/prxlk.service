using System;

namespace Prxlk.IdentityService.Features
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class GatewayFeatureAttribute : Attribute
    {
        public GatewayFeatureAttribute(string name, int order)
        {
            Name = name;
            Order = order;
        }

        public string Name { get; }
        public int Order { get; }
    }
}