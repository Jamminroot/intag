# InTag

> POLL📢[Update Notification Feature](https://github.com/Jamminroot/intag/discussions/17)

InTag is a tool that allows you to add tags to folders and files from the explorer context menu. It can also scan nearby files and use their tags for convenience. The tool is designed to be small and lightweight.

## Features

- Add tags to folders and files from the explorer context menu
- Scan nearby files and use their tags for convenience
- Small and lightweight design
- *NEW* - added support of CLI usage

## Usage (UI)

To use InTag, follow these steps:

1. Right-click the folder (on Windows 11, you may need to press ' Show more options'), and select InTag.

![Context Menu Example](images/ContextMenu.png)

2. Assign the desired tags to a folder (or file, if it supports System.Keywords metadata). The neighboring tags will be included in the list of available tags.

![Main Window Example](images/Window.png)

3. Once you have assigned the tags, click back to the folder or press the enter key to apply the changes. The tags will be assigned, but it may take some time for explorer to detect the changes. To cancel press the esc key. 

   NB. The tag input field should be focused and empty for the enter key worked.

![Tagged Folders Example](images/Result.png)

![Tagged Folders Example - 2](images/Result2.png)

4. Ensure that you have enabled grouping by tags by selecting "Context menu on the folder background > group by > More... > Select 'Tags' in the list".

## Usage (CLI)

```ps1
# Add tags:
.\intag.exe --add "test_tag" --path "C:\path\to\folder\or\file"
# Remove tags:
.\intag.exe --remove "test_tag" --list "C:\path\to\list\file"
# Add And Remove tags (will combine files in list file and argument of "--path")::
.\intag.exe  --add "test_tag" --remove "test_tag" --list "C:\path\to\list\file" --path "C:\path\to\folder\or\file"
```

## Installation

There are three methods for installing InTag:

1. Automagical: Start the application once, and it should install itself automatically.
2. Almost automagical: Right-click the application icon in explorer, and run the application with administrator rights.

![Run as admin](images/RunAsAdmin.png)

3. Manually: Place the .exe file in a directory of your choice, or add a registry entry.

## Registry Entry Snippets

To add the registry entry, use the following code snippets:

For (only) folders:
```reg
Windows Registry Editor Version 5.00

[HKEY_CLASSES_ROOT\Folder\shell\InTag]
"Icon"="\"C:\\<PUT YOUR PATH HERE>\\intag.exe\""

[HKEY_CLASSES_ROOT\Folder\shell\InTag\command]
@="\"C:\\<PUT YOUR PATH HERE>\\intag.exe\" --ui --path \"%1\""
```

For all files:
```reg
Windows Registry Editor Version 5.00

[HKEY_CLASSES_ROOT\*\shell\InTag]
"Icon"="\"C:\\<PUT YOUR PATH HERE>\\intag.exe\""

[HKEY_CLASSES_ROOT\*\shell\InTag\command]
@="\"C:\\<PUT YOUR PATH HERE>\\intag.exe\" --ui --path \"%1\""
```

## Uninstall

There are two methods for uninstalling InTag:

1. Via argument: Run the exe file with the `--uninstall` or `-u` argument.
2. Manually: Remove the `HKEY_CLASSES_ROOT\Folder\shell\InTag` and(or) `HKEY_CLASSES_ROOT\*\shell\InTag` entries.

## Contributions

[montoner0](https://github.com/montoner0) - great PR with bunch of improvements and fixes, also nice suggestions

## Third-Party Notice

The code for individual file management was taken from the Windows API Code Pack.

## Additional Tool

For even better organization, check out Multistack Launcher,
