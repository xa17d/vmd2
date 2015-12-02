using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vmd2.Logging;
using Vmd2.Processing;

namespace Vmd2.DataAccess
{
    class FileToSliceDicomReader : IDisposable
    {
        public FileToSliceDicomReader(string path)
        {
            this.path = path;
        }

        private string path;

        public double MinValue { get; private set; }
        public double MaxValue { get; private set; }

        private int slicesDone = 0;
        private readonly object statusLockObject = new object();

        private Progress progress;
        private Image3D image;
        private bool disposed = false;

        public Image3D ReadImage3D(Progress progress)
        {
            this.progress = progress;

            var directory = new DirectoryInfo(path);
            var files = directory.GetFiles();

            // TODO: remove (allow max 900 slices)
            if (files.Length > 900)
            {
                Array.Resize<FileInfo>(ref files, 900);
            }
            // ---

            Image3D slice0;
            using (var reader0 = new DicomReader(files[0].FullName))
            {
                slice0 = reader0.ReadImage3D(new Progress());
            }
            this.image = new Image3D(files.Length, slice0.LengthY, slice0.LengthX);
            this.image.CloneSlice(slice0, 0, 0);
            MinValue = Math.Min(MinValue, slice0.Minimum);
            MaxValue = Math.Max(MaxValue, slice0.Maximum);
            slice0 = null;
            slicesDone++;

            //ThreadPool.SetMaxThreads(8, 8);

            for (int z = 1; z < files.Length; z++)
            {
                ReadSlice(new SliceInfo(files[z], z));
                //ThreadPool.QueueUserWorkItem(new WaitCallback(ReadSlice), );
            }

            while (slicesDone != image.LengthZ)
            {
                Thread.Sleep(50);
            }

            progress.Done();

            path = null;
            return image;
        }

        private void ReadSlice(object sliceInfoObj)
        {
            if (disposed) { return; }

            var info = (SliceInfo)sliceInfoObj;

            Image3D slice;
            using (var reader = new DicomReader(info.File.FullName))
            {
                slice = reader.ReadImage3D(new Progress());
            }

            var imageCopy = image; // is not null before disposed.
            if (disposed) { return; }

            imageCopy.CloneSlice(slice, 0, info.Index);

            lock (statusLockObject)
            {
                MinValue = Math.Min(MinValue, slice.Minimum);
                MaxValue = Math.Max(MaxValue, slice.Maximum);
                slicesDone++;
            }

            progress.UpdateIncrement((double)1.0 / image.LengthZ);
        }

        private class SliceInfo
        {
            public SliceInfo(FileInfo file, int index)
            {
                this.File = file;

                this.Index = index;
            }

            public FileInfo File { get; private set; }
            public int Index { get; private set; }
        }

        public void Dispose()
        {
            disposed = true;
            image = null;
            progress = null;
        }
    }
}
