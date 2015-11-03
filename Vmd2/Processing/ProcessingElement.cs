using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vmd2.Logging;

namespace Vmd2.Processing
{

    abstract class ProcessingElement : NotifyPropertyChanged
    {
        public ProcessingElement()
        {
            this.Name = GetType().Name;
        }

        private string name;
        public string Name { get { return name; } set { if (value != name) { name = value; OnPropertyChanged(); } } }

        private ProcessingPipeline pipeline;
        public ProcessingPipeline Pipeline { get { return pipeline; } set { if (value != pipeline) { pipeline = value; OnPropertyChanged(); } } }

        public Image3D Process(Image3D image)
        {
            using (var progress = Log.P(Name))
            {
                try
                {
                    return OnProcess(image, progress);
                }
                catch(ThreadAbortException)
                {
                    progress.Abort();
                    throw;
                }
                catch (LogException e)
                {
                    progress.Abort();
                    Log.E(e.Message);
                    throw;
                }
            }
        }

        protected abstract Image3D OnProcess(Image3D image, Progress progress);

        public override string ToString()
        {
            return Name;
        }
    }
}
