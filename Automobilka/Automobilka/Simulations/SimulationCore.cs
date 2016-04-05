using Automobilka.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automobilka
{
    public class SimulationCore
    {
        private double timeActual;
        // maxsimulacny cas

        private LinkedList<Event> eventCalendar;
        private LinkedList<Vehicle> carsBeforeDepo; //auta pred skladkou
        private LinkedList<Vehicle> carsBeforeBuilding; // auta pred stavbou

        public SimulationCore()
        {
            this.eventCalendar = new LinkedList<Event>();
        }

        public void simulation(double timeUntil)
        {
            Event actualEvent;
            while (timeActual <= timeUntil && eventCalendar.Any<Event>())
            {
                actualEvent = eventCalendar.First<Event>();
                timeActual = actualEvent.Time();
                if (timeActual <= timeUntil)
                {
                    actualEvent.execute();
                }
            }
        }

        // prida do zoznamu novy event podla casu
        public void updateEventCalendar(Event evt)
        {
            // najst poradie a potom ho pridat na spravne miesto
            eventCalendar.AddLast(evt);
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
            return carsBeforeBuilding.First();
        }
    }
}