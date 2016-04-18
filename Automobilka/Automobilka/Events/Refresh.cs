using Automobilka.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Automobilka.Events
{
    class Refresh : Event
    {

        private SimulationCore core;
        private double time;

        public Refresh(SimulationCore actualSimulation, double scheduledTime) : base(actualSimulation, scheduledTime, actualSimulation.numberOfEvents)
        {
            this.core = actualSimulation;
            this.time = scheduledTime;
            actualSimulation.numberOfEvents++;
        }

        public override void execute()
        {
            Thread.Sleep(core.getSpeed());
            if (core.isVisualized)
            {
                core.updateEventCalendar(new Refresh(core, time + core.getRepeatTime()));
            }
            else
            {
                core.isRefreshed = false;
            }
        }
    }
}
