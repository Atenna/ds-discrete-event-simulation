using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automobilka.Vehicles
{
    class Vehicle
    {
        private int volume;
        private int speed;
        private double probabilityOfCrash;
        private int timeOfRepair;

        public Vehicle(int pVolume, int pSpeed, double pProbability, int pTime)
        {
            this.volume = pVolume;
            this.speed = pSpeed;
            this.probabilityOfCrash = pProbability;
            this.timeOfRepair = pTime;
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
