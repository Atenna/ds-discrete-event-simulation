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
        private List<Event> eventCalendar;

        public Event init { set; get; }
        protected double timeActual;
        protected double maxTime;

        private int numberOfReplications { get; set; }
        public bool isFinished { get; set; }
        
        public SimulationCoreAbstract(double maxTime, int numberOfReplications, BackgroundWorker worker) : base(worker)
        {
            this.timeActual = 0.0;
            this.maxTime = maxTime;
            this.eventCalendar = new List<Event>();
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
        // vytvori statistiky, init. .. etc
        public virtual void prePreSetup()
        {

        }

        public override void backgroundProcess()
        {
            Event actualEvent;
            int iterator = 0;
            double progress;

            // vytvorenie aut
            prePreSetup();
            while (iterator < numberOfReplications)
            {
                resetVariables();
                preSetup();
                //progress = (iterator / numberOfReplications)*100;
                //Console.WriteLine("Vonkajsi cyklus " + iterator);
                worker.ReportProgress(iterator);

                while (timeActual <= maxTime && eventCalendar.Any<Event>() && condition())
                {
                    //refresh();

                    //Console.WriteLine("Vnutorny cyklus "+ iterator / numberOfReplications);

                    actualEvent = eventCalendar.First();
                    eventCalendar.RemoveAt(0);
                    timeActual = actualEvent.Time();
                    if (timeActual <= maxTime)
                    {
                        actualEvent.execute();
                    }
                }
                postSetup();
                iterator++;
                //Console.WriteLine("Replikacia #" + iterator);
            }
            worker.ReportProgress(100);
            isFinished = true;
            postPostSetup();
        }

        public virtual void postSetup()
        {

        }

        // vypis statistik na konci simulacie
        public virtual void postPostSetup()
        {

        }

        private void resetVariables()
        {
            eventCalendar = new List<Event>();
            timeActual = 0.0;
        }

        public void updateEventCalendar(Event evt)
        {
            // prida ho na koniec
            eventCalendar.Add(evt);
            // to do orderovanie
            eventCalendar.Sort((x, y) => x.Time().CompareTo(y.Time()));
        }

        public virtual void preSetup() {
            updateEventCalendar(init);
        }

        public virtual bool condition()
        {
            return true;
        }

    }
}
