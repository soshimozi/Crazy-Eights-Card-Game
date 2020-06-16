using System;
using System.Drawing;
using static CrazyEightsCardLib.NativeMethods;

namespace CrazyEightsCardLib
{
	public class CardCanvas : IDisposable
	{
		public const int DefaultWidth = 71;
		public const int DefaultHeight = 95;
        private Graphics _graphicsSurface;
		private IntPtr _graphicsDc;
        private bool _disposed;
		private static int _mode;

        public CardCanvas()
		{
			int num = 71;
			int num2 = 95;
			_mode = 0;
			cdtInit(ref num, ref num2);
		}
		~CardCanvas()
		{
			Dispose(false);
		}
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		private void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (_graphicsSurface != null && _graphicsDc != IntPtr.Zero)
				{
					_graphicsSurface.ReleaseHdc(_graphicsDc);
					_graphicsDc = IntPtr.Zero;
				}
				cdtTerm();
				if (disposing)
				{
					_graphicsSurface?.Dispose();
				}
			}
			_disposed = true;
		}
		private void ReleaseDc()
        {
            if (!HasDc()) return;
            _graphicsSurface.ReleaseHdc(_graphicsDc);
            _graphicsDc = IntPtr.Zero;
        }
		private void EnsureDc()
		{
			if (!HasDc())
			{
				_graphicsDc = _graphicsSurface.GetHdc();
			}
		}
		private bool HasDc()
		{
			return _graphicsDc != IntPtr.Zero;
		}
		public void BeginPaint(Graphics graphicsSurface)
		{
			_graphicsSurface = graphicsSurface;
			_graphicsDc = IntPtr.Zero;
		}
		public void EndPaint()
		{
			ReleaseDc();
			_graphicsSurface = null;
		}
		public void DrawCard(Point topLeft, int cardIndex)
		{
			EnsureDc();
			cdtDraw(_graphicsDc, topLeft.X, topLeft.Y, cardIndex, _mode, 16777215);
		}
		public void DrawCardBack(Point topLeft, CardBack cardBack)
		{
			EnsureDc();
			cdtDraw(_graphicsDc, topLeft.X, topLeft.Y, (int)cardBack, 1, 16777215);
		}

        private void InternalDrawCard(Bitmap offScreenBitmap, Point topLeft, int cardIndex)
		{
			using (var graphics = Graphics.FromImage(offScreenBitmap))
			{
				var intPtr = IntPtr.Zero;
				try
				{
					intPtr = graphics.GetHdc();
					cdtDraw(intPtr, topLeft.X, topLeft.Y, cardIndex, _mode, 16777215);
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
    }
}
