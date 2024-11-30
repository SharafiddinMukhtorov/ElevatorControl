using Elevator.Models;
using Microsoft.EntityFrameworkCore;

namespace Elevator.Infrastructure
{
    public class ElevatorControlDbContext : DbContext
    {
        public ElevatorControlDbContext(DbContextOptions<ElevatorControlDbContext> options)
            : base(options)
        {
        }

        public DbSet<ElevatorState> ElevatorStates { get; set; }
        public DbSet<ElevatorRequest> ElevatorRequests { get; set; }
    }
}
