﻿using Automobilka.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automobilka
{
    class EventLoadStart : Event
    {
        private SimulationCore core;
        private double time;
        private Vehicle[] cars;

        public EventLoadStart(SimulationCore actualSimulation, double scheduledTime, params Vehicle[] cars) : base(actualSimulation, scheduledTime)
        {
            this.core = actualSimulation;
            this.time = scheduledTime;
            this.cars = cars;
        }
        public override void execute()
        {
            
            throw new NotImplementedException();
        }
    }
}
