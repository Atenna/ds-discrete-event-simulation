using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automobilka.Responsivity
{
    public abstract class ResponsiveCore
    {
        public BackgroundWorker worker { get; }
        public ResponsiveCore(BackgroundWorker worker)
        {
            this.worker = worker;
        }

        public abstract void backgroundProcess();

    }
}
