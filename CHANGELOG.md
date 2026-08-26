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


