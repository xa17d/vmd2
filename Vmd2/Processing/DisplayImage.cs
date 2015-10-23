using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Vmd2.Processing
{
    class DisplayImage
    {
        public DisplayImage(int width, int height)
        {
            bitmap = new WriteableBitmap(width, height, 96,96, PixelFormats.Bgra32, BitmapPalettes.BlackAndWhite);
            stride = bitmap.PixelWidth * (bitmap.Format.BitsPerPixel / 8);
            CopyToBuffer();
        }

        private WriteableBitmap bitmap;
        private byte[] buffer;
        private int stride;

        public int Width { get { return bitmap.PixelWidth; } }
        public int Height { get { return bitmap.PixelHeight; } }
        
        private void CopyToBuffer()
        {
            bitmap.Lock();
            
            int bufferSize = stride * Height;
            if (buffer == null || buffer.Length != bufferSize)
            {
                buffer = new byte[bufferSize];
            }

            /*
            bitmap.CopyPixels(
                new System.Windows.Int32Rect(0, 0, Width, Height),
                buffer,
                stride,
                0
            );
            */

            bitmap.Unlock();
        }

        private void CopyToBitmap()
        {
            bitmap.Lock();

            int stride = bitmap.PixelWidth * (bitmap.Format.BitsPerPixel / 8);

            bitmap.WritePixels(
                new System.Windows.Int32Rect(0, 0, Width, Height),
                buffer,
                stride,
                0
            );
            bitmap.Unlock();
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

        public BitmapSource GetBitmap()
        {
            return bitmap;
        }
    }
}
