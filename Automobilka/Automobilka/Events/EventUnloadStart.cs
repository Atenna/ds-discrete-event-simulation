using Automobilka.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automobilka
{
    class EventUnloadStart : Event
    {

        private SimulationCore core;
        private double time;
        private Vehicle car;
        private double speedOfUnloading = 200 / 60.0; // m3 / min
        public EventUnloadStart(SimulationCore actualSimulation, double scheduledTime, Vehicle car) : base(actualSimulation, scheduledTime)
        {
            this.core = actualSimulation;
            this.time = scheduledTime;
            this.car = car;
        }

        public override void execute()
        {
            // nastavi nakladac ze pracuje
            core.unloadMachineWorking = true;

            // nastavi cas koniec cakania v rade
            car.setEndOfWaitingOnBuilding(time);

            // TO-DO vypocita cas nakladania
            double timeOfUnloading = car.realVolume / speedOfUnloading; // v minutach
            car.realVolume = 0;

            // vytvori koniec nakladania
            Event unloadEnd = new EventUnloadFinish(core, timeOfUnloading + time, car);

            // prida koniec nakladania do kalendata udalosti
            core.updateEventCalendar(unloadEnd);
        }
    }
}
