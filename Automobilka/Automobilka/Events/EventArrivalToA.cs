﻿using Automobilka.Readonly;
using Automobilka.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automobilka
{
    class EventArrivalToA : Event
    {
        private SimulationCore core;
        private double time;
        private Vehicle car;

        public EventArrivalToA(SimulationCore actualSimulation, double scheduledTime, Vehicle car) : base(actualSimulation, scheduledTime, actualSimulation.numberOfEvents)
        {
            this.core = actualSimulation;
            this.time = scheduledTime;
            this.car = car;
            lock(Constants.gateF)
            {
                this.core.removeFromCA(car);
            }
            actualSimulation.numberOfEvents++;
        }
        public override void execute()
        {
            // postavia sa do radu
            core.updteListBeforeDepo(car);
            // nastavi sa im pociatocny cas cakania
            car.setStartOfWaiting(time);
            // ak sa nic nenaklada, pride prve auto na rad
            if (core.loadMachineWorking == false)
            {
                if (core.materialA <= 0)
                {
                    return;
                }
                Event loadStart = new EventLoadStart(core, time, core.getFirstBeforeDepo());
                core.updateEventCalendar(loadStart);
                core.loadMachineWorking = true;
            }
        }
    }
}
