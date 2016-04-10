using System;
using System.Collections.Generic;
using System.Linq;
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

        // vytvori statistiky, init. .. etc
        public virtual void prePreSetup()
        {

        }

        public override void backgroundProcess()
        {
            Event actualEvent;
            int iterator = 0;
            double progress = 0.0;
            int step = numberOfReplications / 100; // jedno percento replikacie 

            // vytvorenie aut
            prePreSetup();
            while (iterator < numberOfReplications)
            {
                resetVariables();
                preSetup();

                progress = ((double)iterator / (double)numberOfReplications) * 100;

                

                while (timeActual <= maxTime && eventCalendar.Any<Event>() && condition())
                {
                    actualEvent = eventCalendar.First();
                    eventCalendar.RemoveAt(0);
                    timeActual = actualEvent.Time();
                    if (timeActual <= maxTime)
                    {
                        actualEvent.execute();
                    }
                    worker.ReportProgress(Convert.ToInt32(progress));
                    System.Threading.Thread.Sleep(300);
                }
                
                postSetup();
                iterator++;
                //Console.WriteLine("Replikacia #" + iterator);
            }
            if(!condition())
            {
                worker.ReportProgress(0);
            } else
            {
                worker.ReportProgress(100);
            }
            
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

        public virtual void preSetup()
        {
            updateEventCalendar(init);
        }

        public virtual bool condition()
        {
            return true;
        }

    }
}
