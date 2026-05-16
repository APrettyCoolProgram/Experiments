<div align="center">

  <h1>Brandt: Agent prompts</h1>

</div>

## 1. Initial setup

### Prompt

Let's create a simple, cross-platform text editor that we can build upon. The text editor will have the following features:

- Works on Windows, MacOS, and Linux
- Built using .NET 10 and C-Sharp
- Menu bar at the top with the following text: "File", "Settings" and "About"

### Response

- Cross-platform support comes from Avalonia + .UsePlatformDetect() already configured in Program.cs; the same build runs on Windows, macOS, and Linux.
- The menu uses Avalonia's Menu/MenuItem, which renders natively on each platform.
- File I/O uses the modern StorageProvider API so the picker works correctly across all three OSes.
- _currentFilePath is tracked so future "Save (without prompt)" behavior can be added easily.


> [!TIP]
> [Essential prompts](https://github.com/APrettyCoolProgram/TheDocumentationProject/blob/main/copilot/Copilot-EssentialPrompts.md)

***

<br>

<sub>Last updated: 260416 </sub>
