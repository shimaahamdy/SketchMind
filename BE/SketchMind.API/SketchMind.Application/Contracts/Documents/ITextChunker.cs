using SketchMind.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SketchMind.Application.Contracts.Documents
{
    public interface ITextChunker
    {
        IReadOnlyList<TextChunk> Chunk(ExtractedDocument document);
    }
}
