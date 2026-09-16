using Microsoft.Extensions.Options;
using SketchMind.Application.DTOs.AI;
using SketchMind.Application.Interfaces.AI;
using SketchMind.Infrastructure.AI.Embedding.DTOs;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace SketchMind.Infrastructure.AI.Embedding
{
    public sealed class HuggingFaceEmbeddingGenerator : IEmbeddingGenerator
    {
        private readonly HttpClient _httpClient;
        private readonly HuggingFaceOptions _options;

        public HuggingFaceEmbeddingGenerator(HttpClient httpClient, IOptions<HuggingFaceOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<IReadOnlyList<EmbeddingResult>> GenerateAsync(IReadOnlyList<string> texts, CancellationToken cancellationToken = default)
        {
            if (texts.Count == 0)
                return [];

            var request = new
            {
                inputs = texts
            };
            //automatically call Dispose() when the scope ends.
            //represents resources associated with that particular HTTP operation,

            using var response = await _httpClient.PostAsJsonAsync(
                _options.Endpoint,
                request,
                cancellationToken);

            var result = new List<EmbeddingResult>();

            try
            {
                response.EnsureSuccessStatusCode();
                var embeddings = await response.Content.ReadFromJsonAsync<float[][]>(cancellationToken);

                if (embeddings is null)
                {
                    throw new InvalidOperationException(
                        "Hugging Face returned an invalid embedding response.");
                }

                if (embeddings.Length != texts.Count)
                {
                    throw new InvalidOperationException(
                        "The number of embeddings does not match the number of inputs.");
                }

                result = embeddings.Select(vector => new EmbeddingResult
                {
                    Vector = vector
                })
                .ToList();

            }

            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    "An error occurred while generating embeddings from Hugging Face.", ex);
            }

            return result;







        }
    }
}
