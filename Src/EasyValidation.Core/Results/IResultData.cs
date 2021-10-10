namespace EasyValidation.Core.Results;

public interface IResultData
{
    IReadOnlyDictionary<string, IList<string>> FieldErrors { get; }
    IReadOnlyDictionary<string, IResultData> AssignsFieldErrors { get; }

    bool IsValid { get; }

    void AddFieldError(string key, string message);

    void AssignMember(string key, IResultData resultData);
}

public interface IResultData<TData> : IResultData
{
    public TData Data { get; }
}