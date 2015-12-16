using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vmd2.Processing
{
    class ProcessingPipeline : ObservableCollection<ProcessingElement>
    {
        public ProcessingPipeline(string name, IEnumerable<ProcessingElement> initialElements) : base()
        {
            this.Name = name;

            if (initialElements != null)
            {
                foreach (var item in initialElements)
                {
                    Add(item);
                }
            }
        }

        public string Name { get; private set; }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);

            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    var pe = (ProcessingElement)item;
                    //pe.PropertyChanged -= Pe_PropertyChanged;
                    //pe.Pipeline = null;
                }
            }

            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    var pe = (ProcessingElement)item;
                    pe.Pipeline = this;
                    pe.PropertyChanged += Pe_PropertyChanged;
                }
            }

            InvokePipelineChanged();
        }

        private void Pe_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            InvokePipelineChanged();
        }

        private void InvokePipelineChanged()
        {
            var e = PipelineChanged;
            if (e != null)
            {
                e(this, EventArgs.Empty);
            }
        }

        public void Process()
        {
            Image3D image = null;
            try
            {
                foreach (var pe in this.ToArray())
                {
                    image = pe.Process(image);
                }
            }
            catch (LogException) { }
        }

        public event EventHandler PipelineChanged;

        public override string ToString()
        {
            return Name;
        }
    }
}
