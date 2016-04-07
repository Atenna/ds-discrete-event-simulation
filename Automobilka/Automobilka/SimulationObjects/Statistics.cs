using Automobilka.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automobilka.SimulationObjects
{
    public class Statistics
    {
        private List<Vehicle> cars;
        private Queue load;
        private Queue unload;
        private int iterator;
        private double loadSize; // kumulativne sa tam bude ukladat priemerna dlzka radu 
        private double unloadSize;
        private double timeOfWaitingOnBuilding;
        private double timeOfWaitingOnDepo;

        public Statistics()
        {
            cars = new List<Vehicle>();
            iterator = 0;
            loadSize = 0;
            unloadSize = 0;
            timeOfWaitingOnBuilding = 0;
            timeOfWaitingOnDepo = 0;
    }

    public void addVehicleToStats(Vehicle car)
        {
            cars.Add(car);
        }

        public void setLoad(Queue load)
        {
            this.load = load;
        }

        public void setUnload(Queue unload)
        {
            this.unload = unload;
        }

        public void updateStatistics(double simulationTime)
        {
            loadSize += load.getMeanQueueLength(simulationTime);
            unloadSize += unload.getMeanQueueLength(simulationTime);

            foreach(Vehicle v in cars)
            {
                timeOfWaitingOnBuilding += v.getWaitingOnBuilding();
                timeOfWaitingOnDepo += v.getWaitingOnDepo();
            }

            iterator++;
        }

        public double getStatsMeanLoadQueueLength()
        {
            return loadSize / iterator;
        }
        public double getStatsMeanUnloadQueueLength()
        {
            return unloadSize / iterator;
        }
        // statistika pre vsetky auta - priemerna dlza cakania pred nakladacom
        public double getStatsSumMeanLoadQueueTime()
        {
            // asi nebude fungovat hned
            return (timeOfWaitingOnDepo / iterator);
        }
        // statistika pre jedno auto - priemerna dlzka cakania pred nakladacom
        public double getStatsMeanLoadQueueTime()
        {
            return (timeOfWaitingOnDepo / iterator) / cars.Count();
        }
        // statistika pre vsetky auta - priemerna dlzka cakania pred vykladacom
        public double getStatsSumMeanUnloadQueueTime()
        {
            return (timeOfWaitingOnBuilding / iterator);
        }

        public double getStatsMeanUnloadQueueTime()
        {
            return (timeOfWaitingOnBuilding / iterator) / cars.Count();
        }
    }
}
