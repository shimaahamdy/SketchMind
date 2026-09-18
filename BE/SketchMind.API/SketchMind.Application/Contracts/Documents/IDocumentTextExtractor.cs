using SketchMind.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SketchMind.Application.Contracts.Documents
{
    public interface IDocumentTextExtractor
    {
        Task<ExtractedDocument> ExtractAsync(
       Stream document,
       string contentType,
       CancellationToken cancellationToken);
    }
}
