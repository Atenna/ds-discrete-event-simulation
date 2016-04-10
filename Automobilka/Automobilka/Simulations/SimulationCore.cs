using Automobilka.Readonly;
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
        protected Random seedGenerator;

        private Queue carsBeforeDepo; //auta pred skladkou
        private Queue carsBeforeBuilding; // auta pred stavbou
        public Vehicle carAtLoader { set; get; }
        public Vehicle carAtUnloader { get; set; }

        public double materialA { get; set; }
        public double materialB { get; set; }

        private double[] _IS;

        public NarrowWay wayAB { get; set; }
        public NarrowWay wayCA { get; set; }

        protected Statistics cruelStats;

        public bool loadMachineWorking { get; set; }
        public bool unloadMachineWorking { get; set; }

        public SimulationCore(double maxTime, int replications, BackgroundWorker worker, Random seedGeneratorInit) : base(maxTime, replications, worker)
        {
            unloadMachineWorking = false;
            loadMachineWorking = false;
            carsBeforeBuilding = new Queue();
            carsBeforeDepo = new Queue();
            seedGenerator = seedGeneratorInit;
        }

        // vytvori novu statistiku a priradi nove fronty
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

        // vypisovanie statistik ... 
        public override void postPostSetup()
        {
            _IS = cruelStats.confidenceIntervalSimulationTime(0.9);

            Console.WriteLine("Priemerne trvanie simulacie (v hodinach): " + cruelStats.getStatsMeanSimulationTime() / 60);

            Console.WriteLine("Priemerna dlzka radu - Nakladka: " + cruelStats.getStatsMeanLoadQueueLength());
            Console.WriteLine("Priemerna dlzka radu - Vykladka: " + cruelStats.getStatsMeanUnloadQueueLength());

            Console.WriteLine("Priemerna dlzka stravena autom cakanim - Nakladka (v minutach): " + cruelStats.getStatsMeanLoadQueueTime());
            Console.WriteLine("Priemerna dlzka stravena autom cakanim - Vykladka (v minutach): " + cruelStats.getStatsMeanUnloadQueueTime());

            Console.WriteLine("Priemerna dlzka cakania vsetkych aut - Nakladka (v hodinach): " + cruelStats.getStatsSumMeanLoadQueueTime() / 60);
            Console.WriteLine("Priemerna dlzka cakania vsetkych aut - Vykladka (v hodinach): " + cruelStats.getStatsSumMeanUnloadQueueTime() / 60);

            Console.WriteLine("Interval spolahlivosti: <" + _IS[0]/60 + ", " + _IS[1]/60);
        }

        public override void preSetup()
        {
            base.preSetup();
            wayAB = new NarrowWay(); // depo - stavba
            wayCA = new NarrowWay(); // prejazd - depo
            materialA = Constants.materialToLoad;
            materialB = 0;
            carsBeforeBuilding.reset();
            carsBeforeDepo.reset();
            resetCars();
            loadMachineWorking = false;
            unloadMachineWorking = false;
        }

        public virtual void resetCars()
        {

        }

        public override bool condition()
        {
            return materialB < Constants.materialToLoad;
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

        public Statistics getStats()
        {
            return cruelStats;
        }

        public Queue getQueueDepo()
        {
            return carsBeforeDepo;
        }

        public Queue getQueueBuilding()
        {
            return carsBeforeBuilding;
        }

    }
}