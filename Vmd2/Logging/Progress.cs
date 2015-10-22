using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vmd2.Logging
{
    class Progress : IDisposable
    {
        public Progress()
        {
            this.progress = 0;
            this.done = false;
            this.TimeMilliseconds = double.NaN;
            this.start = DateTime.Now;
        }

        public void Update(double progress)
        {
            this.progress = progress;
        }
        
        public void Dispose()
        {
            var end = DateTime.Now;

            this.done = true;
            this.TimeMilliseconds = (end - start).TotalMilliseconds;
        }

        private bool done;
        private double progress;
        private DateTime start;

        public double TimeMilliseconds { get; private set; }
        public double Value { get { return progress; } }
        public bool IsDone {get { return done; } }

        public override string ToString()
        {
            if (!IsDone)
            {
                return Value.ToString("0.0%");
            }
            else
            {
                return TimeMilliseconds.ToString() + "ms";
            }
        }
    }
}
