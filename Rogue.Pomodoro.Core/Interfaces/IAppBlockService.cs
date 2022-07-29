using System.Collections.ObjectModel;

namespace Rogue.Pomodoro.Core.Interfaces;

public interface IAppBlockService
{
    public bool IsBlocking { get; }

    public ReadOnlyCollection<IBlockedApp> BlockedApps { get; }

    public void Add(IBlockedApp app);

    public void Remove(IBlockedApp app);

    public void Block();

    public void Unblock();
}