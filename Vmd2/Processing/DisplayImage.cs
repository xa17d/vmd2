using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Vmd2.Processing
{
    class DisplayImage
    {
        public DisplayImage(int width, int height)
        {
            bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            CopyToBuffer();
        }

        private Bitmap bitmap;
        private byte[] buffer;
        private int stride;

        public int Width { get { return bitmap.Width; } }
        public int Height { get { return bitmap.Height; } }

        private void CopyToBuffer()
        {
            var bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, Width, Height),
                ImageLockMode.ReadOnly,
                bitmap.PixelFormat
            );

            this.stride = bitmapData.Stride;
            int bufferSize = bitmapData.Stride * bitmap.Height;
            if (buffer == null || buffer.Length != bufferSize)
            {
                buffer = new byte[bufferSize];
            }
            IntPtr scan0 = bitmapData.Scan0;

            Marshal.Copy(scan0, buffer, 0, bufferSize);

            bitmap.UnlockBits(bitmapData);
        }

        private void CopyToBitmap()
        {
            var bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, Width, Height),
                ImageLockMode.WriteOnly,
                bitmap.PixelFormat
            );

            IntPtr scan0 = bitmapData.Scan0;

            Marshal.Copy(buffer, 0, scan0, buffer.Length);

            bitmap.UnlockBits(bitmapData);

        }

        public void Update()
        {
            CopyToBitmap();
        }

        public void SetPixel(int x, int y, Color color)
        {
            int i = y * stride + x * 4;
            buffer[i + 0] = color.B;
            buffer[i + 1] = color.G;
            buffer[i + 2] = color.R;
            buffer[i + 3] = color.A;
        }

        public Bitmap GetBitmap()
        {
            return bitmap;
        }
    }
}
