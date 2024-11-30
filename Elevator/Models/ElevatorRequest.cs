namespace Elevator.Models
{
    public class ElevatorRequest
    {
        public int Id { get; set; }
        public int RequestedFloor { get; set; }
        public DateTime RequestTime { get; set; }
    }
}
