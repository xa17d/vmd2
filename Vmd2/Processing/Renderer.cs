using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vmd2.Logging;

namespace Vmd2.Processing
{
    abstract class Renderer
    {
        public Renderer(DisplayImage display)
        {
            this.Display = display;
        }

        public DisplayImage Display { get; private set; }

        public void Render(Progress progress)
        {
            OnRender(progress);
            progress.Done();
        }

        protected abstract void OnRender(Progress progress);
    }
}
