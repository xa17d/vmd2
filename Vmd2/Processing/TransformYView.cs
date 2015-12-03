using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vmd2.Logging;

namespace Vmd2.Processing
{
    class TransformYView : ProcessingElement3D
    {
        private bool activated;
        public bool Activated
        {
            get { return activated; }
            set { if (value != activated) { activated = value; OnPropertyChanged(); } }
        }

        protected override Image3D GetOutputImage(Image3D imageIn)
        {
            var imageOut = new Image3D(imageIn.LengthY, imageIn.LengthZ, imageIn.LengthX);
            imageOut.Minimum = imageIn.Minimum;
            imageOut.Maximum = imageIn.Maximum;
            return imageOut;
        }

        protected override Image3D OnProcess(Image3D image, Progress progress)
        {
            if (Activated)
            {
                return base.OnProcess(image, progress);
            }
            else
            {
                progress.Done();
                return image;
            }
        }

        protected override void OnProcess3D(Image3D imageIn, Image3D imageOut, int x, int y, int z)
        {
            imageOut[x, z, y] = imageIn[x, y, z];
        }
    }
}
