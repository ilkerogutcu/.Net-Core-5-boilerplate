using System;

namespace StarterProject.Core.Entities
{
    public interface IEntity
    {
        public string Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string LastModifiedBy { get; set; }
    }
}