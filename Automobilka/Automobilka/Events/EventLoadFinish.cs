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

        public EventLoadFinish(SimulationCore actualSimulation, double scheduledTime, Vehicle car) : base(actualSimulation, scheduledTime)
        {
            this.core = actualSimulation;
            this.time = scheduledTime;
            this.car = car;
            this.lengthOfWay = Constants.ABLength;
        }
        public override void execute()
        {

            double expectedTime = (lengthOfWay / (double)(car.getSpeed() / 60.0)) + time; // ocakavany cas - kolko by autu trava cesta

            expectedTime = core.wayAB.realTime(expectedTime);
            Event arrivalB = new EventArrivalToB(core, expectedTime, car);
            core.updateEventCalendar(arrivalB);

            if (core.materialA <= 0)
            {
                return;
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
