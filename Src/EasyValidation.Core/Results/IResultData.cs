namespace EasyValidation.Core.Results;

public interface IResultData
{
    IReadOnlyDictionary<string, IList<string>> Errors { get; }
    IDictionary<string, IResultData> Assigns { get; }

    bool IsValid { get; }

    void AddError(string key, string message);

    void AssignMember(string key, IResultData resultData);

    string GetAsJson();
}
