using System;
using System.Collections.Generic;
using System.Linq;
using Automobilka.Responsivity;
using System.ComponentModel;
using Automobilka.Readonly;
using System.Threading;
using Automobilka.Events;

namespace Automobilka.Simulations
{
    public class SimulationCoreAbstract : ResponsiveCore
    {
        private List<Event> eventCalendar;

        public Event init { set; get; }
        protected double timeActual;
        protected double maxTime;
        protected int speed;
        protected int repeateTime = 1;
        private int iterator = 0;
        public int retIterator
        {
            get
            {
                return iterator;
            }
        }
        public int numberOfEvents { get; set; }
        public bool isVisualized { get; set; }
        public bool isRefreshed { get; set; }

        private int numberOfReplications { get; set; }
        public bool isFinished { get; set; }

        public SimulationCoreAbstract(double maxTime, int numberOfReplications, BackgroundWorker worker) : base(worker)
        {
            this.timeActual = 0.0;
            this.maxTime = maxTime;
            this.eventCalendar = new List<Event>();
            this.numberOfReplications = numberOfReplications;
            this.isFinished = false;
            numberOfEvents = 0;
        }

        // vytvori statistiky, init. .. etc
        public virtual void prePreSetup()
        {

        }

        public override void backgroundProcess()
        {
            Event actualEvent;
            iterator = 0;
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
                    lock (Constants.gate)
                    {
                        actualEvent = eventCalendar.First();
                        eventCalendar.RemoveAt(0);
                        timeActual = actualEvent.Time();
                        if (timeActual <= maxTime)
                        {
                            actualEvent.execute();
                        }
                    }

                    Constants.doneEvent.WaitOne(Timeout.Infinite);
                    if (isVisualized)
                    {
                        worker.ReportProgress(Convert.ToInt32(progress));
                        if (!isRefreshed)
                        {
                            isRefreshed = true;
                            addRefresh();
                        }
                    }
                }
                if (!isVisualized)
                {
                    worker.ReportProgress(Convert.ToInt32(progress));
                }
                postSetup();
                iterator++;
            }
            if (!condition())
            {
                worker.ReportProgress(0);
            }
            else
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

        public virtual void addRefresh()
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
            //eventCalendar.Sort((x, y) => x.Time().CompareTo(y.Time()));

            List<Event> sortedList = new List<Event>();
            sortedList = eventCalendar.OrderBy(x => x.timeExecution).ThenBy(x => x.eventNumber).ToList();

            eventCalendar = sortedList;

            // vlastna compare to funkcia, ktora to usortuje podla casu ale aj podla toho, kedy bola ktora aktivita vytvorena
            // dat do eventu integer a pri vytvarani eventov nech ma event integer priradeny z nejakeho countra
            // potom tu zavolat iba moju funkciu compare to event, event 

        }

        public virtual void preSetup()
        {
            updateEventCalendar(init);
        }

        public virtual bool condition()
        {
            return true;
        }

        public void setSpeed(int scrolledValue)
        {
            Console.WriteLine("scrolledValue: " + scrolledValue);
            /*if(scrolledValue != 10)
            {
                this.speed = (10 - scrolledValue) * 100;
            }
            else
            {
                this.speed = 10;
            }*/
            this.speed = scrolledValue;
            Console.WriteLine("speed: " + speed);
        }

        public int getSpeed()
        {
            return speed;
        }

        public int getRepeatTime()
        {
            return repeateTime;
        }

        public double getSimTime()
        {
            return timeActual;
        }

        public int getActualReplication()
        {
            return iterator;
        }
    }
}
