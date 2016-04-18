using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automobilka
{
    public abstract class Event
    {

        public double timeExecution { get; set; }
        public int eventNumber;
        protected SimulationCore mySimulation { get; set; }


        // plus do parametrov Vehicle 
        public Event(SimulationCore actualSimulationCore, double scheduledTime, int order)
        {
            this.mySimulation = actualSimulationCore;
            this.timeExecution = scheduledTime;
            this.eventNumber = order;
        }

        public double Time()
        {
            return timeExecution;
        }

        public abstract void execute();
    }
}
