using Automobilka.Readonly;
using Automobilka.Simulations;
using Automobilka.Vehicles;

namespace Automobilka
{
    class EventUnloadStart : Event
    {

        private SimulationCore _core;
        private double _time;
        private Vehicle _car;
        private double _speedOfUnloading = Constants.UnloadMachinePerformance;
        public EventUnloadStart(SimulationCore actualSimulation, double scheduledTime, Vehicle car) : base(actualSimulation, scheduledTime, actualSimulation.NumberOfEvents)
        {
            this._core = actualSimulation;
            this._time = scheduledTime;
            this._car = car;
            actualSimulation.NumberOfEvents++;
        }

        public override void Execute()
        {
            this._core.CarAtUnloader = _car;
            // nastavi nakladac ze pracuje
            _core.UnloadMachineWorking = true;

            // guicko 
            _core.MaterialToUnload = _car.RealVolume;
            _core.TimeUnloadingStart = _time;

            // nastavi cas koniec cakania v rade
            _car.SetEndOfWaitingOnBuilding(_time);

            // TO-DO vypocita cas nakladania
            double timeOfUnloading = _car.RealVolume / _speedOfUnloading; // v minutach
            _car.RealVolume = 0;

            // vytvori koniec nakladania
            Event unloadEnd = new EventUnloadFinish(_core, timeOfUnloading + _time, _car);

            // prida koniec nakladania do kalendata udalosti
            _core.UpdateEventCalendar(unloadEnd);
        }
    }
}
