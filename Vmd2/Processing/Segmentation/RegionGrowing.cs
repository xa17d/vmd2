using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Vmd2.Logging;

namespace Vmd2.Processing.Segmentation
{
    class RegionGrowing : Renderer
    {
        private int[] tmpSeedPixel;
        private double tmpDelta;
        public RegionGrowing()
        {
            tmpSeedPixel = new int[3] { 90, 200, 0 };
            tmpDelta = 3000;
        }

        private DisplayImage displayImage;

        protected override Image3D OnProcess(Image3D imageIn, Progress progress)
        {
            OnValidate(imageIn);
            Size size = OnDetermineDisplaySize(imageIn);
            int w = (int)Math.Round(size.Width);
            int h = (int)Math.Round(size.Height);

            displayImage = Display.GetDisplay(w, h);

            Image3D imageOut;
            imageOut = imageIn.EmptyCopy();

            progress.Update(0.01);

            bool[,,] pixelComputed = new bool[imageIn.LengthX, imageIn.LengthY, imageIn.LengthZ];

            RegionObject region = new RegionObject(imageIn, imageOut, tmpSeedPixel, pixelComputed);

            var stackSize = 10000000;
            Thread thread = new Thread(new ParameterizedThreadStart(GrowRegion), stackSize);
            thread.Start(region);

            progress.Done();

            return imageOut;
        }

        private void GrowRegion(object o)
        {
            RegionObject region = (RegionObject)o;
            GrowRegion(region.ImageIn, region.ImageOut, region.SeedPixel, region.PixelComputed);
        }

        private void GrowRegion(Image3D imageIn, Image3D imageOut, int[] seedPixel, bool[,,] pixelComputed)
        {
            int x = seedPixel[0];
            int y = seedPixel[1];
            int z = seedPixel[2];

            imageOut[x, y, z] = imageIn[x, y, z];
            pixelComputed[x, y, z] = true;

            int i = z;
            //for (int i = z - 1; i <= z + 1; i++)
            //{
                for (int j = y - 1; j <= y + 1; j++)
                {
                    for (int k = x - 1; k <= x + 1; k++)
                    {
                        if (i >= 0 && i < imageIn.LengthZ && j >= 0 && j < imageIn.LengthY && k >= 0 && k < imageIn.LengthX)
                        {
                            if (Math.Abs(imageIn[x, y, z] - imageIn[k, j, i]) <= tmpDelta && !pixelComputed[k, j, i])
                            {
                                GrowRegion(imageIn, imageOut, new int[3] { k, j, i }, pixelComputed);
                            }
                        }
                    }
                }
            //}
        }

        private class RegionObject
        {
            public Image3D ImageIn { get; private set; }
            public Image3D ImageOut { get; private set; }
            public int[] SeedPixel { get; private set; }
            public bool[,,] PixelComputed { get; private set; }

            public RegionObject(Image3D imageIn, Image3D imageOut, int[] seedPixel, bool[,,] pixelComputed)
            {
                this.ImageIn = imageIn;
                this.ImageOut = imageOut;
                this.SeedPixel = seedPixel;
                this.PixelComputed = pixelComputed;
            }
        }

        protected override void OnRenderPixel(Image3D image, DisplayImage display, int x, int y)
        {
            throw new NotImplementedException();
        }
    }
}
