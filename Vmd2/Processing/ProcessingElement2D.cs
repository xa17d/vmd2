﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vmd2.Logging;

namespace Vmd2.Processing
{
    abstract class ProcessingElement2D : ProcessingElement
    {
        public ProcessingElement2D(int threadCount, bool useInputImageAsOutputImage) : base()
        {
            threads = new Thread[threadCount];
            this.ThreadCount = threadCount;
            this.useInputImageAsOutputImage = useInputImageAsOutputImage;
        }

        public ProcessingElement2D() : this(1, false) { }

        private Thread[] threads;
        private bool useInputImageAsOutputImage;

        public int ThreadCount
        {
            get
            {
                return threads.Length;
            }
            set
            {
                if (value != threads.Length)
                {
                    if (value < 1)
                    {
                        throw new ArgumentOutOfRangeException("ThreadCount", "Value must be greater than 0");
                    }

                    threads = new Thread[value];
                    OnPropertyChanged();
                }
            }
        }

        private void ProcessLines(object lineObj)
        {
            var line = (LineRange)lineObj;
            var progress = line.Progress;
            var imageIn = line.ImageIn;
            var imageOut = line.ImageOut;

            int min = line.Min;
            int max = line.Max;
            int width = imageIn.LengthX;

            double delta = 1.0 / (((double)max - min) * ThreadCount);

            for (int y = min; y < max; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    OnProcess2D(imageIn, imageOut, x, y);
                }

                progress.UpdateIncrement(delta);
            }
        }

        private class LineRange
        {
            public LineRange(Image3D imageIn, Image3D imageOut, Progress progress, int min, int max)
            {
                this.ImageIn = imageIn;
                this.ImageOut = imageOut;
                this.Progress = progress;
                this.Min = min;
                this.Max = max;
            }

            public Image3D ImageIn;
            public Image3D ImageOut;
            public Progress Progress;
            public int Min;
            public int Max;
        }

        protected override Image3D OnProcess(Image3D image, Progress progress)
        {
            Image3D imageOut;
            if (useInputImageAsOutputImage)
            { imageOut = image; }
            else
            { imageOut = new Image3D(image.LengthZ, image.LengthY, image.LengthX); }

            int delta = image.LengthY / threads.Length;

            LineRange firstRange = null;

            for (int i = 0; i < threads.Length; i++)
            {
                int min = delta * i;
                int max = (i == threads.Length - 1 ? image.LengthY : (delta * (i + 1)));
                var range = new LineRange(image, imageOut, progress, min, max);

                if (i == 0)
                {
                    // run first range in this thread
                    // so save the range for later
                    firstRange = range;
                }
                else
                {
                    threads[i] = new Thread(new ParameterizedThreadStart(ProcessLines));
                    threads[i].Start(range);
                }
            }

            // run first range
            ProcessLines(firstRange);

            // join all other threads
            // Attention: start at 1, beacause 0 is null because it was processed in this Thread
            for (int i = 1; i < threads.Length; i++)
            {
                while (threads[i].ThreadState == ThreadState.Unstarted)
                {
                    Thread.Sleep(0);
                }

                threads[i].Join();
            }

            return imageOut;
        }

        protected abstract void OnProcess2D(Image3D imageIn, Image3D imageOut, int x, int y);
    }
}
