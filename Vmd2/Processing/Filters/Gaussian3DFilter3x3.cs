namespace Vmd2.Processing.Filters
{
    class Gaussian3DFilter3x3 : Filter3D
    {
        private static double[,,] array = new double[3, 3, 3] {
            {
                { 1, 2, 1 },
                { 2, 4, 2 },
                { 1, 2, 1 }
            },
            {
                { 1, 2, 1 },
                { 2, 4, 2 },
                { 1, 2, 1 }
            },
            {
                { 1, 2, 1 },
                { 2, 4, 2 },
                { 1, 2, 1 }
            }
        };

        public Gaussian3DFilter3x3() : base(array)
        {
        }
    }
}
