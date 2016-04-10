﻿using Automobilka.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automobilka.SimulationObjects
{
    public class Queue
    {
        private List<Vehicle> queue;
        private double sumOfTime; // in queue * number of cars
        private double lastChange;

        public Queue()
        {
            queue = new List<Vehicle>();
            sumOfTime = 0.0;
            lastChange = 0.0;
        }

        public void addVehicleToEnd(Vehicle car, double simulationTime)
        {
            updateQueue(simulationTime);
            queue.Add(car);
        }

        public Vehicle getVehicleFromQueue(double simulationTime)
        {
            updateQueue(simulationTime);

            if (queue.Any())
            {
                Vehicle toReturn = queue.First();

                queue.RemoveAt(0);
                return toReturn;
            }
            else return null;

        }

        public void updateQueue(double simulationTime)
        {
            sumOfTime += (simulationTime - lastChange) * queue.Count();
            lastChange = simulationTime;
        }

        public int getSize()
        {
            return queue.Count();
        }

        public double getMeanQueueLength(double simulationTime)
        {
            updateQueue(simulationTime);
            return (sumOfTime / simulationTime);
        }

        public void reset()
        {
            queue = new List<Vehicle>();
            sumOfTime = 0.0;
            lastChange = 0.0;
        }

        public Vehicle getAt(int i)
        {
                return queue.ElementAt(i);
        }

        public List<Vehicle> getList()
        {
            return queue;
        }
    }
}
