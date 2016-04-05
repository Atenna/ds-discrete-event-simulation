using Automobilka.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            this.lengthOfWay = 15;
        }
        public override void execute()
        {
            double expectedTime = (lengthOfWay / (car.getSpeed() / 60)) + time; // ocakavany cas - kolko autu trava cesta

            // poruchovost
            if(car.hasFailed())
            {
                expectedTime += car.getTimeOfRepair();
            }

            core.materialB += car.getVolume();
            if(core.materialB >= 5000)
            {
                core.materialB = 5000;
                return;
            }

            Event arrivalC = new EventArrivalToC(core, expectedTime, car);
            core.updateEventCalendar(arrivalC);
        }
    }
}
