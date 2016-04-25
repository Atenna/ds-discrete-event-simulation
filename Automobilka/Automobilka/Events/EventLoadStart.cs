using Automobilka.Vehicles;
using Automobilka.Readonly;
using Automobilka.Simulations;

namespace Automobilka
{
    class EventLoadStart : Event
    {
        private SimulationCore _core;
        private double _time;
        private Vehicle _car;
        private double _speedOfLoading = Constants.LoadMachinePerformance;
        public EventLoadStart(SimulationCore actualSimulation, double scheduledTime, Vehicle car)
            : base(actualSimulation, scheduledTime, actualSimulation.NumberOfEvents)
        {
            this._core = actualSimulation;
            this._time = scheduledTime;
            this._car = car;
            actualSimulation.NumberOfEvents++;
        }
        public override void Execute()
        {
            this._core.CarAtLoader = _car;
            double volumeToLoad = (_core.MaterialA <= _car.GetVolume()) ? _core.MaterialA : _car.GetVolume();

            _core.MaterialToLoad = volumeToLoad;
            _core.TimeLoadingStart = _time;

            _core.MaterialA -= volumeToLoad;

            double timeOfLoading = volumeToLoad / _speedOfLoading;

            _car.RealVolume = volumeToLoad;

            _core.LoadMachineWorking = true;

            _car.SetEndOfWaitingOnDepo(_time);

            Event loadEnd = new EventLoadFinish(_core, timeOfLoading + _time, _car);

            _core.UpdateEventCalendar(loadEnd);
        }
    }
}
