# Intag: Files & Folders

InTag is a lightweight tool for tagging files and folders directly from the Windows Explorer context menu. Nearby tags are suggested automatically for quick reuse.

<a href="https://apps.microsoft.com/store/detail/9PB72S2JQ5DF"><img src="https://get.microsoft.com/images/en-us%20dark.svg" width="200"/></a>

## Features

- Tag files and folders from the Explorer context menu
- Multiple metadata properties: Tags, Title, Subject, Author, Comments, Category, Company, Street, and more
- Suggested tags from nearby files for quick reuse
- Toggle column visibility and grouping per folder with Ctrl+F / Ctrl+D
- CLI support for scripting and automation
- Works on Windows 10 and 11

## Usage

### UI

1. Right-click a folder or file and select InTag.

![Context Menu](images/ContextMenu.png)

2. Assign tags or edit metadata properties. Nearby tags appear as suggestions.

![Main Window](images/Window.png)

3. Press Enter or click away to apply. Press Esc to cancel.

![Tagged Folders](images/Result.png)

![Tagged Folders](images/Result2.png)

4. Right-click the InTag window to access settings: toggle properties, change appearance, and more.

![Properties](images/Properties.png)

5. Use Ctrl+F to toggle column visibility or Ctrl+D to toggle grouping for the current property in the parent folder.

![Folder Settings](images/FolderSettings.png)

### CLI

Tags are the default property. Use `--property` to work with other metadata.

```ps1
# Add tags to a folder:
intag --add "vacation" --add "2026" --path "C:\Photos\Summer"

# Remove a tag:
intag --remove "vacation" --path "C:\Photos\Summer"

# View current tags:
intag --path "C:\Photos\Summer"

# Set the author on a file:
intag --property author --add "John Doe" --path report.docx

# Set a category:
intag --property category --add "Work" --path project.docx

# Set a comment:
intag --property comments --add "Draft version, needs review" --path notes.docx

# Set company:
intag --property company --add "Acme Corp" --path invoice.docx

# Set street address metadata:
intag --property street --add "123 Main St" --path contact.docx

# Process multiple files from a list:
intag --add "archive" --list "C:\files_to_tag.txt"

# Open the UI for specific files:
intag --ui --path file1.txt --path file2.txt
```

#### Available Properties

| Property | CLI name | Type | Explorer Column |
|----------|----------|------|-----------------|
| Tags | `tags` (default) | Multi-value | Tags |
| Title | `title` | Single value | Title |
| Subject | `subject` | Single value | Subject |
| Author | `author` | Multi-value | Authors |
| Comments | `comments` | Free text | Comments |
| Category | `category` | Multi-value | Categories |
| Company | `company` | Single value | Company |
| Street | `street` | Single value | Business Street |
| Other Street | `otherstreet` | Single value | Other Street |

All properties work with both files and folders, are stored as standard Windows metadata, and are visible in Explorer's column view. Use Ctrl+F in the InTag UI to toggle column visibility, or Ctrl+D to toggle grouping.

## Installation

### Microsoft Store (recommended)
Install from the [Microsoft Store](https://apps.microsoft.com/store/detail/9PB72S2JQ5DF). Updates are handled automatically.

### Standalone
Download the latest release from [GitHub Releases](https://github.com/Jamminroot/intag/releases) and run `intag.exe`. It will register itself automatically on first launch.

## Uninstall

- **Store version**: Uninstall from Windows Settings > Apps, or right-click the app in the Start menu.
- **Standalone version**: Launch `intag.exe` directly (not from the context menu) and select the Uninstall option, or run with `--uninstall`.

## Contributions to the legacy version (still grateful)

[montoner0](https://github.com/montoner0) - great PR with bunch of improvements and fixes, also nice suggestions

## Third-Party Notice

The code for individual file management was taken from the Windows API Code Pack.
