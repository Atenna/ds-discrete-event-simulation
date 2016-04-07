using Automobilka.SimulationObjects;
using Automobilka.Simulations;
using Automobilka.Vehicles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Automobilka
{
    public class SimulationCore : SimulationCoreAbstract
    {
        Application smernik;
        private Queue carsBeforeDepo; //auta pred skladkou
        private Queue carsBeforeBuilding; // auta pred stavbou
        public double materialA { get; set; }
        public double materialB { get; set; }

        public NarrowWay wayAB { get; set; }
        public NarrowWay wayCA { get; set; }

        protected Statistics cruelStats;

        public bool loadMachineWorking {get; set;}
        public bool unloadMachineWorking {get; set;}

        public SimulationCore(double maxTime, int replications, BackgroundWorker worker) : base(maxTime, replications, worker)
        {
            unloadMachineWorking = false;
            loadMachineWorking = false;
            carsBeforeBuilding = new Queue();
            carsBeforeDepo = new Queue();
        }

        public override void prePreSetup()
        {
            cruelStats = new Statistics();
            cruelStats.setLoad(carsBeforeDepo);
            cruelStats.setUnload(carsBeforeBuilding);

            addCars();
        }

        public virtual void addCars()
        {

        }

        public override void postSetup()
        {
            cruelStats.updateStatistics(timeActual);
        }

        public override void postPostSetup()
        {
            Console.WriteLine("Priemerna dlzka radu - Nakladka: " + cruelStats.getStatsMeanLoadQueueLength());
            Console.WriteLine("Priemerna dlzka radu - Vykladka: " + cruelStats.getStatsMeanUnloadQueueLength());

            Console.WriteLine("Priemerna dlzka cakania - Nakladka: " + cruelStats.getStatsMeanLoadQueueTime());
            Console.WriteLine("Priemerna dlzka cakania - Vykladka: " + cruelStats.getStatsMeanUnloadQueueTime());
        }

        public override void preSetup()
        {
            base.preSetup();
            wayAB  = new NarrowWay(); // depo - stavba
            wayCA  = new NarrowWay(); // prejazd - depo
            materialA = 5000;
            materialB = 0;
            carsBeforeBuilding.reset();
            carsBeforeDepo.reset();
            resetCars();
        }

        public virtual void resetCars()
        {

        }

        public override bool condition()
        {
            return materialB < 5000;
        }
        public void updteListBeforeDepo(Vehicle car)
        {
            carsBeforeDepo.addVehicleToEnd(car, timeActual);
        }

        public void updteListBeforeBuilding(Vehicle car)
        {
            carsBeforeBuilding.addVehicleToEnd(car, timeActual);
        }

        public Vehicle getFirstBeforeDepo()
        {
            return carsBeforeDepo.getVehicleFromQueue(timeActual);
        }

        public Vehicle getFirstBeforeBuilding()
        {
            return carsBeforeBuilding.getVehicleFromQueue(timeActual);
        }

    }
}