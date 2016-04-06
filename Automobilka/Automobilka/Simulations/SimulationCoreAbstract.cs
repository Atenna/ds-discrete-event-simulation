using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Automobilka.Simulations
{
    public class SimulationCoreAbstract
    {
        private LinkedList<Event> eventCalendar;
        protected double timeActual;
        protected double maxTime;
        private int numberOfReplications { get; set; }
        public bool isFinished { get; set; }
        
        public SimulationCoreAbstract(double maxTime, int numberOfReplications)
        {
            this.timeActual = 0.0;
            this.maxTime = maxTime;
            this.eventCalendar = new LinkedList<Event>();
            this.numberOfReplications = numberOfReplications;
            this.isFinished = false;
        }

        public void Simulation()
        {
            simulation(100000);
        }

        public void simulation(double timeUntil)
        {
            Event actualEvent;
            int iterator = 0;

            while(iterator < numberOfReplications)
            {
                preSetup();
                resetVariables();
                while (timeActual <= timeUntil && eventCalendar.Any<Event>() && condition())
                {
                    //refresh();    
                    actualEvent = eventCalendar.First<Event>();
                    timeActual = actualEvent.Time();
                    if (timeActual <= timeUntil)
                    {
                        actualEvent.execute();
                    }
                }
                iterator++;
                Console.WriteLine("Replikacia #" + iterator);
            }
            isFinished = true;
        }

        private void resetVariables()
        {
            eventCalendar = new LinkedList<Event>();
            timeActual = 0.0;
        }

        public void updateEventCalendar(Event evt)
        {
            // najst poradie a potom ho pridat na spravne miesto
            eventCalendar.AddLast(evt);
            // to do orderovanie
        }

        public virtual void preSetup() {

        }

        public virtual bool condition()
        {
            return true;
        }

        public virtual void refresh()
        {

        }
    }
}
