using System.ComponentModel.DataAnnotations;

namespace EcommerceManagement.Domain.Entities.Commons
{
    public class BaseEntity<TKey>
    {
        [Key]
        public virtual TKey ID { get; set; }

        // Override Equals and GetHashCode methods to provide custom equality comparison.
        private int? _requestedHashCode;

        public bool IsTransient()
        {
            return object.Equals(ID, default(TKey));
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj is not BaseEntity<TKey>)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (GetType() != obj.GetType())
                return false;

            BaseEntity<TKey> item = (BaseEntity<TKey>)obj;

            if (item.IsTransient() || IsTransient())
                return false;

            return object.Equals(item.ID, ID);
        }

        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!_requestedHashCode.HasValue)
                    _requestedHashCode = ID.GetHashCode() ^ 31; // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)

                return _requestedHashCode.Value;
            }
            else
                return base.GetHashCode();
        }
    }
}