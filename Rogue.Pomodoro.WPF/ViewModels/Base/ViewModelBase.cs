using Rogue.Pomodoro.WPF.ViewModels.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Rogue.Pomodoro.WPF.ViewModels.Base;

public class ViewModelBase : IViewModel, INotifyPropertyChanged
{
    private readonly Dictionary<string, object?> fields = new();

    public event PropertyChangedEventHandler? PropertyChanged;

    public event EventHandler<ChildMessageArgs>? ChildMessage;

    /// <summary>
    /// Adds view models which are children of this parent view model by subscribing them to
    /// an event that allows the children to send messages to the parent.
    /// </summary>
    /// <param name="children">The children view models of this parent view model.</param>
    public void RegisterChildren(IEnumerable<IViewModel> children)
    {
        foreach (var child in children)
        {
            RegisterChild(child);
        }
    }

    /// <summary>
    /// Adds a view model as a child of this parent view model by subscribing it to
    /// an event that allows the child to send messages to the parent.
    /// </summary>
    /// <param name="child">The child view model of this parent view model.</param>
    public void RegisterChild(IViewModel child)
    {
        child.ChildMessage += OnChildMessage;
    }

    /// <summary>
    /// Removes the child from this parent view model and unsubscribes it from the message event.
    /// </summary>
    /// <param name="child">The child view model of this parent view model.</param>
    public void RemoveChild(IViewModel child)
    {
        child.ChildMessage -= OnChildMessage;
    }

    /// <summary>
    /// Removes the children from this parent view model and unsubscribes it from the message event.
    /// </summary>
    /// <param name="children">The children view models of this parent view model.</param>
    public void RemoveChildren(IEnumerable<IViewModel> children)
    {
        foreach (var child in children)
        {
            RemoveChild(child);
        }
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Gets the property value from a hidden backing field or default if it doesn't exist.
    /// </summary>
    /// <typeparam name="T">The property type.</typeparam>
    /// <param name="propertyName">The name of the property.</param>
    /// <returns>The value if the backing field has already been set, otherwise returns <see langword="default"/>.</returns>
    /// <exception cref="ArgumentException">Thrown if property name was <see langword="null"/>, empty or whitespace.</exception>
    protected T? GetProperty<T>([CallerMemberName] string propertyName = "")
    {
        if (string.IsNullOrWhiteSpace(propertyName))
        {
            throw new ArgumentException("Invalid property name.", nameof(propertyName));
        }

        if (!fields.ContainsKey(propertyName))
        {
            return default;
        }

        return (T?)fields[propertyName];
    }

    /// <summary>
    /// Sets the property to a hidden backing field and sends <see cref="PropertyChanged"/> event if its value has changed.
    /// </summary>
    /// <typeparam name="T">The property type.</typeparam>
    /// <param name="value">The property value.</param>
    /// <param name="propertyName">The name of the property.</param>
    /// <returns><see langword="true"/> if the <see cref="PropertyChanged"/> event was fired, <see langword="false"/> if the <see cref="PropertyChanged"/> event wasn't fired.</returns>
    /// <exception cref="ArgumentException">Thrown if property name was <see langword="null"/>, empty or whitespace.</exception>
    protected bool SetProperty<T>(T value, [CallerMemberName] string propertyName = "")
    {
        if (string.IsNullOrWhiteSpace(propertyName))
        {
            throw new ArgumentException("Invalid property name.", nameof(propertyName));
        }

        if (fields.ContainsKey(propertyName) && EqualityComparer<T>.Default.Equals(value, (T?)fields[propertyName]))
        {
            return false;
        }

        fields[propertyName] = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    /// <summary>
    /// Sets the property to a backing field and sends <see cref="PropertyChanged"/> event if its value has changed.
    /// </summary>
    /// <typeparam name="T">The property type.</typeparam>
    /// <param name="value">The property value.</param>
    /// <param name="storage">The backing field.</param>
    /// <param name="propertyName">The name of the property.</param>
    /// <returns><see langword="true"/> if the <see cref="PropertyChanged"/> event was fired, <see langword="false"/> if the <see cref="PropertyChanged"/> event wasn't fired.</returns>
    /// <exception cref="ArgumentException">Thrown if property name was <see langword="null"/>, empty or whitespace.</exception>
    protected bool SetProperty<T>(T value, ref T storage, [CallerMemberName] string propertyName = "")
    {
        if (string.IsNullOrWhiteSpace(propertyName))
        {
            throw new ArgumentException("Invalid property name.", nameof(propertyName));
        }

        if (fields.ContainsKey(propertyName) && EqualityComparer<T>.Default.Equals(value, storage))
        {
            return false;
        }

        storage = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    protected virtual void OnChildMessage(object? sender, ChildMessageArgs e)
    {
    }

    /// <summary>
    /// Sends a message to every child of the view model, since they all are subscribed to the same event (observer).
    /// </summary>
    /// <param name="key">The message type.</param>
    /// <param name="value">The message value.</param>
    protected void SendMessage(ChildMessageType key, object value)
    {
        ChildMessage?.Invoke(this, new ChildMessageArgs(key, value));
    }
}
