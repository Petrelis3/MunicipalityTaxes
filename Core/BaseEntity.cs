using System;
using System.ComponentModel.DataAnnotations;

namespace Core
{
    public abstract class BaseEntity
    {
        [Key]
        public Guid Id { get; protected set; }
    }
}
