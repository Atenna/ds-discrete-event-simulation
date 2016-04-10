using Automobilka.Readonly;
using Automobilka.Vehicles;

namespace Automobilka
{
    class EventUnloadFinish : Event
    {
        private SimulationCore core;
        private double time;
        private Vehicle car;
        private int lengthOfWay;

        public EventUnloadFinish(SimulationCore actualSimulation, double scheduledTime, Vehicle car) : base(actualSimulation, scheduledTime)
        {
            this.core = actualSimulation;
            this.time = scheduledTime;
            this.car = car;
            this.lengthOfWay = Constants.BCLength;
            this.core.carAtUnloader = null;
            this.core.carsBC.Add(car);
        }
        public override void execute()
        {
            double expectedTime = (lengthOfWay / (car.getSpeed() / 60.0)) + time; // ocakavany cas - kolko autu trava cesta

            // poruchovost
            if (car.hasFailed())
            {
                expectedTime += car.getTimeOfRepair();
            }

            core.materialB += car.getVolume();
            if (core.materialB >= Constants.materialToLoad)
            {
                core.materialB = Constants.materialToLoad;
                return;
            }

            Event arrivalC = new EventArrivalToC(core, expectedTime, car);
            core.updateEventCalendar(arrivalC);

            Vehicle carInFront = core.getFirstBeforeBuilding();
            if (carInFront != null)
            {
                Event unloadStart = new EventUnloadStart(core, time, carInFront);
                core.updateEventCalendar(unloadStart);
            }
            else
            {
                core.unloadMachineWorking = false;
            }
        }
    }
}
