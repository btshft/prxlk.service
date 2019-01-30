namespace Prxlk.Domain.Models
{
    public abstract class Entity 
    { }
    
    public abstract class Entity<TKey> : Entity
    {    
        public TKey Id { get; protected set; }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((Entity<TKey>) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }
        
        protected bool Equals(Entity<TKey> other)
        {
            return Id.Equals(other.Id);
        }
    }
}