using System.Collections.ObjectModel;

namespace EasyValidation.Core.Results;

public class ResultData : IResultData
{
    public ResultData()
    {
        _errors = new Dictionary<string, IList<string>>();
    }

    private readonly IDictionary<string, IList<string>> _errors;

    public IReadOnlyDictionary<string, IList<string>> Errors => new ReadOnlyDictionary<string, IList<string>>(_errors);

    public void AddError(string key, string message)
    {
        if (!_errors.ContainsKey(key))
        {
            _errors.Add(key, new[] { message });
            return;
        }

        var errors = _errors[key];

        if (errors.Contains(message))
            return;

        errors.Add(message);
    }
}
