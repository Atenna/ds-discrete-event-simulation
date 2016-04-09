using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automobilka.Vehicles
{
    public class Vehicle
    {
        private Random failureGenerator;

        private int volume;
        private int speed;
        private double probabilityOfCrash;
        private int timeOfRepair;

        private double timeOfWaitingOnDepo = 0;
        private double timeOfWaitingOnBuilding = 0;
        private double startOfWaiting = 0;

        private double numberOfWaitingOnDepo = 0;
        private double numberOfWaitingOnBuilding = 0;

        public Vehicle(int pVolume, int pSpeed, double pProbability, int pTime, Random generator)
        {
            this.volume = pVolume;
            this.speed = pSpeed;
            this.probabilityOfCrash = pProbability;
            this.timeOfRepair = pTime;
            this.failureGenerator = generator;
        }

        public int getVolume()
        {
            return volume;
        }

        public int getSpeed()
        {
            return speed;
        }

        public double getProbabilityOfCrash()
        {
            return probabilityOfCrash;
        }

        public int getTimeOfRepair()
        {
            return timeOfRepair;
        }

        public void setStartOfWaiting(double time)
        {
            this.startOfWaiting = time;
        }

        public void setEndOfWaitingOnDepo(double time)
        {
            this.timeOfWaitingOnDepo += (time - startOfWaiting);
            // kedze premenna sa pouzije este pri cakani pred uzlom B, treba vynulovat
            this.startOfWaiting = 0;
            numberOfWaitingOnDepo++;
        }

        public double getWaitingOnDepo()
        {
            return timeOfWaitingOnDepo;
        }
        public double getWaitingOnBuilding()
        {
            return timeOfWaitingOnBuilding;
        }
        public double getMeanWaitingOnDepo()
        {
            return timeOfWaitingOnDepo / numberOfWaitingOnDepo;
        }
        public double getMeanWaitingOnBuilding()
        {
            return timeOfWaitingOnBuilding / numberOfWaitingOnBuilding;
        }

        public void setEndOfWaitingOnBuilding(double time)
        {
            this.timeOfWaitingOnBuilding += (time - startOfWaiting);
            this.startOfWaiting = 0;
            numberOfWaitingOnBuilding++;
        }

        // vrati true ak sa auto pokazi
        public bool hasFailed()
        {
            double failed = failureGenerator.NextDouble();

            return failed < probabilityOfCrash;
        }

        public void resetAttributes(Random failureGeneratorReinit)
        {
            timeOfWaitingOnDepo = 0;
            timeOfWaitingOnBuilding = 0;
            startOfWaiting = 0;
            numberOfWaitingOnBuilding = 0;
            numberOfWaitingOnDepo = 0;
            failureGenerator = failureGeneratorReinit;
        }
    }
}
