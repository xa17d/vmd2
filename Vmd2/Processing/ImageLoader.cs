using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vmd2.DataAccess;
using Vmd2.Logging;

namespace Vmd2.Processing
{
    class ImageLoader : ProcessingElement
    {
        private Image3D cache = null;

        private string path = TestData.GetPath(@"vtkBrain");
        public string Path
        {
            get { return path; }
            set
            {
                if (value != path)
                {
                    path = value;
                    cache = null;
                    OnPropertyChanged();
                }
            }
        }


        protected override Image3D OnProcess(Image3D image, Progress progress)
        {
            if (cache == null)
            {
                using (var reader = new DicomReader(Path))
                {
                    cache = reader.ReadImage3D(progress);
                    cache.Minimum = reader.MinValue;
                    cache.Maximum = reader.MaxValue;
                }
            }

            return cache;
        }
    }
}
