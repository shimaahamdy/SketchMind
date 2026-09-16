using System;
using System.Collections.Generic;
using System.Text;

namespace SketchMind.Infrastructure.Data.VectorStore.Mongo
{
    internal class MongoDbOptions
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
        public string CollectionName { get; set; } = "DocumentChunks";
    }
}

