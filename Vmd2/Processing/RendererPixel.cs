using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vmd2.Logging;

namespace Vmd2.Processing
{
    abstract class RendererPixel : Renderer
    {
        public RendererPixel(DisplayImage display) : base(display)
        {
            ThreadCount = 1;
        }

        private Thread[] threads;

        public int ThreadCount
        {
            get
            {
                return threads.Length;
            }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException("ThreadCount", "Value must be greater than 0");
                }

                threads = new Thread[value];
            }
        }

        protected override void OnRender(Progress progress)
        {
            int delta = Display.Height / threads.Length;
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(new ParameterizedThreadStart(RenderLines));

                int min = delta * i;
                int max = (i == threads.Length - 1 ? Display.Height : (delta * (i + 1)));
                var range = new LineRange(progress, min, max);

                threads[i].Start(range);
            }

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Join();
            }
        }

        private void RenderLines(object lineObj)
        {
            var line = (LineRange)lineObj;
            var progress = line.Progress;

            int min = line.Min;
            int max = line.Max;
            int width = Display.Width;

            double delta = 1.0 / (((double)max - min) * ThreadCount);

            for (int y = min; y < max; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    OnRenderPixel(x, y);
                }

                progress.UpdateIncrement(delta);
            }
        }

        protected abstract void OnRenderPixel(int x, int y);

        private class LineRange
        {
            public LineRange(Progress progress, int min, int max)
            {
                this.Progress = progress;
                this.Min = min;
                this.Max = max;
            }

            public int Min;
            public int Max;
            public Progress Progress;
        }
    }
}
