using System;

namespace intag
{
        internal enum ShellItemDesignNameOptions
        {
            Normal = 0x00000000,           // SIGDN_NORMAL
            ParentRelativeParsing = unchecked((int)0x80018001),   // SIGDN_INFOLDER | SIGDN_FORPARSING
            DesktopAbsoluteParsing = unchecked((int)0x80028000),  // SIGDN_FORPARSING
            ParentRelativeEditing = unchecked((int)0x80031001),   // SIGDN_INFOLDER | SIGDN_FOREDITING
            DesktopAbsoluteEditing = unchecked((int)0x8004c000),  // SIGDN_FORPARSING | SIGDN_FORADDRESSBAR
            FileSystemPath = unchecked((int)0x80058000),             // SIGDN_FORPARSING
            Url = unchecked((int)0x80068000),                     // SIGDN_FORPARSING
            ParentRelativeForAddressBar = unchecked((int)0x8007c001),     // SIGDN_INFOLDER | SIGDN_FORPARSING | SIGDN_FORADDRESSBAR
            ParentRelative = unchecked((int)0x80080001)           // SIGDN_INFOLDER
        }

        [Flags]
        internal enum ShellFileGetAttributesOptions
        {
            /// <summary>The specified items can be copied.</summary>
            CanCopy = 0x00000001,

            /// <summary>The specified items can be moved.</summary>
            CanMove = 0x00000002,

            /// <summary>
            /// Shortcuts can be created for the specified items. This flag has the same value as DROPEFFECT. The normal use of this flag is
            /// to add a Create Shortcut item to the shortcut menu that is displayed during drag-and-drop operations. However, SFGAO_CANLINK
            /// also adds a Create Shortcut item to the Microsoft Windows Explorer's File menu and to normal shortcut menus. If this item is
            /// selected, your application's IContextMenu::InvokeCommand is invoked with the lpVerb member of the CMINVOKECOMMANDINFO
            /// structure set to "link." Your application is responsible for creating the link.
            /// </summary>
            CanLink = 0x00000004,

            /// <summary>The specified items can be bound to an IStorage interface through IShellFolder::BindToObject.</summary>
            Storage = 0x00000008,

            /// <summary>The specified items can be renamed.</summary>
            CanRename = 0x00000010,

            /// <summary>The specified items can be deleted.</summary>
            CanDelete = 0x00000020,

            /// <summary>The specified items have property sheets.</summary>
            HasPropertySheet = 0x00000040,

            /// <summary>The specified items are drop targets.</summary>
            DropTarget = 0x00000100,

            /// <summary>This flag is a mask for the capability flags.</summary>
            CapabilityMask = 0x00000177,

            /// <summary>Windows 7 and later. The specified items are system items.</summary>
            System = 0x00001000,

            /// <summary>The specified items are encrypted.</summary>
            Encrypted = 0x00002000,

            /// <summary>
            /// Indicates that accessing the object = through IStream or other storage interfaces, is a slow operation. Applications should
            /// avoid accessing items flagged with SFGAO_ISSLOW.
            /// </summary>
            IsSlow = 0x00004000,

            /// <summary>The specified items are ghosted icons.</summary>
            Ghosted = 0x00008000,

            /// <summary>The specified items are shortcuts.</summary>
            Link = 0x00010000,

            /// <summary>The specified folder objects are shared.</summary>
            Share = 0x00020000,

            /// <summary>
            /// The specified items are read-only. In the case of folders, this means that new items cannot be created in those folders.
            /// </summary>
            ReadOnly = 0x00040000,

            /// <summary>
            /// The item is hidden and should not be displayed unless the Show hidden files and folders option is enabled in Folder Settings.
            /// </summary>
            Hidden = 0x00080000,

            /// <summary>This flag is a mask for the display attributes.</summary>
            DisplayAttributeMask = 0x000FC000,

            /// <summary>The specified folders contain one or more file system folders.</summary>
            FileSystemAncestor = 0x10000000,

