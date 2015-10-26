using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Vmd2.Logging;
using Vmd2.Processing;
using Vmd2.Processing.TransferFunctions;

namespace Vmd2.Presentation.ViewModels
{
    class RenderVm : NotifyPropertyChanged
    {
        private Image3D image;
        public Image3D Image
        {
            get { return image; }
            set
            {
                if (image != value)
                {
                    image = value;
                    OnPropertyChanged();
                }
            }
        }

        private Renderer renderer;
        public Renderer Renderer
        {
            get { return renderer; }
            set
            {
                if (renderer != value)
                {
                    renderer = value;
                    OnPropertyChanged();
                }
            }
        }

        public void Render()
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(DoRender), null);
        }

        private void DoRender(object state)
        {
            using (var progress = Log.P("Render " + Renderer.ToString()))
            {
                try
                {
                    //System.Threading.Thread.Sleep(1000);
                    Renderer.Render(progress);
                }
                catch (ThreadAbortException)
                {
                    progress.Abort();
                }
            }

            Application.Current.Dispatcher.Invoke(Renderer.Display.Update);
        }
    }
}
