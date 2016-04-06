using System;

namespace Automobilka
{
    internal class ThreadStart
    {
        private Action simulation;

        public ThreadStart(Action simulation)
        {
            this.simulation = simulation;
        }
    }
}