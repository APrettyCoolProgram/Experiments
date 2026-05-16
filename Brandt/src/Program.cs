// 260515_code
// 260515_documentation

using System;
using Avalonia;

namespace Brandt;

/// <summary>Provides the application entry point and Avalonia configuration.</summary>
/// <remarks>
/// This class hosts the <see cref="Main"/> method that bootstraps the Avalonia application and the
/// <see cref="BuildAvaloniaApp"/> method used by both the runtime and the visual designer to configure the
/// <see cref="AppBuilder"/>.
/// </remarks>
class Program
{
    /// <summary>The application entry point that starts the Avalonia classic desktop lifetime.</summary>
    /// <remarks>
    /// Builds the Avalonia application via <see cref="BuildAvaloniaApp"/> and starts it with the classic
    /// desktop lifetime.<br/>
    /// <br/>
    /// Marked with <see cref="STAThreadAttribute"/> because Avalonia requires a single-threaded apartment on
    /// Windows.<br/>
    /// <br/>
    /// Do not invoke any Avalonia, third-party APIs, or <see cref="System.Threading.SynchronizationContext"/>-reliant
    /// code before this method runs.
    /// </remarks>
    /// <param name="args">The command-line arguments passed to the application.</param>
    [STAThread]
    public static void Main(string[] args)
    {
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    /// <summary>Builds and configures the Avalonia <see cref="AppBuilder"/> for the <see cref="App"/>.</summary>
    /// <remarks>
    /// Configures the application with platform auto-detection, developer tools, the Inter font, and trace
    /// logging.<br/>
    /// <list type="bullet">
    /// <item><b>Configure&lt;App&gt;</b> - Specifies the App class as the root Avalonia application type.</item>
    /// <item><b>UsePlatformDetect</b> - Automatically detects and uses the appropriate platform backend (Windows, macOS, Linux).</item>
    /// <item><b>WithDeveloperTools</b> - Enables Avalonia's developer tools (e.g., the DevTools window via F12) for diagnostics.</item>
    /// <item><b>WithInterFont</b> - Registers the bundled Inter font as an embedded asset for use in the UI.</item>
    /// <item><b>LogToTrace</b> - Routes Avalonia's internal logging output to System.Diagnostics.Trace.</item>
    /// </list>
    /// This method is also consumed by the Avalonia visual designer and must not be removed or renamed.
    /// </remarks>
    /// <returns>A configured <see cref="AppBuilder"/> instance ready to be started.</returns>
    public static AppBuilder BuildAvaloniaApp()
    {
        return AppBuilder.Configure<App>()
                         .UsePlatformDetect()
                         .WithDeveloperTools()
                         .WithInterFont()
                         .LogToTrace();
    }
}