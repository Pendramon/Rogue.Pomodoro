using Microsoft.Win32;
using Rogue.Pomodoro.Core;
using Rogue.Pomodoro.Core.Interfaces;
using Rogue.Pomodoro.WPF.Commands;
using Rogue.Pomodoro.WPF.ViewModels.Base;
using Rogue.Pomodoro.WPF.ViewModels.MainContent.Interfaces;
using System.IO;
using System.Windows.Input;

namespace Rogue.Pomodoro.WPF.ViewModels.MainContent;

public class SettingsViewModel : ViewModelBase, ISettingsViewModel
{
    public SettingsViewModel(IAppBlockService appBlockService)
    {
        AppBlockService = appBlockService;
        OpenSelectFileDialog = new RelayCommand(SelectFile);
        DeleteSelectedFile = new RelayCommand(DeleteFile);
    }

    public IAppBlockService AppBlockService { get; }

    public IBlockedApp? SelectedApplication { get; set; }

    public ICommand OpenSelectFileDialog { get; }

    public ICommand DeleteSelectedFile { get; }

    private void SelectFile()
    {
        var dialog = new OpenFileDialog
        {
            Filter = "Exe Files (.exe)|*.exe",
            Multiselect = true,
        };
        var result = dialog.ShowDialog();
        if (result is false)
        {
            return;
        }

        AppBlockService.Add(new BlockedApp(Path.GetFileNameWithoutExtension(dialog.FileName)));
    }

    private void DeleteFile()
    {
        if (SelectedApplication is null)
        {
            return;
        }

        AppBlockService.Remove(SelectedApplication);
    }
}
