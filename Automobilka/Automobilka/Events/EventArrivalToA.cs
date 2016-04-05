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
        private Vehicle[] cars;

        public EventArrivalToA(SimulationCore actualSimulation, double scheduledTime, params Vehicle[] cars) : base(actualSimulation, scheduledTime)
        {
            this.core = actualSimulation;
            this.time = scheduledTime;
            this.cars = cars;
        }
    public override void execute()
        {
            foreach (Vehicle car in cars)
            {
                // postavia sa do radu
                core.updteListBeforeDepo(car);
                // nastavi sa im pociatocny cas cakania
                car.setStartOfWaiting(time);
                // ak sa nic nenaklada, pride prve auto na rad
                if (core.loadMachineWorking == false)
                {
                    Event loadStart = new EventLoadStart(core, time, core.getFirstBeforeDepo());
                    core.updateEventCalendar(loadStart);
                }                
                
            }
        }
    }
}
