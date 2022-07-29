using Rogue.Pomodoro.Core.Interfaces;

namespace Rogue.Pomodoro.Core;

/// <inheritdoc />
public class BlockedApp : IBlockedApp
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BlockedApp"/> class.
    /// </summary>
    /// <param name="name">The name of the blocked app which should be the same as its process name.</param>
    public BlockedApp(string name)
    {
        Name = name;
    }

    /// <inheritdoc />
    public string Name { get; }
}
