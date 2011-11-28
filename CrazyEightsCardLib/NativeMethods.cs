using System;
using System.Runtime.InteropServices;
namespace CrazyEightsCardLib
{
	static class NativeMethods
	{
		[DllImport("Cards.dll")]
		public static extern bool cdtInit(ref int width, ref int height);

        [DllImport("Cards.dll")]
		public static extern void cdtTerm();

        [DllImport("Cards.dll")]
		public static extern bool cdtDraw(IntPtr hdc, int x, int y, int card, int mode, int color);

        [DllImport("Cards.dll")]
		public static extern bool cdtDrawExt(IntPtr hdc, int x, int y, int dx, int dy, int card, int type, int color);

        [DllImport("Cards.dll")]
		public static extern bool cdtAnimate(IntPtr hdc, int cardback, int x, int y, int frame);
	}
}
