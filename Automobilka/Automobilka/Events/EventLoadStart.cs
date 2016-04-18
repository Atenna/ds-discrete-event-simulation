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
            : base(actualSimulation, scheduledTime, actualSimulation.numberOfEvents)
        {
            this.core = actualSimulation;
            this.time = scheduledTime;
            this.car = car;
<<<<<<< HEAD
=======
            this.core.carAtLoader = car;
>>>>>>> NewBranch
            actualSimulation.numberOfEvents++;
        }
        public override void execute()
        {
            this.core.carAtLoader = car;
            double volumeToLoad = (core.materialA <= car.getVolume()) ? core.materialA : car.getVolume();

            core.materialToLoad = volumeToLoad;
            core.timeLoadingStart = time;

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
