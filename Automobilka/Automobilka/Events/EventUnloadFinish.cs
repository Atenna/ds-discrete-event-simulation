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

        public EventUnloadFinish(SimulationCore actualSimulation, double scheduledTime, Vehicle car) : base(actualSimulation, scheduledTime, actualSimulation.numberOfEvents)
        {
            this.core = actualSimulation;
            this.time = scheduledTime;
            this.car = car;
            this.lengthOfWay = Constants.BCLength;
            actualSimulation.numberOfEvents++;
        }
        public override void execute()
        {
            this.core.carAtUnloader = null;
            double expectedTime = (lengthOfWay / (car.getSpeed() / 60.0)) + time; // ocakavany cas - kolko autu trava cesta

            core.timeUnloadingStart = 0.0;
            core.materialToUnload = 0.0;

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

            lock (Constants.gateF)
            {
                this.core.getCarsBC().Add(car);
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
