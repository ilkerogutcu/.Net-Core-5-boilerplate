﻿namespace Core.DataAccess.MongoDb.Configuration
{
    public interface IMongoDbConfiguration
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}