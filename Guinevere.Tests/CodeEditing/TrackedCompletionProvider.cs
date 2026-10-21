namespace Guinevere.Tests.CodeEditing;

/// <summary>Exposes the real word provider's task so tests can await results without polling a deadline.</summary>
sealed class TrackedCompletionProvider : ICompletionProvider
{
    Task<CompletionList?>? _request;

    internal Task<CompletionList?> Request =>
        _request ?? throw new InvalidOperationException("No completion request was started.");

    /// <inheritdoc />
    public Task<CompletionList?> GetCompletionsAsync(TextSnapshot snapshot, int offset,
        CancellationToken cancellation) =>
        _request = WordCompletionProvider.Instance.GetCompletionsAsync(snapshot, offset, cancellation);
}
