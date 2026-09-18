using System;
using System.Collections.Generic;
using System.Text;

namespace SketchMind.Application.Contracts.Documents
{
    public interface IFileStorage
    {
        Task<string> SaveAsync(
               Stream file,
               string fileName,
               string contentType,
               CancellationToken cancellationToken);

        Task<Stream> OpenReadAsync(
            string storagePath,
            CancellationToken cancellationToken);

        Task DeleteAsync(
            string storagePath,
            CancellationToken cancellationToken);
    }
}
