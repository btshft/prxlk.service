using System;

namespace Prxlk.Gateway.Features.ScheduledEventEmit.Emitters
{
    [AttributeUsage(AttributeTargets.Class, Inherited =  false, AllowMultiple = false)]
    public sealed class EventEmitterCategoryAttribute : Attribute
    {
        public EventEmitterCategoryAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}