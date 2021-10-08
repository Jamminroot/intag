# InTag

> **To organize things even better** check out my other tool I've been developing. It's actually quite good :)
> 
> [GitHub project](https://github.com/Jamminroot/multistack_launcher)

> **SUGGESTIONS ARE WELOME, SO ARE DONATIONS**, as they both boost my motivattion to improve those tools :+1:

Add tags from explorer context menu. Can scan "neighbours" and use their tags. 

Aimed to be small and kind of lightweight (yet I believe code is fairly bad :D)

![Context Menu Example](images/ContextMenu.png)

![Main Window Example](images/Window.png)

![Tagged Folders Example](images/Result.png)

## Installation

### Via argument

Run exe with `install` arguement

### Manually

Put .exe somewhere, make sure it's executable - I did try to fix those permissions, but did not bother too much tbh (only spent like 2 hrs on this including "investigation").
Alternativelyyou add registry entry (don't forget to fix your path):

> **NOTE**: This does not include icon. For icon refer to the code below

in `HKEY_CLASSES_ROOT\Folder\shell\InTag\command`, replace Default in with `C:\\<PUT YOUR PATH HERE>\\intag.exe %1`


Or just run this regedit (also included as a reg, but again - don't forget to fix path):

```reg
Windows Registry Editor Version 5.00

[HKEY_CLASSES_ROOT\Folder\shell\InTag]
"Icon"="C:\\<PUT YOUR PATH HERE>\\intag.exe"

[HKEY_CLASSES_ROOT\Folder\shell\InTag\command]
@="C:\\<PUT YOUR PATH HERE>\\intag.exe %1"
```

## Uninstall

### Via argument

Run exe with `uninstall` argument

### Manually

Remove `HKEY_CLASSES_ROOT\Folder\shell\InTag` entry