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

<<<<<<< HEAD
        public EventVehiclesInit(SimulationCore actualSimulation, double scheduledTime, params Vehicle[] cars) : base(actualSimulation, 0, actualSimulation.numberOfEvents)
=======
        public EventVehiclesInit(SimulationCore actualSimulation, double scheduledTime, params Vehicle[] cars) : 
            base(actualSimulation, 0, actualSimulation.numberOfEvents)
>>>>>>> NewBranch
        {
            this.core = actualSimulation;
            this.time = scheduledTime;
            this.cars = cars;
            actualSimulation.numberOfEvents++;
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
