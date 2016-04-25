using Automobilka.Readonly;
using Automobilka.Vehicles;
using Automobilka.Simulations;

namespace Automobilka
{
    class EventArrivalToA : Event
    {
        private SimulationCore _core;
        private double _time;
        private Vehicle _car;

        public EventArrivalToA(SimulationCore actualSimulation, double scheduledTime, Vehicle car) : base(actualSimulation, scheduledTime, actualSimulation.NumberOfEvents)
        {
            this._core = actualSimulation;
            this._time = scheduledTime;
            this._car = car;
            lock(Constants.GateF)
            {
                this._core.RemoveFromCa(car);
            }
            actualSimulation.NumberOfEvents++;
        }
        public override void Execute()
        {
            // postavia sa do radu
            _core.UpdteListBeforeDepo(_car);
            // nastavi sa im pociatocny cas cakania
            _car.SetStartOfWaiting(_time);
            // ak sa nic nenaklada, pride prve auto na rad
            if (_core.LoadMachineWorking == false)
            {
                if (_core.MaterialA <= 0)
                {
                    return;
                }
                Event loadStart = new EventLoadStart(_core, _time, _core.GetFirstBeforeDepo());
                _core.UpdateEventCalendar(loadStart);
                _core.LoadMachineWorking = true;
            }
        }
    }
}
