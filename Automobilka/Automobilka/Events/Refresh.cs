using System.Threading;
using Automobilka.Simulations;

namespace Automobilka.Events
{
    class Refresh : Event
    {

        private SimulationCore _core;
        private double _time;

        public Refresh(SimulationCore actualSimulation, double scheduledTime) : base(actualSimulation, scheduledTime, actualSimulation.NumberOfEvents)
        {
            this._core = actualSimulation;
            this._time = scheduledTime;
            actualSimulation.NumberOfEvents++;
        }

        public override void Execute()
        {
            Thread.Sleep(_core.GetSpeed());
            if (_core.IsVisualized)
            {
                _core.UpdateEventCalendar(new Refresh(_core, _time + _core.GetRepeatTime()));
            }
            else
            {
                _core.IsRefreshed = false;
            }
        }
    }
}
