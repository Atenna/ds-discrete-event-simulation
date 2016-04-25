using Automobilka.Readonly;
using Automobilka.Vehicles;
using Automobilka.Simulations;

namespace Automobilka
{
    class EventArrivalToC : Event
    {

        private SimulationCore _core;
        private double _time;
        private Vehicle _car;
        private int _lengthOfWay;

        public EventArrivalToC(SimulationCore actualSimulation, double scheduledTime, Vehicle car) : base(actualSimulation, scheduledTime, actualSimulation.NumberOfEvents)
        {
            this._core = actualSimulation;
            this._time = scheduledTime;
            this._car = car;
            this._lengthOfWay = Constants.CaLength;
            actualSimulation.NumberOfEvents++;
        }
        public override void Execute()
        {
            lock (Constants.GateF)
            {
                this._core.RemoveFromBc(_car);
                this._core.GetCarsCa().Add(_car);
            }
            double expectedTime = (_lengthOfWay / (_car.GetSpeed() / 60.0)) + _time; // ocakavany cas - kolko by autu trava cesta

            expectedTime = _core.WayCa.RealTime(expectedTime);
            Event arrivalA = new EventArrivalToA(_core, expectedTime, _car);
            _core.UpdateEventCalendar(arrivalA);
        }
    }
}
