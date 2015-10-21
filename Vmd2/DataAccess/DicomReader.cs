using Kitware.VTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vmd2.Processing;

namespace Vmd2.DataAccess
{
    class DicomReader : IDisposable
    {
        public DicomReader(string directoryPath)
        {
            reader = vtkDICOMImageReader.New();
            reader.SetDirectoryName(directoryPath);
            reader.Update();
        }

        public void Dispose()
        {
            reader.Dispose();
        }

        private vtkDICOMImageReader reader;

        public Image3D ReadImage3D()
        {
            // based on http://www.vtk.org/Wiki/VTK/Examples/Cxx/ImageData/IterateImageData

            var imageData = reader.GetOutput();
            int[] dimensions = imageData.GetDimensions();

            if (dimensions.Length != 3)
            {
                throw new Exception("Image must have 3 dimensions!");
            }

            var image = new Image3D(dimensions[2], dimensions[1], dimensions[0]);
            
            for (int z = 0; z < dimensions[2]; z++)
            {
                for (int y = 0; y < dimensions[1]; y++)
                {
                    for (int x = 0; x < dimensions[0]; x++)
                    {
                        double voxel = imageData.GetScalarComponentAsDouble(x, y, z, 0);
                        image[x, y, z] = voxel;
                    }
                }
            }

            return image;
        }
    }
}
