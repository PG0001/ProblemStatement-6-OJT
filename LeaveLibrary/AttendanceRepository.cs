using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LeaveLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace LeaveLibrary.Repository
{
    public class AttendanceRepository
    {
        private readonly DBContext _context;

        public AttendanceRepository(DBContext context)
        {
            _context = context;
        }

        public async Task<bool> HasMarkedToday(int employeeId)
        {
            return await _context.Attendances
                .AnyAsync(a => a.EmployeeId == employeeId &&
                               a.Date == DateOnly.FromDateTime(DateTime.Now));
        }

        public async Task<List<Attendance>> GetByEmployee(int employeeId)
        {
            return await _context.Attendances
                .Where(a => a.EmployeeId == employeeId)
                .OrderByDescending(a => a.Date)
                .ToListAsync();
        }

        public async Task Add(Attendance attendance)
        {
            _context.Attendances.Add(attendance);
            await _context.SaveChangesAsync();
        }
    }
}
