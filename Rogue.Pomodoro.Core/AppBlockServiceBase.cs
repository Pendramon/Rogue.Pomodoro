using Rogue.Pomodoro.Core.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Rogue.Pomodoro.Core;

/// <summary>
/// Provides functionality for blocking apps by minimizing them when they get un-minimized.
/// </summary>
public abstract class AppBlockServiceBase : IAppBlockService, INotifyPropertyChanged
{
    private readonly List<IBlockedApp> blockedApps = new();

    /// <summary>
    /// Notifies when a property has changed.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Gets or sets a value indicating whether blocked apps are currently being blocked.
    /// </summary>
    public bool IsBlocking { get; protected set; }

    /// <summary>
    /// Gets a collection containing all the blocked apps.
    /// </summary>
    public virtual ReadOnlyCollection<IBlockedApp> BlockedApps => blockedApps.AsReadOnly();

    /// <summary>
    /// Adds an app to be blocked.
    /// </summary>
    /// <param name="app">The app to add to the blocked app list.</param>
    public virtual void Add(IBlockedApp app)
    {
        if (blockedApps.Contains(app))
        {
            return;
        }

        blockedApps.Add(app);
        OnPropertyChanged(nameof(BlockedApps));
    }

    /// <summary>
    /// Removes an app from being blocked.
    /// </summary>
    /// <param name="app">The app to remove from the blocked app list.</param>
    public virtual void Remove(IBlockedApp app)
    {
        if (!blockedApps.Contains(app))
        {
            return;
        }

        blockedApps.Remove(app);
        OnPropertyChanged(nameof(BlockedApps));
    }

    /// <summary>
    /// Blocks all apps in the blocked app list.
    /// </summary>
    public virtual void Block()
    {
        if (IsBlocking)
        {
            return;
        }

        IsBlocking = true;
    }

    /// <summary>
    /// Unblocks all apps in the blocked app list.
    /// </summary>
    public virtual void Unblock()
    {
        if (!IsBlocking)
        {
            return;
        }

        IsBlocking = false;
    }

    /// <summary>
    /// Invokes a <see cref="PropertyChanged"/> event for certain property.
    /// </summary>
    /// <param name="propertyName">The property that changed.</param>
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
