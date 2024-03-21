namespace AdaTask
{
    public class ReservationResponse
    {
        public bool IsReservationPossible { get; set; }
        public List<SeatAssignment> SeatAssignments { get; set; }
    }
}
