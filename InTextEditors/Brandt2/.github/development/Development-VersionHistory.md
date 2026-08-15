<div align="center">

  <h1>Brandt: Development - Version History</h1>

</div>

## R26.5.0.0-development+160726

> Update documentation, refactorMainWindow handlers

- Refactor MainWindow.axaml.cs:
  - add using System.Collections.Generic
  - extract New/Open logic into NewButtonClicked and OpenButtonClicked (Open now uses explicit pickerOptions and locals)
  - preserve original event handlers by forwarding them to the new methods
  - apply minor formatting/alignment changes

## R26.5.0.0-development+151601

> Documentation updates

## R26.5.0.0-development+151548

> Add basic editor UI and file I/O handlers

- Introduce initial editor scaffold (MainWindow.axaml):
  - add a top menu (File, Settings, About)
  - add main TextBox editing surface 
  - add code-behind handlers for New/Open/Save/Exit using Avalonia's StorageProvider - tracking the current file path for window title updates.
- Update project docs and metadata:
  - adjust agent prompts and version history entries
  - tweak README content/stage badge
  - bump the development build identifier in ProjectInfo.cs.

## R26.5.0.0-development+151539

> Documentation updates

Update documentation and improve Program.cs clarity and configuration.

- .github/agents/prompts.md: rename header to "Agent prompts" and fix markup.
- .github/development/DesignDocument.md: add a short description that Brandt is a cross-platform text editor.
- src/Program.cs: add XML documentation for the Program class and its methods, expand Main to a block form, and consolidate AppBuilder configuration (platform detection, developer tools, Inter font, trace logging).
- src/ProjectInfo.cs: bump development build metadata.

These edits improve maintainability and make the application bootstrap and tooling setup clearer for contributors.

## R26.5.0.0-development+151526

> Documentation updates

- Insert metadata comment headers into MainWindow.axaml, MainWindow.axaml.cs, and Program.cs.

## R26.5.0.0-development+151523

> Documentation updates

- Replace generic 'Repository:' headings with 'Brandt:' across documentation.
