using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Vmd2.Processing.TransferFunctions
{
    class Histogram
    {

        public Histogram(double minimum, double maximum, int count)
        {
            this.Minimum = minimum;
            this.Maximum = maximum;
            this.histogramValues = new int[count];
        }

        public double Minimum { get; private set; }
        public double Maximum { get; private set; }
        public int Count { get { return histogramValues.Length; } }

        private int[] histogramValues;

        private int GetHistogramIndex(double value)
        {
            int i = (int)Math.Floor((value - Minimum) / (Maximum - Minimum) * Count);
            if (i < 0) { i = 0; }
            else if (i >= Count) { i = Count - 1; }

            return i;
        }

        public void FromImage(Image3D image)
        {
            if (image.LengthZ <= 25)
            {
                // full image
                for (int z = 0; z < image.LengthZ; z++)
                {
                    HistogramFromSlice(image, z);
                }
            }
            else
            {
                int delta = image.LengthZ / 25;
                // from 25 slices
                for (int z = 0; z < image.LengthZ; z += delta)
                {
                    HistogramFromSlice(image, z);
                }
            }

        }

        private void HistogramFromSlice(Image3D image, int slice)
        {
            for (int y = 0; y < image.LengthY; y++)
            {
                for (int x = 0; x < image.LengthX; x++)
                {
                    double voxel = image[x, y, slice];
                    histogramValues[GetHistogramIndex(voxel)]++;
                }
            }
        }

        public double this[int index]
        {
            get
            {
                return histogramValues[index];
            }
        }

        public void Render(DisplayImage display, TransferFunction1D tf)
        {
            // find max value
            int histogramMax = 0;
            int histogramMax2 = 0;
            int histogramSum = 0;
            for (int i = 0; i < histogramValues.Length; i++)
            {
                var v = histogramValues[i];
                if (v > histogramMax)
                {
                    histogramMax2 = histogramMax;
                    histogramMax = v;
                }

                if (v > histogramMax2 && v < histogramMax)
                {
                    histogramMax2 = v;
                }
                histogramSum += v;
            }

            int histogramHeight = histogramSum * 4 / Count; // TODO: find a suitable height function

            if (histogramHeight == 0) { histogramHeight = 1; } // avoid division by 0 error

            // draw histogram values
            int displayHeight = display.Height;
            int displayWidth = display.Width;

            double delta = (Maximum - Minimum) / Count;

            for (int x = 0; x < displayWidth; x++)
            {
                int histogramIndexStart = (x * Count) / (displayWidth);
                int histogramIndexEnd = ((x + 1) * Count) / (displayWidth);

                int sum = 0;
                for (int i = histogramIndexStart; i < histogramIndexEnd; i++)
                {
                    sum += histogramValues[i];
                }

                int barHeight = Math.Min(displayHeight, (sum * displayHeight) / histogramHeight);

                Color tfColor = tf.GetColor(Minimum + delta * x);
                for (int y = 0; y < 3; y++)
                {
                    display.SetPixel(x, y, tfColor);
                }
                for (int y = 3; y < (displayHeight - barHeight); y++)
                {
                    if (y >= display.Height) { break; }
                    display.SetPixel(x, y, Colors.Transparent);
                }
                for (int y = (displayHeight - barHeight); y < displayHeight; y++)
                {
                    if (y >= display.Height) { break; }
                    display.SetPixel(x, y, tfColor);
                }
            }
        }
    }
}
