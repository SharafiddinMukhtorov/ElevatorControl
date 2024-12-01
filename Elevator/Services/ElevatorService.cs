using ActualLab.Fusion;
using Elevator.Infrastructure;
using Elevator.Models;
using Microsoft.EntityFrameworkCore;

public class ElevatorService : IComputeService
{
    private readonly ElevatorControlDbContext _context;

    public ElevatorService(ElevatorControlDbContext context)
    {
        _context = context;
    }

    [ComputeMethod(AutoInvalidationDelay = 1)]
    public virtual async Task<ElevatorState?> GetCurrentElevatorStateAsync()
    {
        var elevatorState = _context.ElevatorStates.FirstOrDefaultAsync().Result;
        return elevatorState;
    }
        
    [ComputeMethod]
    public virtual async Task RequestElevatorAsync(int targetFloor)
    {
        var request = new ElevatorRequest
        {
            RequestedFloor = targetFloor,
            RequestTime = DateTime.UtcNow.AddHours(5),
        };
        _context.ElevatorRequests.Add(request);
        await _context.SaveChangesAsync();

        var elevatorState = await GetCurrentElevatorStateAsync();
        if (elevatorState != null)
        {
            elevatorState.Direction = elevatorState.CurrentFloor > targetFloor ? "Pastga" : "Tepaga";
            elevatorState.CurrentFloor = targetFloor;
            elevatorState.IsBusy = true;

            _context.ElevatorStates.Update(elevatorState);
            await _context.SaveChangesAsync();
        }

        elevatorState.IsBusy = false;

        _context.ElevatorStates.Update(elevatorState);
        await _context.SaveChangesAsync();
    }
    public async Task<List<ElevatorRequest>> GetElevatorRequestsAsync()
    {
        return await _context.ElevatorRequests.OrderByDescending(r => r.RequestTime).ToListAsync();
    }
}
