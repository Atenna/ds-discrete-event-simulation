using Automobilka.Readonly;
using Automobilka.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automobilka
{
    class EventArrivalToC : Event
    {

        private SimulationCore core;
        private double time;
        private Vehicle car;
        private int lengthOfWay;

        public EventArrivalToC(SimulationCore actualSimulation, double scheduledTime, Vehicle car) : base(actualSimulation, scheduledTime)
        {
            this.core = actualSimulation;
            this.time = scheduledTime;
            this.car = car;
            this.lengthOfWay = Constants.CALength;

            this.core.carsBC.Remove(car);
            this.core.carsCA.Add(car);
        }
        public override void execute()
        {
            double expectedTime = (lengthOfWay / (car.getSpeed() / 60.0)) + time; // ocakavany cas - kolko by autu trava cesta

            expectedTime = core.wayCA.realTime(expectedTime);
            Event arrivalA = new EventArrivalToA(core, expectedTime, car);
            core.updateEventCalendar(arrivalA);
        }
    }
}
