using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Vmd2.Processing.TransferFunctions;

namespace Vmd2.Presentation.ViewModels
{
    class RenderSlice : RenderVm
    {
        private int slice;
        public int Slice
        {
            get { return slice; }
            set
            {
                if (value != slice)
                {
                    slice = value;

                    ((TransferFunction1DRenderer)Renderer).Slice = value;

                    OnPropertyChanged();
                    Render();
                }
            }
        }
    }
}
