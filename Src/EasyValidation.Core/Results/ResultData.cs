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

    public bool IsValid => !_errors.Any();

    public void AddError(string key, string message)
    {
        if (key is null)
            throw new ArgumentNullException(nameof(key));

        if (message is null)
            throw new ArgumentNullException(nameof(message));

        if (!_errors.ContainsKey(key))
        {
            _errors.Add(key, new List<string> { message });
            return;
        }

        var errors = _errors[key];

        if (errors.Contains(message))
            return;

        errors.Add(message);
    }
}
