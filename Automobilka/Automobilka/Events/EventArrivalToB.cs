using Automobilka.Readonly;
using Automobilka.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automobilka
{
    class EventArrivalToB : Event
    {
        private SimulationCore core;
        private double time;
        private Vehicle car;

        public EventArrivalToB(SimulationCore actualSimulation, double scheduledTime, Vehicle car) : base(actualSimulation, scheduledTime)
        {
            this.core = actualSimulation;
            this.time = scheduledTime;
            this.car = car;
            lock(Constants.gateF)
            {
                if (this.core.getCarsAB().Contains(car))
                {
                    this.core.removeFromAB(car);
                }
            }
        }
        public override void execute()
        {
            // postavia sa do radu
            core.updteListBeforeBuilding(car);
            // nastavi sa im pociatocny cas cakania
            car.setStartOfWaiting(time);
            // ak sa nic nenaklada, pride prve auto na rad

            if (core.unloadMachineWorking == false)
            {
                Event unloadStart = new EventUnloadStart(core, time, core.getFirstBeforeBuilding());
                core.updateEventCalendar(unloadStart);
                core.unloadMachineWorking = true;
            }
        }
    }
}
