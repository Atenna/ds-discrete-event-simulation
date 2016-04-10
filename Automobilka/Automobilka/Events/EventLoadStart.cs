using Automobilka.Vehicles;
using Automobilka.Readonly;
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
        private double speedOfLoading = Constants.loadMachinePerformance;
        public EventLoadStart(SimulationCore actualSimulation, double scheduledTime, Vehicle car)
            : base(actualSimulation, scheduledTime)
        {
            this.core = actualSimulation;
            this.time = scheduledTime;
            this.car = car;
            this.core.carAtLoader = car;
        }
        public override void execute()
        {
            double volumeToLoad = (core.materialA <= car.getVolume()) ? core.materialA : car.getVolume();

            core.materialA -= volumeToLoad;

            double timeOfLoading = volumeToLoad / speedOfLoading;

            car.realVolume = volumeToLoad;
            
            core.loadMachineWorking = true;
            
            car.setEndOfWaitingOnDepo(time);

            Event loadEnd = new EventLoadFinish(core, timeOfLoading + time, car);

            core.updateEventCalendar(loadEnd);
        }
    }
}
