using System.ComponentModel;

namespace Automobilka.Responsivity
{
    public abstract class ResponsiveCore
    {
        public BackgroundWorker Worker { get; }
        public ResponsiveCore(BackgroundWorker worker)
        {
            this.Worker = worker;
        }

        public abstract void BackgroundProcess();

    }
}
