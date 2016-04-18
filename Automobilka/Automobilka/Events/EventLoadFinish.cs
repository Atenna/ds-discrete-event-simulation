using Automobilka.Readonly;
using Automobilka.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automobilka
{
    class EventLoadFinish : Event
    {
        private SimulationCore core;
        private double time;
        private Vehicle car;
        private int lengthOfWay; // dlzka cesty z A do B v km

        public EventLoadFinish(SimulationCore actualSimulation, double scheduledTime, Vehicle car) : base(actualSimulation, scheduledTime, actualSimulation.numberOfEvents)
        {
            this.core = actualSimulation;
            this.time = scheduledTime;
            this.car = car;
            this.lengthOfWay = Constants.ABLength;
            actualSimulation.numberOfEvents++;
        }
        public override void execute()
        {
            this.core.carAtLoader = null;
            double expectedTime = (lengthOfWay / (double)(car.getSpeed() / 60.0)) + time; // ocakavany cas - kolko by autu trava cesta

            core.timeLoadingStart = 0.0;
            core.materialToLoad = 0.0;

            expectedTime = core.wayAB.realTime(expectedTime);
            Event arrivalB = new EventArrivalToB(core, expectedTime, car);
            core.updateEventCalendar(arrivalB);

            if (core.materialA <= 0)
            {
                return;
            }

            lock (Constants.gateF)
            {
                this.core.getCarsAB().Add(car);
            }

            Vehicle carInFront = core.getFirstBeforeDepo();
            if (carInFront != null)
            {
                Event loadStart = new EventLoadStart(core, time, carInFront);
                core.updateEventCalendar(loadStart);
            }
            else
            {
                core.loadMachineWorking = false;
            }
        }
    }
}
