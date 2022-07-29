namespace Rogue.Pomodoro.Core.Interfaces;

/// <summary>
/// Represents an app that should be blocked.
/// </summary>
public interface IBlockedApp
{
    /// <summary>
    /// Gets the name of the blocked app which should be the same as its process name.
    /// </summary>
    string Name { get; }
}
