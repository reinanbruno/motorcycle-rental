﻿namespace Domain.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
    }
}
