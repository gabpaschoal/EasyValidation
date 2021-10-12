using EasyValidation.Core.Results;

namespace EasyValidation.Core;

public interface IValidation<T> : IDisposable
{
    IResultData ResultData { get; }
    bool HasErrors { get; }

    void AddError(string property, string message);
    void SetValue(T value);
    void Validate();
}

