using System;
using MongoDB.Bson;

namespace Core.Entities.MongoDb
{
    public abstract class DocumentEntity : IDocument
    {
        public ObjectId Id { get; set; }
        public DateTime CreatedAt => Id.CreationTime;
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedBy { get; set; }
    }
}