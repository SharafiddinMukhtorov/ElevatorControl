namespace Elevator.Models
{
    public class ElevatorState
    {
        public int Id { get; set; }
        public int CurrentFloor { get; set; }
        public string Direction { get; set; } = "Idle";
        public bool IsBusy { get; set; }
    }
}
