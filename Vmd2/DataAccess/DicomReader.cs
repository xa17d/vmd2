using Kitware.VTK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vmd2.Logging;
using Vmd2.Processing;

namespace Vmd2.DataAccess
{
    class DicomReader : IDisposable
    {
        public DicomReader(string path)
        {
            this.MinValue = double.NaN;
            this.MaxValue = double.NaN;

            reader = vtkDICOMImageReader.New();
            if (Directory.Exists(path))
            {
                reader.SetDirectoryName(path);
            }
            else if (File.Exists(path))
            {
                reader.SetFileName(path);
            }
            else
            {
                throw new LogException("Can not find file or directory: " + path);
            }

            try
            {
                reader.Update();
            }
            catch (Exception e)
            {
                Log.E("VTK Exception: " + e.Message);
            }
        }

        public void Dispose()
        {
            reader.Dispose();
        }

        private vtkDICOMImageReader reader;

        public double MinValue { get; private set; }
        public double MaxValue { get; private set; }

        public Image3D ReadImage3D(Progress progress)
        {
            // based on http://www.vtk.org/Wiki/VTK/Examples/Cxx/ImageData/IterateImageData

            // conversion value to HU based on http://www.codeproject.com/Articles/466955/Medical-image-visualization-using-WPF

            var imageData = reader.GetOutput();
            var rescaleSlope = reader.GetRescaleSlope();
            var rescaleOffset = reader.GetRescaleOffset();
            int[] dimensions = imageData.GetDimensions();

            if (dimensions.Length != 3)
            {
                throw new LogException("Image must have 3 dimensions!");
            }

            // TODO: remove (allow max 1000 slices)
            if (dimensions[2] > 1000) { dimensions[2] = 1000; }
            // ---

            var image = new Image3D(dimensions[2], dimensions[1], dimensions[0]);

            double min = double.MaxValue;
            double max = double.MinValue;

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

            this.MinValue = min;
            this.MaxValue = max;

            return image;
        }
    }
}
