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

        private double timeOfWaitingA = -1;
        private double timeOfWaitingB = -1;
        private double startOfWaiting = -1;

        public Vehicle(int pVolume, int pSpeed, double pProbability, int pTime, Random generator)
        {
            this.volume = pVolume;
            this.speed = pSpeed;
            this.probabilityOfCrash = pProbability;
            this.timeOfRepair = pTime;
            this.failureGenerator = generator;
        };

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
    }
}
