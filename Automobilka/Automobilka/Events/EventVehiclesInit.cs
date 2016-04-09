using Automobilka.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automobilka.Events
{
    class EventVehiclesInit : Event
    {
        private SimulationCore core;
        private double time;
        private Vehicle[] cars;

        public EventVehiclesInit(SimulationCore actualSimulation, double scheduledTime, params Vehicle[] cars) : base(actualSimulation, 0)
        {
            this.core = actualSimulation;
            this.time = scheduledTime;
            this.cars = cars;
        }
        public override void execute()
        {
            // vsetky poslem aby prisli pred depo v case 0
            foreach (Vehicle v in cars)
            {
                Event arrival = new EventArrivalToA(core, time, v);
                core.updateEventCalendar(arrival);
            }
        }
    }
}
