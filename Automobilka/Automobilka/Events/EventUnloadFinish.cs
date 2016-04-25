using Automobilka.Readonly;
using Automobilka.Simulations;
using Automobilka.Vehicles;

namespace Automobilka
{
    class EventUnloadFinish : Event
    {
        private SimulationCore _core;
        private double _time;
        private Vehicle _car;
        private int _lengthOfWay;

        public EventUnloadFinish(SimulationCore actualSimulation, double scheduledTime, Vehicle car) : base(actualSimulation, scheduledTime, actualSimulation.NumberOfEvents)
        {
            this._core = actualSimulation;
            this._time = scheduledTime;
            this._car = car;
            this._lengthOfWay = Constants.BcLength;
            actualSimulation.NumberOfEvents++;
        }
        public override void Execute()
        {
            this._core.CarAtUnloader = null;
            double expectedTime = (_lengthOfWay / (_car.GetSpeed() / 60.0)) + _time; // ocakavany cas - kolko autu trava cesta

            _core.TimeUnloadingStart = 0.0;
            _core.MaterialToUnload = 0.0;

            // poruchovost
            if (_car.HasFailed())
            {
                expectedTime += _car.GetTimeOfRepair();
            }

            _core.MaterialB += _car.GetVolume();
            if (_core.MaterialB >= Constants.MaterialToLoad)
            {
                _core.MaterialB = Constants.MaterialToLoad;
                return;
            }

            lock (Constants.GateF)
            {
                this._core.GetCarsBc().Add(_car);
            }
            Event arrivalC = new EventArrivalToC(_core, expectedTime, _car);
            _core.UpdateEventCalendar(arrivalC);

            Vehicle carInFront = _core.GetFirstBeforeBuilding();
            if (carInFront != null)
            {
                Event unloadStart = new EventUnloadStart(_core, _time, carInFront);
                _core.UpdateEventCalendar(unloadStart);
            }
            else
            {
                _core.UnloadMachineWorking = false;
            }
        }
    }
}
