using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vmd2.Processing
{
    class ProcessEventArgs : EventArgs
    {
        public ProcessEventArgs(Image3D image)
        {
            this.Image = image;
        }

        public Image3D Image { get; private set; }
    }
}
