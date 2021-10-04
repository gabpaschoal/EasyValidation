using System.Collections.ObjectModel;
using System.Text.Json;

namespace EasyValidation.Core.Results;

public class ResultData : IResultData
{
    public ResultData()
    {
        _errors = new Dictionary<string, IList<string>>();
        _assigns = new Dictionary<string, IResultData>();
    }

    private readonly IDictionary<string, IList<string>> _errors;
    private readonly IDictionary<string, IResultData> _assigns;

    public IReadOnlyDictionary<string, IList<string>> Errors => new ReadOnlyDictionary<string, IList<string>>(_errors);
    public IDictionary<string, IResultData> Assigns => new ReadOnlyDictionary<string, IResultData>(_assigns);

    public bool IsValid => !_errors.Any() || !_assigns.Any();

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

    public string GetAsJson()
    {
        return JsonSerializer.Serialize(this);
    }

    public void AssignMember(string key, IResultData resultData)
    {
        if (resultData.IsValid)
            return;

        if (_assigns.ContainsKey(key))
            return;

        _assigns.Add(key, resultData);
    }
}

public class ResultData<TData> : ResultData
{
    public TData Data { get; }
    public ResultData(TData data) : base()
    {
        Data = data;
    }
}