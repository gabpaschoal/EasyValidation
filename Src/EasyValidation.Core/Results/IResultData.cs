namespace EasyValidation.Core.Results;

public interface IResultData
{
    IReadOnlyDictionary<string, IList<string>> Errors { get; }

    void AddError(string key, string message);
}
