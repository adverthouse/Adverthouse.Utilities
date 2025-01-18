using System;

namespace Adverthouse.Common.Data
{
    public abstract class AuditableEntity<T>
    {
        public T CreatedBy { get; set; }

        public DateTime CreateDate { get; set; }

        public T LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public AuditableEntity()
        {
            CreateDate = DateTime.UtcNow;
            LastModifiedDate = DateTime.UtcNow;
        }
    }
}
