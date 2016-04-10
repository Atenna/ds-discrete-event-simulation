using Automobilka.Readonly;
using System;

namespace Automobilka.Vehicles
{
    class CarB : Vehicle
    {
        public CarB(Random generator) : 
            base("B",Constants.volumeOfVehicleB, Constants.speedOfVehicleB, Constants.probabilityOfCrashOfVehicleB, Constants.timeOfRepairOfVehicleB, generator)
        {

        }
    }
}

