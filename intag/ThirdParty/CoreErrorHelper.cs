namespace intag.ThirdParty
{
 internal static class CoreErrorHelper
    {
        /// <summary>This is intended for Library Internal use only.</summary>
        public const int Ignored = (int)HResult.Ok;

        /// <summary>This is intended for Library Internal use only.</summary>
        private const int FacilityWin32 = 7;

        /// <summary>This is intended for Library Internal use only.</summary>
        /// <param name="result">The error code.</param>
        /// <returns>True if the error code indicates failure.</returns>
        public static bool Failed(HResult result) => !Succeeded(result);

        /// <summary>This is intended for Library Internal use only.</summary>
        /// <param name="result">The error code.</param>
        /// <returns>True if the error code indicates failure.</returns>
        public static bool Failed(int result) => !Succeeded(result);

        /// <summary>This is intended for Library Internal use only.</summary>
        /// <param name="win32ErrorCode">The Windows API error code.</param>
        /// <returns>The equivalent HRESULT.</returns>
        public static int HResultFromWin32(int win32ErrorCode)
        {
            if (win32ErrorCode > 0)
            {
                win32ErrorCode =
                    (int)(((uint)win32ErrorCode & 0x0000FFFF) | (FacilityWin32 << 16) | 0x80000000);
            }
            return win32ErrorCode;
        }

        /// <summary>This is intended for Library Internal use only.</summary>
        /// <param name="result">The COM error code.</param>
        /// <param name="win32ErrorCode">The Win32 error code.</param>
        /// <returns>Inticates that the Win32 error code corresponds to the COM error code.</returns>
        public static bool Matches(int result, int win32ErrorCode) => (result == HResultFromWin32(win32ErrorCode));

        /// <summary>This is intended for Library Internal use only.</summary>
        /// <param name="result">The error code.</param>
        /// <returns>True if the error code indicates success.</returns>
        public static bool Succeeded(int result) => result >= 0;

        /// <summary>This is intended for Library Internal use only.</summary>
        /// <param name="result">The error code.</param>
        /// <returns>True if the error code indicates success.</returns>
        public static bool Succeeded(HResult result) => Succeeded((int)result);
    }
}