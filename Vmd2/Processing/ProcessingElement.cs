using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Vmd2.Logging;

namespace Vmd2.Processing
{
    /// <summary>
    /// Element of the Processing Pipeline.
    /// Gets an input image and must return a output image.
    /// </summary>
    abstract class ProcessingElement : NotifyPropertyChanged
    {
        public ProcessingElement()
        {
            this.Name = GetType().Name;
        }

        private string name;
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get { return name; } set { if (value != name) { name = value; OnPropertyChanged(); } } }

        private ProcessingPipeline pipeline;
        /// <summary>
        /// Pipline this Element is assigned to
        /// </summary>
        public ProcessingPipeline Pipeline { get { return pipeline; } set { if (value != pipeline) { pipeline = value; OnPropertyChanged(); } } }

        /// <summary>
        /// Invoked before the Element is processed
        /// </summary>
        public event EventHandler<ProcessEventArgs> PreProcessing;

        /// <summary>
        /// Process an 3D Image with this element
        /// </summary>
        /// <param name="image">Input image</param>
        /// <returns>Output image</returns>
        public Image3D Process(Image3D image)
        {
            using (var progress = Log.P(ToString()))
            {
                try
                {
                    if (PreProcessing !=null)
                    {
                        PreProcessing(this, new ProcessEventArgs(image));
                    }

                    return OnProcess(image, progress);
                }
                catch (ThreadAbortException)
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

        /// <summary>
        /// Must be implemented to process the image
        /// </summary>
        /// <param name="image">Input image</param>
        /// <param name="progress">Progress of the process</param>
        /// <returns>Output image</returns>
        protected abstract Image3D OnProcess(Image3D image, Progress progress);

        public override string ToString()
        {
            return Name;
        }
    }
}
