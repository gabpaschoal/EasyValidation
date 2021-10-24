namespace EasyValidation.Core.Results;

public interface IResultData : IDisposable
{
    IReadOnlyDictionary<string, IList<string>> FieldErrors { get; }
    IReadOnlyDictionary<string, IResultData> AssignFieldErrors { get; }

    bool IsValid { get; }

    void AddFieldError(string key, string message);

    void AssignMember(string key, IResultData resultData);

    /// <summary>
    /// Overlap Errors
    /// </summary>
    /// <param name="resultData"></param>
    void IncoporateErrors(IDictionary<string, IList<string>> errors, IDictionary<string, IResultData> assignFieldErrors);
    void ClearErrors();
}

public interface IResultData<TData> : IResultData where TData : new()
{
    public TData Data { get; }
}