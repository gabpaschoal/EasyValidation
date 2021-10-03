namespace EasyValidation.Core;

public record ErrorNotification
{
    public ErrorNotification(string keys, string message)
    {
        Keys = keys;
        Message = message;
    }

    public string Keys { get; }
    public string Message { get; }
}

