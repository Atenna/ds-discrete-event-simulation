using Automobilka.Readonly;
using Automobilka.Vehicles;
using Automobilka.Simulations;

namespace Automobilka
{
    class EventLoadFinish : Event
    {
        private SimulationCore _core;
        private double _time;
        private Vehicle _car;
        private int _lengthOfWay; // dlzka cesty z A do B v km

        public EventLoadFinish(SimulationCore actualSimulation, double scheduledTime, Vehicle car) : base(actualSimulation, scheduledTime, actualSimulation.NumberOfEvents)
        {
            this._core = actualSimulation;
            this._time = scheduledTime;
            this._car = car;
            this._lengthOfWay = Constants.AbLength;
            actualSimulation.NumberOfEvents++;
        }
        public override void Execute()
        {
            this._core.CarAtLoader = null;
            double expectedTime = (_lengthOfWay / (double)(_car.GetSpeed() / 60.0)) + _time; // ocakavany cas - kolko by autu trava cesta

            _core.TimeLoadingStart = 0.0;
            _core.MaterialToLoad = 0.0;

            expectedTime = _core.WayAb.RealTime(expectedTime);
            Event arrivalB = new EventArrivalToB(_core, expectedTime, _car);
            _core.UpdateEventCalendar(arrivalB);

            if (_core.MaterialA <= 0)
            {
                return;
            }

            lock (Constants.GateF)
            {
                this._core.GetCarsAb().Add(_car);
            }

            Vehicle carInFront = _core.GetFirstBeforeDepo();
            if (carInFront != null)
            {
                Event loadStart = new EventLoadStart(_core, _time, carInFront);
                _core.UpdateEventCalendar(loadStart);
            }
            else
            {
                _core.LoadMachineWorking = false;
            }
        }
    }
}
