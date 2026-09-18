using System;
using System.Collections.Generic;
using System.Text;

namespace SketchMind.Domain.Entities
{
    public class Material
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string OriginalFileName { get; set; } = string.Empty;

        public string ContentType { get; set; } = string.Empty;

        public long FileSize { get; set; }

        public string StoragePath { get; set; } = string.Empty;


        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
