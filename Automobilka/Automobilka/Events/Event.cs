using Automobilka.Simulations;

namespace Automobilka
{
    public abstract class Event
    {

        public double TimeExecution { get; set; }
        public int EventNumber;
        protected SimulationCore MySimulation { get; set; }


        // plus do parametrov Vehicle 
        public Event(SimulationCore actualSimulationCore, double scheduledTime, int order)
        {
            this.MySimulation = actualSimulationCore;
            this.TimeExecution = scheduledTime;
            this.EventNumber = order;
        }

        public double Time()
        {
            return TimeExecution;
        }

        public abstract void Execute();
    }
}
