using System.Diagnostics;

namespace AdaTask
{
    public class ReservationRequest
    {
        public Train Train { get; set; }
        public int NumberOfPassengers { get; set; }
        public bool AllowMultipleWagons { get; set; }
    }

}
