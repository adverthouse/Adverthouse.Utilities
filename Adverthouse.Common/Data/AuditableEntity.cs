using System;
using System.Collections.Generic;
using System.Text;

namespace Adverthouse.Common.Data
{
    public abstract class AuditableEntity<T>
    {
        public T CreatedBy { get; set; }

        public DateTime Created { get; set; }

        public T LastModifiedBy { get; set; }

        public DateTime? LastModified { get; set; }

        public AuditableEntity()
        {
            Created = DateTime.Now;
            LastModified = DateTime.Now;
        }
    }
}