            /// <summary>The specified items are folders.</summary>
            Folder = 0x20000000,

            /// <summary>
            /// The specified folders or file objects are part of the file system that is, they are files, directories, or root directories).
            /// </summary>
            FileSystem = 0x40000000,

            /// <summary>The specified folders have subfolders = and are, therefore, expandable in the left pane of Windows Explorer).</summary>
            HasSubFolder = unchecked((int)0x80000000),

            /// <summary>This flag is a mask for the contents attributes.</summary>
            ContentsMask = unchecked((int)0x80000000),

            /// <summary>
            /// When specified as input, SFGAO_VALIDATE instructs the folder to validate that the items pointed to by the contents of apidl
            /// exist. If one or more of those items do not exist, IShellFolder::GetAttributesOf returns a failure code. When used with the
            /// file system folder, SFGAO_VALIDATE instructs the folder to discard cached properties retrieved by clients of
            /// IShellFolder2::GetDetailsEx that may have accumulated for the specified items.
            /// </summary>
            Validate = 0x01000000,

            /// <summary>The specified items are on removable media or are themselves removable devices.</summary>
            Removable = 0x02000000,

            /// <summary>The specified items are compressed.</summary>
            Compressed = 0x04000000,

            /// <summary>The specified items can be browsed in place.</summary>
            Browsable = 0x08000000,

            /// <summary>The items are nonenumerated items.</summary>
            Nonenumerated = 0x00100000,

            /// <summary>The objects contain new content.</summary>
            NewContent = 0x00200000,

            /// <summary>It is possible to create monikers for the specified file objects or folders.</summary>
            CanMoniker = 0x00400000,

            /// <summary>Not supported.</summary>
            HasStorage = 0x00400000,

            /// <summary>
            /// Indicates that the item has a stream associated with it that can be accessed by a call to IShellFolder::BindToObject with
            /// IID_IStream in the riid parameter.
            /// </summary>
            Stream = 0x00400000,

            /// <summary>
            /// Children of this item are accessible through IStream or IStorage. Those children are flagged with SFGAO_STORAGE or SFGAO_STREAM.
            /// </summary>
            StorageAncestor = 0x00800000,

            /// <summary>This flag is a mask for the storage capability attributes.</summary>
            StorageCapabilityMask = 0x70C50008,

            /// <summary>
            /// Mask used by PKEY_SFGAOFlags to remove certain values that are considered to cause slow calculations or lack context. Equal
            /// to SFGAO_VALIDATE | SFGAO_ISSLOW | SFGAO_HASSUBFOLDER.
            /// </summary>
            PkeyMask = unchecked((int)0x81044000),
        }

        /// <summary>
        /// Indicate flags that modify the property store object retrieved by methods that create a property store, such as
        /// IShellItem2::GetPropertyStore or IPropertyStoreFactory::GetPropertyStore.
        /// </summary>
        [Flags]
        internal enum GetPropertyStoreOptions
        {
            /// <summary>
            /// Meaning to a calling process: Return a read-only property store that contains all properties. Slow items (offline files) are
            /// not opened. Combination with other flags: Can be overridden by other flags.
            /// </summary>
            Default = 0,

            /// <summary>
            /// Meaning to a calling process: Include only properties directly from the property handler, which opens the file on the disk,
            /// network, or device. Meaning to a file
            /// folder: Only include properties directly from the handler.
            ///
            /// Meaning to other folders: When delegating to a file folder, pass this flag on to the file folder; do not do any multiplexing
            /// (MUX). When not delegating to a file folder, ignore this flag instead of returning a failure code.
            ///
            /// Combination with other flags: Cannot be combined with GPS_TEMPORARY, GPS_FASTPROPERTIESONLY, or GPS_BESTEFFORT.
            /// </summary>
            HandlePropertiesOnly = 0x1,

