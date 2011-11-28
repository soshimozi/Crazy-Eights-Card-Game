using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Fireworks
{
    unsafe class BitmapHelper
    {
        public void SetPixel(BitmapData bitmapData, int x, int y, int color)
        {
            switch (bitmapData.PixelFormat)
            {
                case PixelFormat.Format16bppArgb1555:
                case PixelFormat.Format16bppGrayScale:
                case PixelFormat.Format16bppRgb555:
                case PixelFormat.Format16bppRgb565:
                    setPixelNonIndexed(bitmapData, x, y, color, 16);
                    break;

                case PixelFormat.Format24bppRgb:
                    setPixelNonIndexed(bitmapData, x, y, color, 24);
                    break;

                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                case PixelFormat.Format32bppRgb:
                    setPixelNonIndexed(bitmapData, x, y, color, 32);
                    break;

                case PixelFormat.Format48bppRgb:
                    setPixelNonIndexed(bitmapData, x, y, color, 48);
                    break;

                case PixelFormat.Format64bppArgb:
                case PixelFormat.Format64bppPArgb:
                    setPixelNonIndexed(bitmapData, x, y, color, 64);
                    break;

                case PixelFormat.Format1bppIndexed:
                    setPixelIndexed1Bit(bitmapData, x, y, (color != 0));
                    break;

                case PixelFormat.Format4bppIndexed:
                    setPixeIndexed4Bit(bitmapData, x, y, (byte)color);
                    break;
            }
        }

        private void setPixelNonIndexed(BitmapData data, int x, int y, int color, int pixelSize)
        {
            int* row = (int*)data.Scan0 + (y * data.Stride);
            row[x * pixelSize] = color;
        }

        private void setPixeIndexed4Bit(BitmapData data, int x, int y, byte color)
        {
            int offset = (y * data.Stride) + (x >> 1);
            byte currentByte = ((byte*)data.Scan0)[offset];
            if ((x & 1) == 1)
            {
                currentByte &= 0xF0;
                currentByte |= (byte)(color & 0x0F);
            }

            else
            {
                currentByte &= 0x0F;
                currentByte |= (byte)(color << 4);
            }

            ((byte*)data.Scan0)[offset] = currentByte;
        }

        private void setPixelIndexed1Bit(BitmapData data, int x, int y, bool setPixel)
        {
            byte* p = (byte*)data.Scan0.ToPointer();
            int index = y * data.Stride + (x >> 3);
            byte mask = (byte)(0x80 >> (x & 0x7));
            if (setPixel)
                p[index] |= mask;
            else
                p[index] &= (byte)(mask ^ 0xff); 
        }
    }
}
