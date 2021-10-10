namespace EasyValidation.Core.Results;

public interface IResultData
{
    IReadOnlyDictionary<string, IList<string>> FieldErrors { get; }
    IReadOnlyDictionary<string, IResultData> AssignFieldErrors { get; }

    bool IsValid { get; }

    void AddFieldError(string key, string message);

    void AssignMember(string key, IResultData resultData);
}

public interface IResultData<TData> : IResultData where TData : new()
{
    public TData Data { get; }
}