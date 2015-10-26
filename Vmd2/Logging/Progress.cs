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
            this.status = ProgressStatus.Running;
            this.TimeMilliseconds = double.NaN;
            this.start = DateTime.Now;
        }

        public void Update(double progress)
        {
            this.progress = progress;
        }

        public void UpdateIncrement(double delta)
        {
            lock (lockObject)
            {
                this.progress += delta;
            }
        }
        
        public void Dispose()
        {
            var end = DateTime.Now;

            if (status == ProgressStatus.Running)
            {
                status = ProgressStatus.Done;
            }
            this.TimeMilliseconds = (end - start).TotalMilliseconds;
        }

        public void Done()
        {
            status = ProgressStatus.Done;
        }

        public void Abort()
        {
            status = ProgressStatus.Aborted;
        }

        private ProgressStatus status;
        private double progress;
        private DateTime start;
        private readonly object lockObject = new object();

        public double TimeMilliseconds { get; private set; }
        public double Value { get { return progress; } }
        public ProgressStatus Status {get { return status; } }

        public override string ToString()
        {
            if (status == ProgressStatus.Running)
            {
                return Value.ToString("0.0%");
            }
            else if (status == ProgressStatus.Done)
            {
                return TimeMilliseconds.ToString() + "ms";
            }
            else if (status == ProgressStatus.Aborted)
            {
                return "aborted";
            }

            throw new Exception("invalid status "+status.ToString());
        }
    }
}
