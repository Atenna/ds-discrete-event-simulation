using System;
using System.Collections.Generic;
using System.Linq;
using Automobilka.Responsivity;
using System.ComponentModel;
using Automobilka.Readonly;
using System.Threading;

namespace Automobilka.Simulations
{
    public class SimulationCoreAbstract : ResponsiveCore
    {
        private List<Event> _eventCalendar;

        public Event Init { set; get; }
        protected double TimeActual;
        protected double MaxTime;
        protected int Speed;
        protected int RepeateTime = 1;
        private int _iterator = 0;
        public int RetIterator
        {
            get
            {
                return _iterator;
            }
        }
        public int NumberOfEvents { get; set; }
        public bool IsVisualized { get; set; }
        public bool IsRefreshed { get; set; }

        private int NumberOfReplications { get; set; }
        public bool IsFinished { get; set; }

        public SimulationCoreAbstract(double maxTime, int numberOfReplications, BackgroundWorker worker) : base(worker)
        {
            this.TimeActual = 0.0;
            this.MaxTime = maxTime;
            this._eventCalendar = new List<Event>();
            this.NumberOfReplications = numberOfReplications;
            this.IsFinished = false;
            NumberOfEvents = 0;
        }

        // vytvori statistiky, init. .. etc
        public virtual void PrePreSetup()
        {

        }

        public override void BackgroundProcess()
        {
            Event actualEvent;
            _iterator = 0;
            double progress = 0.0;

            // vytvorenie aut
            PrePreSetup();
            while (_iterator < NumberOfReplications)
            {
                ResetVariables();
                PreSetup();

                progress = ((double)_iterator / (double)NumberOfReplications) * 100;

                while (TimeActual <= MaxTime && _eventCalendar.Any<Event>() && Condition())
                {
                    lock (Constants.Gate)
                    {
                        actualEvent = _eventCalendar.First();
                        _eventCalendar.RemoveAt(0);
                        TimeActual = actualEvent.Time();
                        if (TimeActual <= MaxTime)
                        {
                            actualEvent.Execute();
                        }
                    }

                    Constants.DoneEvent.WaitOne(Timeout.Infinite);
                    if (IsVisualized)
                    {
                        Worker.ReportProgress(Convert.ToInt32(progress));
                        if (!IsRefreshed)
                        {
                            IsRefreshed = true;
                            AddRefresh();
                        }
                    }
                }
                if (!IsVisualized)
                {
                    Worker.ReportProgress(Convert.ToInt32(progress));
                }
                PostSetup();
                _iterator++;
            }
            if (!Condition())
            {
                Worker.ReportProgress(0);
            }
            else
            {
                Worker.ReportProgress(100);
            }

            IsFinished = true;
            PostPostSetup();
        }

        public virtual void PostSetup()
        {

        }

        // vypis statistik na konci simulacie
        public virtual void PostPostSetup()
        {

        }

        public virtual void AddRefresh()
        {

        }

        private void ResetVariables()
        {
            _eventCalendar = new List<Event>();
            TimeActual = 0.0;
        }

        public void UpdateEventCalendar(Event evt)
        {
            _eventCalendar.Add(evt);
            List<Event> sortedList = new List<Event>();
            sortedList = _eventCalendar.OrderBy(x => x.TimeExecution).ThenBy(x => x.EventNumber).ToList();
            _eventCalendar = sortedList;
        }

        public virtual void PreSetup()
        {
            UpdateEventCalendar(Init);
        }

        public virtual bool Condition()
        {
            return true;
        }

        public void SetSpeed(int scrolledValue)
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
            this.Speed = scrolledValue;
            Console.WriteLine("speed: " + Speed);
        }

        public int GetSpeed()
        {
            return Speed;
        }

        public int GetRepeatTime()
        {
            return RepeateTime;
        }

        public double GetSimTime()
        {
            return TimeActual;
        }

        public int GetActualReplication()
        {
            return _iterator;
        }
    }
}
