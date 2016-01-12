using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vmd2.Logging
{
    /// <summary>
    /// Progress-Object that can be used to update progress information
    /// </summary>
    class Progress : IDisposable
    {
        public Progress()
        {
            this.progress = 0;
            this.status = ProgressStatus.Running;
            this.TimeMilliseconds = double.NaN;
            this.start = DateTime.Now;
        }

        /// <summary>
        /// Set new progress
        /// </summary>
        /// <param name="progress">Set Progress. Must be 0..1</param>
        public void Update(double progress)
        {
            this.progress = progress;
        }

        /// <summary>
        /// Thread safe increment the progress by a delta value
        /// </summary>
        /// <param name="delta">delta</param>
        public void UpdateIncrement(double delta)
        {
            lock (lockObject)
            {
                this.progress += delta;
            }
        }
        
        /// <summary>
        /// Stops the progress info
        /// </summary>
        public void Dispose()
        {
            var end = DateTime.Now;

            if (status == ProgressStatus.Running)
            {
                status = ProgressStatus.Done;
            }
            this.TimeMilliseconds = (end - start).TotalMilliseconds;
        }

        /// <summary>
        /// Call when process is done
        /// </summary>
        public void Done()
        {
            status = ProgressStatus.Done;
        }

        /// <summary>
        /// Call when process is aborted
        /// </summary>
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
