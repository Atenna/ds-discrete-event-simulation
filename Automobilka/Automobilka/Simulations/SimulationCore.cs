using Automobilka.Simulations;
using Automobilka.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Automobilka
{
    public class SimulationCore : SimulationCoreAbstract
    {
        Application smernik;
        private LinkedList<Vehicle> carsBeforeDepo; //auta pred skladkou
        private LinkedList<Vehicle> carsBeforeBuilding; // auta pred stavbou
        public double materialA { get; set; }
        public double materialB { get; set; }

        public NarrowWay wayAB { get; set; }
        public NarrowWay wayCA { get; set; }

        public bool loadMachineWorking {get; set;}
        public bool unloadMachineWorking {get; set;}

        public SimulationCore(double maxTime, int replications) : base(maxTime, replications)
        {
            unloadMachineWorking = false;
            loadMachineWorking = false;
            materialA = 5000;
            materialB = 0;
    }

        public override void preSetup()
        {
           wayAB  = new NarrowWay(); // depo - stavba
           wayCA  = new NarrowWay(); // prejazd - depo
           materialA = 5000;
           materialB = 0;
        }

        public override bool condition()
        {
            return materialB < 5000;
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

        public override void refresh()
        {
            Application.DoEvents();
        }

        public void setApp(Application app)
        {
            this.smernik = app;
        }
    }
}