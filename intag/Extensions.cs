using System;

namespace intag
{
	public static class Extensions
	{
		public static System.Drawing.Color WithAlpha(this System.Drawing.Color color, byte alpha)
		{
			return System.Drawing.Color.FromArgb(alpha, color);
		}

		public static System.Drawing.Color WithBrightness(this System.Drawing.Color color, float brightness)
		{
			var clamp = Math.Clamp(brightness, 0, 1);
			return System.Drawing.Color.FromArgb(color.A, (int)(color.R * clamp), (int)(color.G * clamp), (int)(color.B * clamp));
		}
	}
}