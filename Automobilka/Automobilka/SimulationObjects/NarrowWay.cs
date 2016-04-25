namespace Automobilka.SimulationObjects
{
    public class NarrowWay
    {
        private double _timeOfArrival;
        public NarrowWay()
        {
            _timeOfArrival = 0;
        }
        // najdlhsi cas prichodu aktualnych aut na ceste
        public double RealTime(double expectedTime)
        {
            if (expectedTime < _timeOfArrival)
            {
                return _timeOfArrival;
            }
            _timeOfArrival = expectedTime;
            return expectedTime;
        }
    }
}
