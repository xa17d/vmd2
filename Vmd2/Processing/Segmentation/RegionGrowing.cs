﻿using System;
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

        protected override Image3D OnProcess(Image3D imageIn, Progress progress)
        {
            OnValidate(imageIn);
            Size size = OnDetermineDisplaySize(imageIn);
            int w = (int)Math.Round(size.Width);
            int h = (int)Math.Round(size.Height);

            Image3D imageOut;
            imageOut = imageIn.EmptyCopy();

            Region.Pixel seedPixel = new Region.Pixel(markerX, markerY, markerZ);
            Region region = new Region(imageIn, imageOut, seedPixel, deltaGlobal, deltaLocal, progress);
            Thread thread = new Thread(new ThreadStart(region.Grow), 10000);
            thread.Start();

            while (thread.ThreadState == ThreadState.Running)
            {
                Thread.Sleep(10);
            }

            return imageOut;
        }

        private class Region
        {
            private double deltaGlobal;
            private double deltaLocal;
            private Image3D imageIn;
            private Image3D imageOut;
            private Progress progress;

            private ISet<Pixel> pixelToCompute = new HashSet<Pixel>();
            private ISet<Pixel> computedPixel = new HashSet<Pixel>();

            public Region(Image3D imageIn, Image3D imageOut, Pixel seedPixel, double deltaGlobal, double deltaLocal, Progress progress)
            {
                this.imageIn = imageIn;
                this.imageOut = imageOut;
                this.deltaGlobal = deltaGlobal;
                this.deltaLocal = deltaLocal;
                this.progress = progress;

                pixelToCompute.Add(seedPixel);
            }

            public void Grow()
            {
                Pixel firstPixel = pixelToCompute.First();
                var min = imageIn[firstPixel.X, firstPixel.Y, firstPixel.Z] - deltaGlobal;
                var max = imageIn[firstPixel.X, firstPixel.Y, firstPixel.Z] + deltaGlobal;
                pixelToCompute.Add(firstPixel);

                double totalPixel = imageIn.LengthX * imageIn.LengthY * imageIn.LengthZ;

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
                                    if (imageIn[k, j, i] >= min && imageIn[k, j, i] <= max && Math.Abs(imageIn[pixel.X, pixel.Y, pixel.Z] - imageIn[k, j, i]) <= deltaLocal)
                                    {
                                        pixelToCompute.Add(newPixel);
                                    }
                                }
                            }
                        }
                    }

                    pixelToCompute.Remove(pixel);
                    computedPixel.Add(pixel);
                    progress.Update(computedPixel.Count / totalPixel);
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

                    return (X == p.X) && (Y == p.Y) && (Z == p.Z);
                }
            }
        }

        protected override void OnRenderPixel(Image3D image, DisplayImage display, int x, int y)
        {
            throw new NotImplementedException();
        }
    }
}
