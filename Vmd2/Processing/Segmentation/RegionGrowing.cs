﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Vmd2.Logging;
using Vmd2.Processing.Filters;

namespace Vmd2.Processing.Segmentation
{
    class RegionGrowing : Renderer
    {
        public RegionGrowing()
        {
            // default values
            markerX = 116;
            markerY = 146;
            markerZ = 6;

            deltaGlobal = 100;
            deltaLocal = 25;

            //TODO
            maxDelta = 200;
        }

        protected override Image3D OnProcess(Image3D imageIn, Progress progress)
        {
            OnValidate(imageIn);
            Size size = OnDetermineDisplaySize(imageIn);
            int w = (int)Math.Round(size.Width);
            int h = (int)Math.Round(size.Height);

            Image3D imageOut = imageIn.EmptyCopy();
            imageEnhancedContrast = imageIn.EmptyCopy();

            //TODO
            if(true)
            {
                filter = new ContrastEnhancement2DFilter3x3();
                Thread threadFilter = new Thread(new ThreadStart(ProcessFilter));
                threadFilter.Start();

                while (threadFilter.ThreadState == ThreadState.Running)
                {
                    Thread.Sleep(50);
                }
            }

            Region.Pixel seedPixel = new Region.Pixel(markerX, markerY, markerZ);
            Region region = new Region(imageIn, imageOut, imageEnhancedContrast, seedPixel, deltaGlobal, deltaLocal, progress);
            Thread threadGrow = new Thread(new ThreadStart(region.Grow));
            threadGrow.Start();

            while (threadGrow.ThreadState == ThreadState.Running)
            {
                Thread.Sleep(50);
            }

            return imageOut;
        }

        private Image3D imageEnhancedContrast;
        private ProcessingElement filter;

        private void ProcessFilter()
        {
            imageEnhancedContrast = filter.Process(imageEnhancedContrast);
        }

        private class Region
        {
            private double deltaGlobal;
            private double deltaLocal;
            private Image3D imageIn;
            private Image3D imageOut;
            private Image3D imageEnhancedContrast;
            private Progress progress;

            private ISet<Pixel> pixelToCompute = new HashSet<Pixel>();
            private ISet<Pixel> computedPixel = new HashSet<Pixel>();

            public Region(Image3D imageIn, Image3D imageOut, Image3D imageEnhancedContrast, Pixel seedPixel, double deltaGlobal, double deltaLocal, Progress progress)
            {
                this.imageIn = imageIn;
                this.imageOut = imageOut;
                this.imageEnhancedContrast = imageEnhancedContrast;
                this.deltaGlobal = deltaGlobal;
                this.deltaLocal = deltaLocal;
                this.progress = progress;

                pixelToCompute.Add(seedPixel);
            }

            public void Grow()
            {
                Pixel firstPixel = pixelToCompute.First();
                var min = imageIn[firstPixel.X, firstPixel.Y, firstPixel.Z] - deltaGlobal / 2;
                var max = imageIn[firstPixel.X, firstPixel.Y, firstPixel.Z] + deltaGlobal / 2;

                double increment = 1.0 / (imageIn.LengthX * imageIn.LengthY * imageIn.LengthZ);

                while (pixelToCompute.Any())
                {
                    Pixel pixel = pixelToCompute.First();

                    imageOut[pixel.X, pixel.Y, pixel.Z] = imageIn[pixel.X, pixel.Y, pixel.Z];
                    for (int i = pixel.Z - 1; i <= pixel.Z + 1; i++)
                    {
                        for (int j = pixel.Y - 1; j <= pixel.Y + 1; j++)
                        {
                            for (int k = pixel.X - 1; k <= pixel.X + 1; k++)
                            {
                                Pixel newPixel = new Pixel(k, j, i);
                                if (!computedPixel.Contains(newPixel) && newPixel.IsExistingPixel(imageIn))
                                {
                                    if (imageIn[k, j, i] >= min 
                                        &&
                                        imageIn[k, j, i] <= max 
                                        && 
                                        Math.Abs(imageEnhancedContrast[pixel.X, pixel.Y, pixel.Z] - imageEnhancedContrast[k, j, i]) <= deltaLocal)
                                    {
                                        pixelToCompute.Add(newPixel);
                                    }
                                }
                            }
                        }
                    }

                    pixelToCompute.Remove(pixel);
                    computedPixel.Add(pixel);

                    progress.UpdateIncrement(increment);
                }
            }

            public class Pixel
            {
                public int X { get; set; }
                public int Y { get; set; }
                public int Z { get; set; }

                public Pixel(int x, int y, int z)
                {
                    X = x;
                    Y = y;
                    Z = z;
                }

                public bool IsExistingPixel(Image3D image)
                {
                    if(X < 0 || X >= image.LengthX || Y < 0 || Y >= image.LengthY || Z < 0 || Z >= image.LengthZ)
                    {
                        return false;
                    }

                    return true;
                }

                public override int GetHashCode()
                {
                    return Z + Y * 10000 + X * 100000000;
                }

                public override bool Equals(object obj)
                {
                    if(obj == null)
                    {
                        return false;
                    }

                    Pixel p = obj as Pixel;
                    if ((System.Object)p == null)
                    {
                        return false;
                    }

                    /*
                    // inefficiency ???
                    if(this.GetHashCode() != p.GetHashCode())
                    {
                        return false;
                    }
                    */

                    return (X == p.X) && (Y == p.Y) && (Z == p.Z);
                }
            }
        }

        protected override void OnRenderPixel(Image3D image, DisplayImage display, int x, int y)
        {
            throw new NotImplementedException();
        }

        private int markerX;
        public int MarkerX
        {
            get { return markerX; }
            set { if (value != markerX) { markerX = value; OnPropertyChanged(); } }
        }

        private int markerY;
        public int MarkerY
        {
            get { return markerY; }
            set { if (value != markerY) { markerY = value; OnPropertyChanged(); } }
        }

        private int markerZ;
        public int MarkerZ
        {
            get { return markerZ; }
            set { if (value != markerZ) { markerZ = value; OnPropertyChanged(); } }
        }

        private double deltaGlobal;
        public double DeltaGlobal
        {
            get { return deltaGlobal; }
            set { deltaGlobal = value; OnPropertyChanged(); }
        }

        private double deltaLocal;
        public double DeltaLocal
        {
            get { return deltaLocal; }
            set { deltaLocal = value; OnPropertyChanged(); }
        }

        private double maxDelta;
        public double MaxDelta
        {
            get { return maxDelta; }
        }
    }
}
