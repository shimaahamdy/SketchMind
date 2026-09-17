using System;
using System.Collections.Generic;
using System.Text;

namespace SketchMind.Application.Contracts.Documents
{
    public interface IFileStorage
    {
        // method to save file 
       Task<string> SaveAsync(string fileName, string contentType, CancellationToken cancellationToken);

        // mehtod to read file 
        //Task<> ReadAsync(string filePath, CancellationToken cancellationToken);

        // method to delte file 
        Task DeleteAsync(string filePath, CancellationToken cancellationToken);
    }
}
