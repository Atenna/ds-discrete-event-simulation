using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automobilka.Responsivity
{
    public class ResponsiveCore
    {
        protected BackgroundWorker worker { get; }
        public ResponsiveCore(BackgroundWorker worker)
        {
            this.worker = worker;
        }

        public virtual void backgroundProcess()
        {
            int iterator = 1;
            while (iterator < 100)
            {
                // your code here

                // update GUI
                worker.ReportProgress(iterator / 10);
                iterator++;
            }
        }

    }
}
