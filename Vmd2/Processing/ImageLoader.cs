﻿using System;
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

        private bool combineFilesToSlices = false;
        public bool CombineFilesToSlices
        {
            get { return combineFilesToSlices; }
            set
            {
                if (value != combineFilesToSlices)
                {
                    combineFilesToSlices = value;
                    cache = null;
                    OnPropertyChanged();
                }
            }
        }

        protected override Image3D OnProcess(Image3D image, Progress progress)
        {
            if (cache == null)
            {
                if (!CombineFilesToSlices)
                {
                    using (var reader = new DicomReader(Path))
                    {
                        cache = reader.ReadImage3D(progress);
                        cache.Minimum = reader.MinValue;
                        cache.Maximum = reader.MaxValue;
                    }
                }
                else
                {
                    using (var reader = new FileToSliceDicomReader(Path))
                    {
                        cache = reader.ReadImage3D(progress);
                        cache.Minimum = reader.MinValue;
                        cache.Maximum = reader.MaxValue;
                    }
                }

                Log.I("Image loaded. min: " + cache.Minimum + "; max: " + cache.Maximum);
            }

            return cache;
        }
    }
}
