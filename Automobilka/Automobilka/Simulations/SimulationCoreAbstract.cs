using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Automobilka.Responsivity;
using System.ComponentModel;

namespace Automobilka.Simulations
{
    public class SimulationCoreAbstract : ResponsiveCore
    {
        private LinkedList<Event> eventCalendar;
        protected double timeActual;
        protected double maxTime;
        private int numberOfReplications { get; set; }
        public bool isFinished { get; set; }
        
        public SimulationCoreAbstract(double maxTime, int numberOfReplications, BackgroundWorker worker) : base(worker)
        {
            this.timeActual = 0.0;
            this.maxTime = maxTime;
            this.eventCalendar = new LinkedList<Event>();
            this.numberOfReplications = numberOfReplications;
            this.isFinished = false;
        }

        public void simulation()
        {
            Event actualEvent;
            int iterator = 0;

            while(iterator < numberOfReplications)
            {
                preSetup();
                resetVariables();
                while (timeActual <= maxTime && eventCalendar.Any<Event>() && condition())
                {
                    //refresh();    
                    actualEvent = eventCalendar.First<Event>();
                    timeActual = actualEvent.Time();
                    if (timeActual <= maxTime)
                    {
                        actualEvent.execute();
                    }
                }
                iterator++;
                Console.WriteLine("Replikacia #" + iterator);
            }
            isFinished = true;
        }

        public override void backgroundProcess()
        {
            int iterator = 1;
            while (iterator < 100)
            {
                // your code here

                // update GUI
                worker.ReportProgress(iterator / 10);
                iterator++;
            }
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
