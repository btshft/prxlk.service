using System;

namespace Prxlk.Gateway.Features
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class GatewayFeatureAttribute : Attribute
    {
        public GatewayFeatureAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}