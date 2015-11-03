﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Vmd2.Logging;

namespace Vmd2.Processing
{
    abstract class Renderer : ProcessingElement2D
    {
        public Renderer() : base(8, true)
        {
        }

        private IDisplay display;
        public IDisplay Display
        {
            get { return display; }
            set { if (value != display) { display = value; OnPropertyChanged(); } }
        }

        private DisplayImage displayImage;

        protected override Image3D OnProcess(Image3D image, Progress progress)
        {
            OnValidate(image);
            Size size = OnDetermineDisplaySize(image);
            int w = (int)Math.Round(size.Width);
            int h = (int)Math.Round(size.Height);

            displayImage = Display.GetDisplay(w, h);
            var result = base.OnProcess(image, progress);
            return result;
        }

        protected override void OnProcess2D(Image3D imageIn, Image3D imageOut, int x, int y)
        {
            OnRenderPixel(imageIn, displayImage, x, y);
        }

        protected virtual void OnValidate(Image3D image) { }

        protected virtual Size OnDetermineDisplaySize(Image3D image)
        {
            return new Size(image.LengthX, image.LengthY);
        }

        protected abstract void OnRenderPixel(Image3D image, DisplayImage display, int x, int y);
    }
}
