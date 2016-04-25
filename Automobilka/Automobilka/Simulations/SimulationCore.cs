using System;
using System.Collections.Generic;
using System.ComponentModel;
using Automobilka.Events;
using Automobilka.Readonly;
using Automobilka.SimulationObjects;
using Automobilka.Vehicles;

namespace Automobilka.Simulations
{
    public class SimulationCore : SimulationCoreAbstract
    {
        protected Random SeedGenerator;

        private Queue _carsBeforeDepo; //auta pred skladkou
        private Queue _carsBeforeBuilding; // auta pred stavbou
        public Vehicle CarAtLoader { set; get; }
        public Vehicle CarAtUnloader { get; set; }

        public double TimeLoadingStart { get; set; }
        public double TimeUnloadingStart { get; set; }
        public double MaterialToLoad { get; set; }
        public double MaterialToUnload { get; set; }

        private List<Vehicle> _carsAb;
        private List<Vehicle> _carsBc;
        private List<Vehicle> _carsCa;

        public double MaterialA { get; set; }
        public double MaterialB { get; set; }

        private double[] _is;

        public NarrowWay WayAb { get; set; }
        public NarrowWay WayCa { get; set; }

        protected Statistics CruelStats;

        public bool LoadMachineWorking { get; set; }
        public bool UnloadMachineWorking { get; set; }

        public SimulationCore(double maxTime, int replications, BackgroundWorker worker, Random seedGeneratorInit) : base(maxTime, replications, worker)
        {
            UnloadMachineWorking = false;
            LoadMachineWorking = false;
            _carsBeforeBuilding = new Queue();
            _carsBeforeDepo = new Queue();
            SeedGenerator = seedGeneratorInit;
            _carsAb = new List<Vehicle>();
            _carsBc = new List<Vehicle>();
            _carsCa = new List<Vehicle>();
        }

        // vytvori novu statistiku a priradi nove fronty
        public override void PrePreSetup()
        {
            CruelStats = new Statistics();
            CruelStats.SetLoad(_carsBeforeDepo);
            CruelStats.SetUnload(_carsBeforeBuilding);

            AddCars();
        }

        public virtual void AddCars()
        {

        }

        public override void PostSetup()
        {
            CruelStats.UpdateStatistics(TimeActual);
        }

        // vypisovanie statistik ... 
        public override void PostPostSetup()
        {
            _is = CruelStats.ConfidenceIntervalSimulationTime(0.9);

            /*
            Console.WriteLine("Priemerne trvanie simulacie (v hodinach): " + cruelStats.getStatsMeanSimulationTime() / 60);

            Console.WriteLine("Priemerna dlzka radu - Nakladka: " + cruelStats.getStatsMeanLoadQueueLength());
            Console.WriteLine("Priemerna dlzka radu - Vykladka: " + cruelStats.getStatsMeanUnloadQueueLength());

            Console.WriteLine("Priemerna dlzka stravena autom cakanim - Nakladka (v minutach): " + cruelStats.getStatsMeanLoadQueueTime());
            Console.WriteLine("Priemerna dlzka stravena autom cakanim - Vykladka (v minutach): " + cruelStats.getStatsMeanUnloadQueueTime());

            Console.WriteLine("Priemerna dlzka cakania vsetkych aut - Nakladka (v hodinach): " + cruelStats.getStatsSumMeanLoadQueueTime() / 60);
            Console.WriteLine("Priemerna dlzka cakania vsetkych aut - Vykladka (v hodinach): " + cruelStats.getStatsSumMeanUnloadQueueTime() / 60);

            Console.WriteLine("Interval spolahlivosti: <" + _IS[0]/60 + ", " + _IS[1]/60);
            */
        }

        public override void PreSetup()
        {
            base.PreSetup();
            WayAb = new NarrowWay(); // depo - stavba
            WayCa = new NarrowWay(); // prejazd - depo
            MaterialA = Constants.MaterialToLoad;
            MaterialB = 0;
            _carsBeforeBuilding.Reset();
            _carsBeforeDepo.Reset();
            ResetCars();
            LoadMachineWorking = false;
            UnloadMachineWorking = false;
        }

        public virtual void ResetCars()
        {

        }

        public override bool Condition()
        {
            return MaterialB < Constants.MaterialToLoad;
        }
        public void UpdteListBeforeDepo(Vehicle car)
        {
            lock (Constants.Gate)
            {
                _carsBeforeDepo.AddVehicleToEnd(car, TimeActual);
            }
        }

        public void UpdteListBeforeBuilding(Vehicle car)
        {
            lock (Constants.Gate)
            {
                _carsBeforeBuilding.AddVehicleToEnd(car, TimeActual);
            }
        }

        public Vehicle GetFirstBeforeDepo()
        {
            lock (Constants.Gate)
            {
                return _carsBeforeDepo.GetVehicleFromQueue(TimeActual);
            }
        }

        public Vehicle GetFirstBeforeBuilding()
        {
            lock (Constants.Gate)
            {
                return _carsBeforeBuilding.GetVehicleFromQueue(TimeActual);
            }
        }

        public Statistics GetStats()
        {
            return CruelStats;
        }

        public Queue GetQueueDepo()
        {
            return _carsBeforeDepo;
        }

        public Queue GetQueueBuilding()
        {
            return _carsBeforeBuilding;
        }

        public List<Vehicle> GetCarsAb()
        {
            lock (Constants.GateF)
            {
                return _carsAb;
            }
        }

        public void RemoveFromAb(Vehicle car)
        {
            lock (Constants.GateF)
            {
                _carsAb.Remove(car);
            }
        }

        public List<Vehicle> GetCarsBc()
        {
            lock (Constants.GateF)
            {
                return _carsBc;
            }
        }

        public void RemoveFromBc(Vehicle car)
        {
            lock (Constants.GateF)
            {
                _carsBc.Remove(car);
                _carsBc.Remove(car);
            }
        }

        public void RemoveFromCa(Vehicle car)
        {
            lock (Constants.GateF)
            {
                _carsCa.Remove(car);
            }
        }

        public List<Vehicle> GetCarsCa()
        {
            lock (Constants.GateF)
            {
                return _carsCa;
            }
        }

        public double[] GetProgressOfLoading()
        {
            double[] pole = new double[2];
            pole[0] = (TimeActual - TimeLoadingStart) * Constants.LoadMachinePerformance;
            pole[1] = MaterialToLoad;
            return pole;
        }

        public double[] GetProgressOfUnloading()
        {
            double[] pole = new double[2];
            pole[0] = MaterialToUnload - (TimeActual - TimeUnloadingStart) * Constants.UnloadMachinePerformance;
            pole[1] = MaterialToUnload;
            return pole;
        }

        public override void AddRefresh()
        {
            UpdateEventCalendar(new Refresh(this, TimeActual));
        }
    }
}