            /// <summary>
            /// Meaning to a calling process: Can write properties to the item.
            /// Note: The store may contain fewer properties than a read-only store.
            ///
            /// Meaning to a file folder: ReadWrite.
            ///
            /// Meaning to other folders: ReadWrite. Note: When using default MUX, return a single unmultiplexed store because the default
            /// MUX does not support ReadWrite.
            ///
            /// Combination with other flags: Cannot be combined with GPS_TEMPORARY, GPS_FASTPROPERTIESONLY, GPS_BESTEFFORT, or
            /// GPS_DELAYCREATION. Implies GPS_HANDLERPROPERTIESONLY.
            /// </summary>
            ReadWrite = 0x2,

            /// <summary>
            /// Meaning to a calling process: Provides a writable store, with no initial properties, that exists for the lifetime of the
            /// Shell item instance; basically, a property bag attached to the item instance.
            ///
            /// Meaning to a file folder: Not applicable. Handled by the Shell item.
            ///
            /// Meaning to other folders: Not applicable. Handled by the Shell item.
            ///
            /// Combination with other flags: Cannot be combined with any other flag. Implies GPS_READWRITE
            /// </summary>
            Temporary = 0x4,

            /// <summary>
            /// Meaning to a calling process: Provides a store that does not involve reading from the disk or network. Note: Some values may
            /// be different, or missing, compared to a store without this flag.
            ///
            /// Meaning to a file folder: Include the "innate" and "fallback" stores only. Do not load the handler.
            ///
            /// Meaning to other folders: Include only properties that are available in memory or can be computed very quickly (no properties
            /// from disk, network, or peripheral IO devices). This is normally only data sources from the IDLIST. When delegating to other
            /// folders, pass this flag on to them.
            ///
            /// Combination with other flags: Cannot be combined with GPS_TEMPORARY, GPS_READWRITE, GPS_HANDLERPROPERTIESONLY, or GPS_DELAYCREATION.
            /// </summary>
            FastPropertiesOnly = 0x8,

            /// <summary>
            /// Meaning to a calling process: Open a slow item (offline file) if necessary. Meaning to a file folder: Retrieve a file from
            /// offline storage, if necessary.
            /// Note: Without this flag, the handler is not created for offline files.
            ///
            /// Meaning to other folders: Do not return any properties that are very slow.
            ///
            /// Combination with other flags: Cannot be combined with GPS_TEMPORARY or GPS_FASTPROPERTIESONLY.
            /// </summary>
            OpensLowItem = 0x10,

            /// <summary>
            /// Meaning to a calling process: Delay memory-intensive operations, such as file access, until a property is requested that
            /// requires such access.
            ///
            /// Meaning to a file folder: Do not create the handler until needed; for example, either GetCount/GetAt or GetValue, where the
            /// innate store does not satisfy the request.
            /// Note: GetValue might fail due to file access problems.
            ///
            /// Meaning to other folders: If the folder has memory-intensive properties, such as delegating to a file folder or network
            /// access, it can optimize performance by supporting IDelayedPropertyStoreFactory and splitting up its properties into a fast
            /// and a slow store. It can then use delayed MUX to recombine them.
            ///
            /// Combination with other flags: Cannot be combined with GPS_TEMPORARY or GPS_READWRITE
            /// </summary>
            DelayCreation = 0x20,

            /// <summary>
            /// Meaning to a calling process: Succeed at getting the store, even if some properties are not returned. Note: Some values may
            /// be different, or missing, compared to a store without this flag.
            ///
            /// Meaning to a file folder: Succeed and return a store, even if the handler or innate store has an error during creation. Only
            /// fail if substores fail.
            ///
            /// Meaning to other folders: Succeed on getting the store, even if some properties are not returned.
            ///
            /// Combination with other flags: Cannot be combined with GPS_TEMPORARY, GPS_READWRITE, or GPS_HANDLERPROPERTIESONLY.
            /// </summary>
            BestEffort = 0x40,

            /// <summary>Mask for valid GETPROPERTYSTOREFLAGS values.</summary>
            MaskValid = 0xff,
        }
}