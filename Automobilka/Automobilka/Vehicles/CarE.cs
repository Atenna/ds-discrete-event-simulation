using Automobilka.Readonly;
using System;

namespace Automobilka.Vehicles
{
    class CarE : Vehicle
    {
        public CarE(Random generator) : 
            base("E",Constants.volumeOfVehicleE, Constants.speedOfVehicleE, Constants.probabilityOfCrashOfVehicleE, Constants.timeOfRepairOfVehicleE, generator)
        {

        }
    }
}

