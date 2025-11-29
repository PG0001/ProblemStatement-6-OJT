using LeaveLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace LeaveLibrary.Repository
{
    public class LeaveRequestRepository
    {
        private readonly DBContext _context;

        public LeaveRequestRepository(DBContext context)
        {
            _context = context;
        }

        public async Task<List<LeaveRequest>> GetByEmployee(int employeeId)
        {
            return await _context.LeaveRequests
                .Where(l => l.EmployeeId == employeeId)
                .OrderByDescending(l => l.AppliedOn)
                .ToListAsync();
        }

        public async Task<List<LeaveRequest>> GetAll()
        {
            return await _context.LeaveRequests
                .Include(l => l.Employee)
                .OrderByDescending(l => l.AppliedOn)
                .ToListAsync();
        }
        public async Task<LeaveRequest?> GetById(int id)
        {
            return await _context.LeaveRequests
                .Include(l => l.Employee)
                .FirstOrDefaultAsync(l => l.Id == id);
        }
        public async Task<bool> HasOverlappingLeave(int employeeId, DateOnly from, DateOnly to)
        {
            return await _context.LeaveRequests.AnyAsync(l =>
                l.EmployeeId == employeeId &&
                l.Status != "Rejected" &&
                l.FromDate <= to &&
                l.ToDate >= from
            );
        }

        public async Task Add(LeaveRequest leave)
        {
            _context.LeaveRequests.Add(leave);
            await _context.SaveChangesAsync();
        }

        public async Task Update(LeaveRequest leave)
        {
            _context.LeaveRequests.Update(leave);
            await _context.SaveChangesAsync();
        }
    }
}
