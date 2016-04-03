using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Automobilka
{
    public class SimulationCore
    {
        private double timeActual;
        private LinkedList<Event> eventCalendar;

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

        public virtual void updateEventCalendar(Event evt)
        {
            eventCalendar.AddLast(evt);
        }
    }
}