namespace Rogue.Pomodoro.WPF.ViewModels.Base;

public enum ChildMessageType
{
    /// <summary>
    /// Changes the current main content view.
    /// </summary>
    ChangeMainContentView,
}

public class ChildMessageArgs
{
    public ChildMessageArgs(ChildMessageType messageType, object value)
    {
        Key = messageType;
        Value = value;
    }

    public ChildMessageType Key { get; }

    public object Value { get; }
}