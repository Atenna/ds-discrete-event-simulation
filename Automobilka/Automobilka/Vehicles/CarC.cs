using Automobilka.Readonly;
using System;

namespace Automobilka.Vehicles
{
    class CarC : Vehicle
    {
        public CarC(Random generator) : 
            base("C",Constants.volumeOfVehicleC, Constants.speedOfVehicleC, Constants.probabilityOfCrashOfVehicleC, Constants.timeOfRepairOfVehicleC, generator)
        {

        }
    }
}

