using System;
using Core.Entities;

namespace Entities.Concrete
{
    public class Product:IEntity
    {
        public Guid Id { get; init; }
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string LastModifiedBy { get; set; }
    }
}