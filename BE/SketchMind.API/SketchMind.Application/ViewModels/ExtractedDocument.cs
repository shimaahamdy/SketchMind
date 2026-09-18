using System;
using System.Collections.Generic;
using System.Text;

namespace SketchMind.Application.ViewModels
{
    public sealed class ExtractedDocument
    {
        public IReadOnlyList<ExtractedPage> Pages { get; init; } = [];
    }

    public sealed class ExtractedPage
    {
        public int PageNumber { get; init; }

        public string Text { get; init; } = string.Empty;
    }
}
