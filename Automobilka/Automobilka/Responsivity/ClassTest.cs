using System.ComponentModel;

namespace Automobilka.Responsivity
{
    class ClassTest
    {
        private int _top;
        private BackgroundWorker _worker;
        public ClassTest(BackgroundWorker worker)
        {
            this._worker = worker;
            _top = 1000;
        }

        public int Calculate()
        {
            int sum = 0;
            int iterator = 1;
            while(iterator < _top)
            {
                sum += iterator;
                _worker.ReportProgress(iterator/10);
                iterator++;
            }
            return sum;
        }
    }
}
