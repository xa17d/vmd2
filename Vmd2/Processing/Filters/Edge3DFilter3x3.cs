namespace Vmd2.Processing.Filters
{
    class EdgeDFilter3x3 : Filter2D
    {
        //TODO
        private static double[,] array = new double[3, 3] { 
            { 1,  1, 1 }, 
            { 1, -8, 1 }, 
            { 1,  1, 1 }
        };

        public EdgeDFilter3x3() : base(array)
        {
        }
    }
}
