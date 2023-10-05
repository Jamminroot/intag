using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace intag
{
	public static class WindowUtils
	{
		public static void EnableAcrylic(IWin32Window window, Color blurColor)
		{
			unsafe
			{
				var accent = new AccentPolicy
				{
					AccentState = ACCENT.ENABLE_ACRYLICBLURBEHIND,
					GradientColor = ToAbgr(blurColor)
				};

				var accentStructSize = Marshal.SizeOf(accent);
				var accentPtr = Marshal.AllocHGlobal(accentStructSize);
				Marshal.StructureToPtr(accent, accentPtr, false);

				var data = new WindowCompositionAttributeData
				{
					Attribute = WCA.ACCENT_POLICY,
					Data = (void*)accentPtr,
					DataLength = accentStructSize
				};

				SetWindowCompositionAttribute(new HandleRef(null, window.Handle), in data);

				Marshal.FreeHGlobal(accentPtr);
			}
		}

		private static uint ToAbgr(Color color)
		{
			var a = color.A;
			var r = color.R;
			var g = color.G;
			var b = color.B;

			return (uint)((a << 24) | (b << 16) | (g << 8) | r);
		}

		// ReSharper disable InconsistentNaming, UnusedMember.Local, NotAccessedField.Local
#pragma warning disable 649

		// Discovered via:
		// https://withinrafael.com/2015/07/08/adding-the-aero-glass-blur-to-your-windows-10-apps/
		// https://github.com/riverar/sample-win32-acrylicblur/blob/917adc277c7258307799327d12262ebd47fd0308/MainWindow.xaml.cs

		[DllImport("user32.dll")]
		private static extern int SetWindowCompositionAttribute(HandleRef hWnd, in WindowCompositionAttributeData data);

		private unsafe struct WindowCompositionAttributeData
		{
			public WCA Attribute;
			public void* Data;
			public int DataLength;
		}

		private enum WCA
		{
			ACCENT_POLICY = 19
		}

		private enum ACCENT
		{
			DISABLED = 0,
			ENABLE_GRADIENT = 1,
			ENABLE_TRANSPARENTGRADIENT = 2,
			ENABLE_BLURBEHIND = 3,
			ENABLE_ACRYLICBLURBEHIND = 4,
			INVALID_STATE = 5
		}

		private struct AccentPolicy
		{
			public ACCENT AccentState;
			public uint AccentFlags;
			public uint GradientColor;
			public uint AnimationId;
		}
	}
}