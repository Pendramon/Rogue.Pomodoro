using Microsoft.Extensions.DependencyInjection;
using Rogue.Pomodoro.Core.Interfaces;

namespace Rogue.Pomodoro.Core;

public static class ServiceCollectionExtensions
{
    public static void AddPomodoroCore(this IServiceCollection services)
    {
        services.AddSingleton<IProcessWatcher, ProcessWatcher>();
        services.AddSingleton<IWindowEventListener, WindowEventListener>();
        services.AddScoped<IAppBlockService, WinEventHookAppBlockService>();
        services.AddScoped<IPomodoroTimer, PomodoroTimer>();
    }
}