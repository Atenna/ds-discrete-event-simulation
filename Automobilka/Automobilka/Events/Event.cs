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
        protected SimulationCore mySimulation { get; set; }

        // plus do parametrov Vehicle 
        public Event(SimulationCore actualSimulationCore, double scheduledTime)
        {
            this.mySimulation = actualSimulationCore;
            this.timeExecution = scheduledTime;
        }

        public double Time()
        {
            return timeExecution;
        }

        public abstract void execute();
    }
}
