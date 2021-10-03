namespace EasyValidation.Core.Results;

public interface IResultData
{
    IReadOnlyDictionary<string, IList<string>> Errors { get; }

    bool IsValid { get; }

    void AddError(string key, string message);

    string GetAsJson();
}
