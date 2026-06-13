// 260516_code
// 260516_documentation

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;

namespace Brandt;

public partial class MainWindow : Window
{
    private string? _currentFilePath;

    public MainWindow()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Clears the main editing surface and resets the current file state to prepare for creating a new document.
    /// </summary>
    /// <remarks>
    /// This method resets the editor to a blank state, discarding any unsaved changes. It is typically called when the
    /// user initiates the creation of a new document.
    /// </remarks>
    private void NewButtonClicked()
    {
        MainEditSurface.Text = string.Empty;
        _currentFilePath     = null;
        Title                = "Brandt";
    }

    /// <summary>
    /// Handles the Open button click event to prompt the user to select a file and loads its contents into the main
    /// editing surface.
    /// </summary>
    /// <remarks>If the user cancels the file picker or no file is selected, the method returns without making
    /// changes. The method updates the window title to reflect the opened file's name.</remarks>
    private async void OpenButtonClicked()
    {
        FilePickerOpenOptions pickerOptions = new FilePickerOpenOptions
        {
            Title         = "Open file",
            AllowMultiple = false
        };

        IReadOnlyList<IStorageFile> files = await StorageProvider.OpenFilePickerAsync(pickerOptions);

        IStorageFile? file = files?.FirstOrDefault();

        if (file is null)
        {
            return;
        }

        await using var stream = await file.OpenReadAsync();

        using var reader = new StreamReader(stream);

        MainEditSurface.Text = await reader.ReadToEndAsync();

        _currentFilePath = file.Path.LocalPath;

        Title = $"Brandt - {Path.GetFileName(_currentFilePath)}";
    }

    private async void OnSaveClicked(object? sender, RoutedEventArgs e)
    {
        var file = await StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Save file",
            SuggestedFileName = _currentFilePath is null
                ? "untitled.txt"
                : Path.GetFileName(_currentFilePath)
        });

        if (file is null)
            return;

        await using var stream = await file.OpenWriteAsync();
        await using var writer = new StreamWriter(stream);
        await writer.WriteAsync(MainEditSurface.Text ?? string.Empty);

        _currentFilePath = file.Path.LocalPath;
        Title = $"Brandt - {Path.GetFileName(_currentFilePath)}";
    }

    private void OnExitClicked(object? sender, RoutedEventArgs e)
    {
        if (Avalonia.Application.Current?.ApplicationLifetime
            is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.Shutdown();
        }
    }

    private void OnSettingsClicked(object? sender, RoutedEventArgs e)
    {
        // Placeholder for future settings dialog.
    }

    private void OnAboutClicked(object? sender, RoutedEventArgs e)
    {
        // Placeholder for future about dialog.
    }







    private void OnNewClicked(object? sender, RoutedEventArgs e) => NewButtonClicked();


    private async void OnOpenClicked(object? sender, RoutedEventArgs e) => OpenButtonClicked();
}