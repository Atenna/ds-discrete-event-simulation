using Automobilka.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Statistics;
using MathNet.Numerics.Distributions;

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
        private double simulationTimeCumulative;
        private double simulationTimePower;
        private double minAvgSimTime;
        private double maxAvgSimTime;

        private double meanWaitingOnDepo;
        private double meanWaitingOnBuilding;

        public Statistics()
        {
            cars = new List<Vehicle>();
            iterator = 0;
            loadSize = 0;
            unloadSize = 0;
            timeOfWaitingOnBuilding = 0;
            timeOfWaitingOnDepo = 0;
            minAvgSimTime = 99999;
            maxAvgSimTime = -1;
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
            simulationTimeCumulative += simulationTime;
            simulationTimePower += Math.Pow(simulationTime, 2);

            loadSize += load.getMeanQueueLength(simulationTime);
            unloadSize += unload.getMeanQueueLength(simulationTime);

            foreach (Vehicle v in cars)
            {
                timeOfWaitingOnBuilding += v.getWaitingOnBuilding();
                timeOfWaitingOnDepo += v.getWaitingOnDepo();

                meanWaitingOnDepo += v.getMeanWaitingOnDepo();
                meanWaitingOnBuilding += v.getMeanWaitingOnBuilding();
            }

            iterator++;
            double t = simulationTimeCumulative / iterator;
            minAvgSimTime = t < minAvgSimTime ? t : minAvgSimTime;
            maxAvgSimTime = t > maxAvgSimTime ? t : maxAvgSimTime;
        }
        public double getStatsMeanSimulationTime()
        {
            return simulationTimeCumulative / iterator;
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
            return (meanWaitingOnDepo / iterator) / cars.Count;
        }
        // statistika pre vsetky auta - priemerna dlzka cakania pred vykladacom
        public double getStatsSumMeanUnloadQueueTime()
        {
            return (timeOfWaitingOnBuilding / iterator);
        }

        public double getStatsMeanUnloadQueueTime()
        {
            return (meanWaitingOnBuilding / iterator) / cars.Count;
        }

        public double[] confidenceIntervalSimulationTime(double confidence)
        {
            try {
                confidence = 1 - ((1 - confidence) / 2);
                double[] interval = new double[2];
                double avg = simulationTimeCumulative / iterator;
                double standardDeviation = Math.Sqrt((simulationTimePower / iterator) - Math.Pow(avg, 2));

                double value = 0;

                if (iterator < 30)
                {
                    value = StudentT.InvCDF(0.0, 1.0, iterator, confidence);
                }
                else
                {
                    value = Normal.InvCDF(0, 1, confidence);
                }

                interval[0] = avg - (value * standardDeviation / Math.Sqrt((iterator - 1)));
                interval[1] = avg + (value * standardDeviation / Math.Sqrt((iterator - 1)));

                return interval;
            } catch(ArgumentException ex)
            {
                Console.WriteLine("Nestiham ratat intervaly ;)" + ex.StackTrace);
            }
            return new double[2];
        }

        public double getMinAvgSimTime()
        {
            return minAvgSimTime/60;
        }

        public double getMaxAvgSimTime()
        {
            return maxAvgSimTime/60;
        }
    }
}
