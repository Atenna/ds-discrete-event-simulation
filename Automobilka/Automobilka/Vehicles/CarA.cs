using Automobilka.Readonly;
using System;

namespace Automobilka.Vehicles
{
    class CarA : Vehicle
    {
        public CarA(Random generator) : 
            base("A", Constants.VolumeOfVehicleA, Constants.SpeedOfVehicleA, Constants.ProbabilityOfCrashOfVehicleA, Constants.TimeOfRepairOfVehicleA, generator)
        {

        }
    }
}
