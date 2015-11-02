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
        }

        public int Slice { get; set; }
        public Windowing Window { get; }
        private Image3D image;
        private DisplayImage display;

        protected override void OnRenderPixel(int x, int y)
        {
            var voxel = image[x, y, Slice];
            display.SetPixel(x, y, Window.GetColor(voxel));
        }

        public override string ToString()
        {
            return GetType().Name + " [slice: " + Slice + "]";
        }
    }
}
