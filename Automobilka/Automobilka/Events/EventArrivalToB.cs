using Automobilka.Readonly;
using Automobilka.Vehicles;
using Automobilka.Simulations;

namespace Automobilka
{
    class EventArrivalToB : Event
    {
        private SimulationCore _core;
        private double _time;
        private Vehicle _car;

        public EventArrivalToB(SimulationCore actualSimulation, double scheduledTime, Vehicle car) : base(actualSimulation, scheduledTime, actualSimulation.NumberOfEvents)
        {
            this._core = actualSimulation;
            this._time = scheduledTime;
            this._car = car;
            actualSimulation.NumberOfEvents++;
        }
        public override void Execute()
        {
            lock (Constants.GateF)
            {
                this._core.RemoveFromAb(_car);
            }
            // postavia sa do radu
            _core.UpdteListBeforeBuilding(_car);
            // nastavi sa im pociatocny cas cakania
            _car.SetStartOfWaiting(_time);
            // ak sa nic nenaklada, pride prve auto na rad

            if (_core.UnloadMachineWorking == false)
            {
                Event unloadStart = new EventUnloadStart(_core, _time, _core.GetFirstBeforeBuilding());
                _core.UpdateEventCalendar(unloadStart);
                _core.UnloadMachineWorking = true;
            }
        }
    }
}
