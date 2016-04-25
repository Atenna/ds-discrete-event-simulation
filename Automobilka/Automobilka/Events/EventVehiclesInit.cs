using Automobilka.Vehicles;
using Automobilka.Simulations;

namespace Automobilka.Events
{
    class EventVehiclesInit : Event
    {
        private SimulationCore _core;
        private double _time;
        private Vehicle[] _cars;

        public EventVehiclesInit(SimulationCore actualSimulation, double scheduledTime, params Vehicle[] cars) : base(actualSimulation, 0, actualSimulation.NumberOfEvents)
        {
            this._core = actualSimulation;
            this._time = scheduledTime;
            this._cars = cars;
            actualSimulation.NumberOfEvents++;
        }
        public override void Execute()
        {
            // vsetky poslem aby prisli pred depo v case 0
            foreach (Vehicle v in _cars)
            {
                Event arrival = new EventArrivalToA(_core, _time, v);
                _core.UpdateEventCalendar(arrival);
            }
        }
    }
}
