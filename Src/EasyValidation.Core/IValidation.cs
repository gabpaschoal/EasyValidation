using EasyValidation.Core.Results;

namespace EasyValidation.Core;

public interface IValidation<T>
{
    IResultData ResultData { get; }
    bool HasNotifications { get; }

    void AddNotification(string property, string message);
    void Validate();
}

