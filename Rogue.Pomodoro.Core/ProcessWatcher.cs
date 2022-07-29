using Rogue.Pomodoro.Core.Interfaces;
using System;
using System.Diagnostics;
using System.Management;

namespace Rogue.Pomodoro.Core;

internal sealed class ProcessWatcher : IProcessWatcher
{
    private ManagementEventWatcher creationWatcher;
    private ManagementEventWatcher deletionWatcher;

    public ProcessWatcher()
    {
        // The polling rate in milliseconds
        const int pollingRate = 1;
        var managementScope = new ManagementScope(ManagementPath.DefaultPath);
        var creationQuery = new WqlEventQuery($"SELECT * FROM __InstanceCreationEvent WITHIN {pollingRate} WHERE TargetInstance ISA 'Win32_Process'");
        creationWatcher = new ManagementEventWatcher(managementScope, creationQuery);
        creationWatcher.EventArrived += OnEventArrived;
        creationWatcher.Start();
        var deletionQuery = new WqlEventQuery($"SELECT * FROM __InstanceDeletionEvent WITHIN {pollingRate} WHERE TargetInstance ISA 'Win32_Process'");
        deletionWatcher = new ManagementEventWatcher(managementScope, deletionQuery);
        deletionWatcher.EventArrived += OnEventArrived;
        deletionWatcher.Start();
    }

    public event EventHandler<ProcessEventArgs>? Started;

    public event EventHandler<ProcessEventArgs>? Terminated;

    public void Dispose()
    {
        Dispose(true);
    }

    private void Dispose(bool disposing)
    {
        if (!disposing)
        {
            return;
        }

        // The dispose method should be able to be called multiple times without any issues.
        if (creationWatcher is not null)
        {
            creationWatcher.EventArrived -= OnEventArrived;
            creationWatcher.Stop();
            creationWatcher.Dispose();
            creationWatcher = null!;
        }

        if (deletionWatcher is null)
        {
            return;
        }

        deletionWatcher.EventArrived -= OnEventArrived;
        deletionWatcher.Stop();
        deletionWatcher.Dispose();
        deletionWatcher = null!;
    }

    private void OnEventArrived(object sender, EventArrivedEventArgs e)
    {
        try
        {
            var eventName = e.NewEvent.ClassPath.ClassName;
            var managementBaseObject = (ManagementBaseObject)e.NewEvent.Properties["TargetInstance"].Value;

            // All processes have a Process Id and Process Name.
            var processName = managementBaseObject.Properties["Name"].Value.ToString()!;
            var processId = int.Parse(managementBaseObject.Properties["ProcessId"].Value.ToString()!);
            if (string.Compare(eventName, "__InstanceCreationEvent", StringComparison.Ordinal) == 0)
            {
                Started?.Invoke(this, new ProcessEventArgs(processId, processName));
            }
            else if (string.Compare(eventName, "__InstanceDeletionEvent", StringComparison.Ordinal) == 0)
            {
                Terminated?.Invoke(this, new ProcessEventArgs(processId, processName));
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }
}