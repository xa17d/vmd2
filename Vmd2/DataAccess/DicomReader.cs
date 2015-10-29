using Kitware.VTK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vmd2.Logging;
using Vmd2.Processing;

namespace Vmd2.DataAccess
{
    class DicomReader : IDisposable
    {
        public DicomReader(string directoryPath)
        {
            this.MinValue = double.NaN;
            this.MaxValue = double.NaN;

            reader = vtkDICOMImageReader.New();
            reader.SetDirectoryName(directoryPath);
            reader.Update();
        }

        public void Dispose()
        {
            reader.Dispose();
        }

        private vtkDICOMImageReader reader;

        public double MinValue { get; private set; }
        public double MaxValue { get; private set; }

        public Image3D ReadImage3D()
        {
            // based on http://www.vtk.org/Wiki/VTK/Examples/Cxx/ImageData/IterateImageData

            // conversion value to HU based on http://www.codeproject.com/Articles/466955/Medical-image-visualization-using-WPF

            var imageData = reader.GetOutput();
            var rescaleSlope = reader.GetRescaleSlope();
            var rescaleOffset = reader.GetRescaleOffset();
            int[] dimensions = imageData.GetDimensions();

            if (dimensions.Length != 3)
            {
                throw new Exception("Image must have 3 dimensions!");
            }

            var image = new Image3D(dimensions[2], dimensions[1], dimensions[0]);

            double min = double.MaxValue;
            double max = double.MinValue;

            using (var progress = Log.P("Read in image..."))
            {
                for (int z = 0; z < dimensions[2]; z++)
                {
                    for (int y = 0; y < dimensions[1]; y++)
                    {
                        for (int x = 0; x < dimensions[0]; x++)
                        {
                            double voxel = imageData.GetScalarComponentAsDouble(x, y, z, 0) * rescaleSlope + rescaleOffset;
                            image[x, y, z] = voxel;

                            min = Math.Min(min, voxel);
                            max = Math.Max(max, voxel);
                        }
                    }

                    progress.Update((z / (double)image.LengthZ));
                }
            }

            Log.I("Image loaded. min: " + min + "; max: " + max);

            this.MinValue = min;
            this.MaxValue = max;

            return image;
        }
    }
}
