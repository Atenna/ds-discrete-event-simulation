using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automobilka
{
    public abstract class Event
    {

        private double timeExecution;

        public double Time()
        {
            return timeExecution;
        }

        public abstract void execute();
    }
}
