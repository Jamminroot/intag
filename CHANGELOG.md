## [v2.6.96.0] - 2026-09-30

What's new in v2.6.96.0:
  Bug Fixes:
  - Fixed the window looking tiny on displays that use Windows scaling above 100% (#46). InTag told Windows
  it would scale itself but never did, so on 125% or 150% displays - common on 1080p monitors - it stayed
  small while everything around it grew. It now follows the display's scale and the monitor it opens on.
  - Fixed the InTag context menu entry missing on .ogg, .oga and .opus files. Ogg tagging arrived in 2.5,
  but these extensions were never registered for the context menu, so on Store installs there was no way
  to open InTag on them from Explorer.
  New Features:
  - Added tagging support for WebP images (#45). Windows reads tags from .webp files but refuses to write
  them, which is why tags used to disappear. InTag now writes the XMP metadata block itself, and Explorer
  shows the result directly: Tags, Title and Authors are supported. Other properties are not surfaced by
  Windows for this format and are reported as failures instead of being silently dropped.
  - Added tagging support for WebM and MKV video, plus MKA and WEBA audio (#45). As with WebP, Windows
  reads Matroska tags but cannot write them; InTag now writes them itself. Tags, Title and Comments are
  supported, and existing metadata such as per-track durations is preserved.
  - Added a zoom setting (#46): Appearance > Zoom offers 100% to 200%, or use Ctrl+= and Ctrl+- in the
  window. The window, the right-click menu and the dialogs all scale together, and the choice is remembered.
  - The window can now be resized horizontally by dragging its left or right edge, and its width is
  remembered between sessions (#46).

Note: GIF files still cannot be tagged. Windows neither writes nor reads metadata for GIF, so tags written
by any tool would stay invisible in Explorer.

## [v2.5.87.0] - 2026-08-26

What's new in v2.5.87.0:
  Bug Fixes:
  - Fixed settings not persisting when the UI was opened from the Windows 11 context menu. The menu launches
  InTag with package identity, so registry virtualization redirected its HKCU writes into the package's private
  hive — invisible to a direct launch of the exe. Tab visibility, theme and transparency choices made from the
  context menu silently reverted. Registry write virtualization is now disabled in the sparse package manifest.
  - Fixed Explorer not showing new tags until the search indexer caught up. Tag and property writes now notify the
  shell directly, so the columns refresh right after saving, for both files and folders.
  - Fixed the window rendering as a transparent hole on remote desktop sessions and on Windows 11 builds before
  22621, where the requested DWM backdrop material is not drawn. Those systems now fall back to a solid background.

Note: the context menu registration has to be refreshed for the settings fix to take effect - run InTag and
reinstall the context menu, or run `intag.exe --install`, after updating.

## [v2.5.84.0] - 2026-07-31

What's new in v2.5.84.0:
  Bug Fixes:
  - report metadata writes that fail instead of failing silently (#43)
  - remove shell extension from GUI uninstall path (#42)
  New Features:
  - write Vorbis comments for .ogg/.oga/.opus (#44)
  Maintenance:
  - ci: collapse the two-stage release into a single workflow
  - docs: correct msix tracking note and record the build-number pitfall
  - chore: bump display version to 2.5

## [v2.4.77.0] - 2026-05-27

- Write desktop.ini as UTF-16 LE so Explorer renders non-ASCII correctly

## [v2.4.75.0] - 2026-04-17

- Fix changelog generation heredoc in release workflow
- Preserve existing desktop.ini entries when writing folder metadata
- Add .pdf to supported file extensions for context menu (fixes #38)
- Restructure pipelines for 100% manual triggering, rc tagging, and auto-computed changelogs

## [v2.4.71.0] - 2026-02-22

**Full Changelog**: https://github.com/Jamminroot/intag/compare/2.4.31.0...v2.4.71.0

## [v2.4.70.0] - 2026-02-22

**Full Changelog**: https://github.com/Jamminroot/intag/compare/2.4.31.0...v2.4.70.0

## [v2.4.69.0] - 2026-02-22

**Full Changelog**: https://github.com/Jamminroot/intag/compare/2.4.31.0...v2.4.69.0

## [v2.4.68.0] - 2026-02-22

**Full Changelog**: https://github.com/Jamminroot/intag/compare/2.4.31.0...v2.4.68.0

## [v2.4.67.0] - 2026-02-22

**Full Changelog**: https://github.com/Jamminroot/intag/compare/2.4.31.0...v2.4.67.0

## [v2.4.66.0] - 2026-02-22

**Full Changelog**: https://github.com/Jamminroot/intag/compare/2.4.31.0...v2.4.66.0

## [v2.4.65.0] - 2026-02-22

**Full Changelog**: https://github.com/Jamminroot/intag/compare/2.4.31.0...v2.4.65.0

## [v2.4.64.0] - 2026-02-22

**Full Changelog**: https://github.com/Jamminroot/intag/compare/2.4.31.0...v2.4.64.0

## [v2.4.62.0] - 2026-02-22

**Full Changelog**: https://github.com/Jamminroot/intag/compare/2.4.31.0...v2.4.62.0

## [v2.4.63.0] - 2026-02-22

**Full Changelog**: https://github.com/Jamminroot/intag/compare/2.4.31.0...v2.4.63.0

## [v2.4.63.0] - 2026-02-21

**Full Changelog**: https://github.com/Jamminroot/intag/compare/2.4.31.0...v2.4.63.0

## [v2.4.62.0] - 2026-02-21

**Full Changelog**: https://github.com/Jamminroot/intag/compare/2.4.31.0...v2.4.62.0

## [v2.4.61.0] - 2026-02-21

**Full Changelog**: https://github.com/Jamminroot/intag/compare/2.4.31.0...v2.4.61.0

## [v2.4.60.0] - 2026-02-21

**Full Changelog**: https://github.com/Jamminroot/intag/compare/2.4.31.0...v2.4.60.0

## [v2.4.59.0] - 2026-02-21

**Full Changelog**: https://github.com/Jamminroot/intag/compare/2.4.31.0...v2.4.59.0

## [v2.4.58.0] - 2026-02-21

**Full Changelog**: https://github.com/Jamminroot/intag/compare/2.4.31.0...v2.4.58.0

## [v2.4.57.0] - 2026-02-21

**Full Changelog**: https://github.com/Jamminroot/intag/compare/2.4.31.0...v2.4.57.0

## [v2.4.56.0] - 2026-02-21

**Full Changelog**: https://github.com/Jamminroot/intag/compare/2.4.31.0...v2.4.56.0

## [v2.4.55.0] - 2026-02-21

**Full Changelog**: https://github.com/Jamminroot/intag/compare/2.4.31.0...v2.4.55.0

## [v2.4.54.0] - 2026-02-21

**Full Changelog**: https://github.com/Jamminroot/intag/compare/2.4.31.0...v2.4.54.0

## [v2.4.53.0] - 2026-02-21

**Full Changelog**: https://github.com/Jamminroot/intag/compare/2.4.31.0...v2.4.53.0

## [v2.4.52.0] - 2026-02-20

**Full Changelog**: https://github.com/Jamminroot/intag/compare/2.4.31.0...v2.4.52.0


