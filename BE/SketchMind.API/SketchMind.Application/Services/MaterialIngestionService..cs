using SketchMind.Application.Contracts;
using SketchMind.Application.Contracts.AI;
using SketchMind.Application.Contracts.Documents;
using SketchMind.Application.Repos;
using SketchMind.Application.ViewModels;
using SketchMind.Application.ViewModels.AI;

public sealed class MaterialIngestionService : IMaterialIngestionService
{
    private readonly IMaterialRepository _materialRepository;
    private readonly IFileStorage _fileStorage;
    private readonly IDocumentTextExtractor _textExtractor;
    private readonly ITextChunker _chunker;
    private readonly IEmbeddingGenerator _embeddingGenerator;
    private readonly IVectorStore _vectorStore;

    public MaterialIngestionService(
        IMaterialRepository materialRepository,
        IFileStorage fileStorage,
        IDocumentTextExtractor textExtractor,
        ITextChunker chunker,
        IEmbeddingGenerator embeddingGenerator,
        IVectorStore vectorStore)
    {
        _materialRepository = materialRepository;
        _fileStorage = fileStorage;
        _textExtractor = textExtractor;
        _chunker = chunker;
        _embeddingGenerator = embeddingGenerator;
        _vectorStore = vectorStore;
    }

    public async Task ProcessAsync(int materialId, CancellationToken cancellationToken)
    {
        // get material from Database
        var material = await _materialRepository.GetByIdAsync(materialId,cancellationToken);

        if (material is null)
        {
            throw new InvalidOperationException(
                $"Material with id {materialId} was not found.");
        }

        // get material file
        await using var file =
            await _fileStorage.OpenReadAsync(
                material.StoragePath,
                cancellationToken);

        // extract text from file
        var document =
            await _textExtractor.ExtractAsync(
                file,
                material.ContentType,
                cancellationToken);

        // chunk document
        var textChunks = _chunker.Chunk(document);

        if (textChunks.Count == 0)
            return;

        var texts = textChunks
            .Select(x => x.Text)
            .ToList();

        // embeding chunks
        var embeddings =
            await _embeddingGenerator.GenerateAsync(
                texts,
                cancellationToken);

        if (embeddings.Count != textChunks.Count)
        {
            throw new InvalidOperationException(
                "The number of embeddings does not match the number of chunks.");
        }

        var documentChunks = textChunks
            .Select((chunk, index) => new VectorDocument
            {
                UserId = material.UserId,
                MaterialId = material.Id,
                ChunkIndex = chunk.ChunkIndex,
                Text = chunk.Text,
                PageNumber = chunk.PageNumber,
                Embedding = embeddings[index].Vector,
                Metadata = chunk.Metadata
            })
            .ToList();

        await _vectorStore.InsertAsync(documentChunks,
            cancellationToken);
    }
}