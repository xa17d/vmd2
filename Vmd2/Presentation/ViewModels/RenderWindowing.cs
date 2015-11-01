using Vmd2.Processing.Mapping;

namespace Vmd2.Presentation.ViewModels
{
    class RenderWindowing : RenderVm
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
                    ((WindowingRenderer)Renderer).Slice = value;

                    OnPropertyChanged();
                    Render();
                }
            }
        }
    }
}
