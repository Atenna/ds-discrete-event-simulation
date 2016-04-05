using Automobilka.Simulations;
using Automobilka.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automobilka
{
    public class SimulationCore : SimulationCoreAbstract
    {
       
        private LinkedList<Vehicle> carsBeforeDepo; //auta pred skladkou
        private LinkedList<Vehicle> carsBeforeBuilding; // auta pred stavbou

        public NarrowWay wayAB { get; }
        public NarrowWay wayCA { get; }

        public bool loadMachineWorking {get; set;}
        public bool unloadMachineWorking {get; set;}

        public SimulationCore(double maxTime, int replications) : base(maxTime, replications)
        {
            unloadMachineWorking = false;
            loadMachineWorking = false;
        }

        public override void preSetup()
        {
           wayAB  = new NarrowWay(); // depo - stavba
           wayCA  = new NarrowWay(); // prejazd - depo
        }
        

        public void updteListBeforeDepo(Vehicle car)
        {
            carsBeforeDepo.AddLast(car);
        }

        public void updteListBeforeBuilding(Vehicle car)
        {
            carsBeforeBuilding.AddLast(car);
        }

        public Vehicle getFirstBeforeDepo()
        {
            return carsBeforeDepo.First();
        }

        public Vehicle getFirstBeforeBuilding()
        {
            Vehicle car = carsBeforeBuilding.First();
            carsBeforeBuilding.RemoveFirst();
            return car;
        }
    }
}