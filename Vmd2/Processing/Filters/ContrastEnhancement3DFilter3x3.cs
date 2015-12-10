namespace Vmd2.Processing.Filters
{
    class ContrastEnhancement3DFilter3x3 : Filter3D
    {
        private static double[,,] array = new double[3, 3, 3] {
            {
                { -2, -2, -2 },
                { -2, 32, -2 },
                { -2, -2, -2 }
            },
            {
                { -2, -2, -2 },
                { -2, 32, -2 },
                { -2, -2, -2 }
            },
            {
                { -2, -2, -2 },
                { -2, 32, -2 },
                { -2, -2, -2 }
            }
        };

        public ContrastEnhancement3DFilter3x3() : base(array)
        {
        }
    }
}
