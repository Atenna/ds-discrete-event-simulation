using Automobilka.Vehicles;
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
        private Vehicle car;
        private double speedOfLoading = 180/60; // m3 / min
        public EventLoadStart(SimulationCore actualSimulation, double scheduledTime, Vehicle car) : base(actualSimulation, scheduledTime)
        {
            this.core = actualSimulation;
            this.time = scheduledTime;
            this.car = car;
        }
        public override void execute()
        {
            // nastavi nakladac ze pracuje
            core.loadMachineWorking = true;

            // nastavi cas koniec cakania v rade
            car.setEndOfWaitingOnDepo(time);

            // TO-DO vypocita cas nakladania
            double timeOfLoading = car.getVolume() / speedOfLoading; // v minutach

            // vytvori koniec nakladania
            Event loadEnd = new EventLoadFinish(core, timeOfLoading + time, car);

            // prida koniec nakladania do kalendata udalosti
            core.updateEventCalendar(loadEnd);
        }
    }
}
