using System.Collections.ObjectModel;

namespace EasyValidation.Core.Results;

internal class ResultData : IResultData
{
    public ResultData()
    {
        _errors = new Dictionary<string, IList<string>>();
        _assigns = new Dictionary<string, IResultData>();
    }

    private readonly IDictionary<string, IList<string>> _errors;
    private readonly IDictionary<string, IResultData> _assigns;

    public IReadOnlyDictionary<string, IList<string>> FieldErrors => new ReadOnlyDictionary<string, IList<string>>(_errors);
    public IReadOnlyDictionary<string, IResultData> AssignFieldErrors => new ReadOnlyDictionary<string, IResultData>(_assigns);

    public bool IsValid => !_errors.Any() && !_assigns.Any();

    public void AddFieldError(string key, string message)
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

    public void AssignMember(string key, IResultData resultData)
    {
        if (resultData.IsValid)
            return;

        if (_assigns.ContainsKey(key))
            return;

        _assigns.Add(key, resultData);
    }
}

internal class ResultData<TData> : ResultData, IResultData<TData>
{
    public TData Data { get; }
    public ResultData(TData data) : base()
    {
        Data = data;
    }
}