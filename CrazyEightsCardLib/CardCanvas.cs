using System;
using System.Drawing;
namespace CrazyEightsCardLib
{
	public class CardCanvas : IDisposable
	{
		public const int DefaultWidth = 71;
		public const int DefaultHeight = 95;
		internal const int BorderMask = 16777215;
		private Graphics graphicsSurface;
		private IntPtr graphicsDC;
		private bool lastReturnValue;
		private bool disposed;
		private static int mode;
		public int Mode
		{
			get
			{
				return CardCanvas.mode;
			}
			set
			{
				CardCanvas.mode = value;
			}
		}
		public bool LastReturnValue
		{
			get
			{
				return this.lastReturnValue;
			}
		}
		public CardCanvas()
		{
			int num = 71;
			int num2 = 95;
			CardCanvas.mode = 0;
			NativeMethods.cdtInit(ref num, ref num2);
		}
		~CardCanvas()
		{
			this.Dispose(false);
		}
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		private void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (this.graphicsSurface != null && this.graphicsDC != IntPtr.Zero)
				{
					this.graphicsSurface.ReleaseHdc(this.graphicsDC);
					this.graphicsDC = IntPtr.Zero;
				}
				NativeMethods.cdtTerm();
				if (disposing && this.graphicsSurface != null)
				{
					this.graphicsSurface.Dispose();
				}
			}
			this.disposed = true;
		}
		private void ReleaseDC()
		{
			if (this.HasDC())
			{
				this.graphicsSurface.ReleaseHdc(this.graphicsDC);
				this.graphicsDC = IntPtr.Zero;
			}
		}
		private void EnsureDC()
		{
			if (!this.HasDC())
			{
				this.graphicsDC = this.graphicsSurface.GetHdc();
			}
		}
		private bool HasDC()
		{
			return this.graphicsDC != IntPtr.Zero;
		}
		public void BeginPaint(Graphics graphicsSurface)
		{
			this.graphicsSurface = graphicsSurface;
			this.graphicsDC = IntPtr.Zero;
		}
		public void EndPaint()
		{
			this.ReleaseDC();
			this.graphicsSurface = null;
		}
		public void DrawCard(Point topLeft, int cardIndex)
		{
			this.EnsureDC();
			this.lastReturnValue = NativeMethods.cdtDraw(this.graphicsDC, topLeft.X, topLeft.Y, cardIndex, CardCanvas.mode, 16777215);
		}
		public void DrawCardBack(Point topLeft, CardBack cardBack)
		{
			this.EnsureDC();
			this.lastReturnValue = NativeMethods.cdtDraw(this.graphicsDC, topLeft.X, topLeft.Y, (int)cardBack, 1, 16777215);
		}
		public void DrawCardBack(Point topLeft, CardBack cardBack, int frameNo)
		{
			this.EnsureDC();
			this.lastReturnValue = NativeMethods.cdtAnimate(this.graphicsDC, (int)cardBack, topLeft.X, topLeft.Y, frameNo);
		}
		public void DrawHighlightedCard(Point topLeft, int cardIndex)
		{
			this.EnsureDC();
			this.lastReturnValue = NativeMethods.cdtDraw(this.graphicsDC, topLeft.X, topLeft.Y, cardIndex, 2, 16777215);
		}
		public void DrawEmptyCard(Point topLeft, Color color)
		{
			this.EnsureDC();
			this.lastReturnValue = NativeMethods.cdtDraw(this.graphicsDC, topLeft.X, topLeft.Y, 1, 3, color.ToArgb());
		}
		public void DrawExtrudedCard(Point topLeft, Point bottomRight, int cardIndex)
		{
			this.EnsureDC();
			Size size = new Size(bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y);
			this.lastReturnValue = NativeMethods.cdtDrawExt(this.graphicsDC, topLeft.X, topLeft.Y, size.Width, size.Height, cardIndex, CardCanvas.mode, 16777215);
		}
		private void InternalDrawCard(Bitmap offScreenBitmap, Point topLeft, int cardIndex)
		{
			using (Graphics graphics = Graphics.FromImage(offScreenBitmap))
			{
				IntPtr intPtr = IntPtr.Zero;
				try
				{
					intPtr = graphics.GetHdc();
					this.lastReturnValue = NativeMethods.cdtDraw(intPtr, topLeft.X, topLeft.Y, cardIndex, CardCanvas.mode, 16777215);
				}
				finally
				{
					if (intPtr != IntPtr.Zero)
					{
						graphics.ReleaseHdc(intPtr);
					}
				}
			}
		}
		public void DrawRotatedCard(Point upperLeft, Point upperRight, Point lowerLeft, int cardIndex)
		{
			using (Bitmap bitmap = new Bitmap(71, 95))
			{
				this.InternalDrawCard(bitmap, new Point(0, 0), cardIndex);
				this.ReleaseDC();
				Point[] destPoints = new Point[]
				{
					upperLeft, 
					upperRight, 
					lowerLeft
				};
				this.graphicsSurface.DrawImage(bitmap, destPoints);
			}
		}
		public void DrawRotatedCard(Point upperLeft, int angle, int cardIndex)
		{
			using (Bitmap bitmap = new Bitmap(71, 95))
			{
				this.InternalDrawCard(bitmap, new Point(0, 0), cardIndex);
				this.ReleaseDC();
				double num = (double)angle / 180.0 * 3.1415926535897931;
				Point point = new Point(upperLeft.X + (int)(71.0 * Math.Cos(num)), upperLeft.Y + (int)(-71.0 * Math.Sin(num)));
				Point point2 = new Point(upperLeft.X + (int)(95.0 * Math.Sin(num)), upperLeft.Y + (int)(95.0 * Math.Cos(num)));
				Point[] destPoints = new Point[]
				{
					upperLeft, 
					point, 
					point2
				};
				this.graphicsSurface.DrawImage(bitmap, destPoints);
			}
		}
	}
}
