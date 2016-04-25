using Automobilka.Readonly;
using System;

namespace Automobilka.Vehicles
{
    class CarB : Vehicle
    {
        public CarB(Random generator) : 
            base("B",Constants.VolumeOfVehicleB, Constants.SpeedOfVehicleB, Constants.ProbabilityOfCrashOfVehicleB, Constants.TimeOfRepairOfVehicleB, generator)
        {

        }
    }
}

