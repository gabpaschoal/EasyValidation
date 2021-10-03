namespace EasyValidation.Core;

public interface IValidation<T>
{
    IReadOnlyCollection<ErrorNotification> Notifications { get; }
    bool HasNotifications { get; }

    void AddNotification(string property, string message);
    void AddNotifications(IEnumerable<ErrorNotification> notifications);
    void ClearNotifications();
    void Setup(T value, string firstPartName = "");
    void Validate();
}

