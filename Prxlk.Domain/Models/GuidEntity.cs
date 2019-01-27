using System;

namespace Prxlk.Domain.Models
{
    public abstract class GuidEntity
    {    
        public Guid Id { get; protected set; }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((GuidEntity) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }
        
        protected bool Equals(GuidEntity other)
        {
            return Id.Equals(other.Id);
        }
    }
}