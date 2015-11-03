using Vmd2.Processing.Filter;

namespace Vmd2.Processing.Mapping
{
    class WindowingRenderer : RendererPixel
    {
        public WindowingRenderer(Image3D image, DisplayImage display, Windowing window) : base(display)
        {
            this.image = image;
            this.display = display;
            this.Window = window;
            this.Slice = 0;

            // 3 x 3 Filter
            //this.filter = new GaussianFilter(new double[3, 3] { { 1, 2, 1 }, { 2, 4, 2 }, { 1, 2, 1 } });

            // 7 x 7 Filter
            this.filter = new GaussianFilter(new double[7, 7] { { 1, 2, 3, 4, 3, 2, 1 }, { 2, 4, 6, 8, 6, 4, 2 }, { 3, 6, 9, 12, 9, 6, 3 }, { 4, 8, 12, 16, 12, 8, 4 }, { 3, 6, 9, 12, 9, 6, 3 }, { 2, 4, 6, 8, 6, 4, 2 }, { 1, 2, 3, 4, 3, 2, 1 } });
        }

        public int Slice { get; set; }
        public Windowing Window { get; }
        private Image3D image;
        private DisplayImage display;
        private GaussianFilter filter;

        protected override void OnRenderPixel(int x, int y)
        {
            //var voxel = image[x, y, Slice];
            var voxel = filter.FilterPixel(x, y, Slice, image);
            display.SetPixel(x, y, Window.GetColor(voxel));
        }

        public override string ToString()
        {
            return GetType().Name + " [slice: " + Slice + "]";
        }
    }
}
