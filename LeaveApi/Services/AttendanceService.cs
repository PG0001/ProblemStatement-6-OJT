using LeaveApi.Dto;
using LeaveLibrary.Models;
using LeaveLibrary.Repository;

namespace LeaveApi.Services
{
    public class AttendanceService
    {
        private readonly AttendanceRepository _repo;

        public AttendanceService(AttendanceRepository repo)
        {
            _repo = repo;
        }

        public async Task<(bool Success, string Message)> Mark(MarkAttendanceDto dto)
        {
            if (await _repo.HasMarkedToday(dto.EmployeeId))
                return (false, "Attendance already marked for today");

            var attendance = new Attendance
            {
                EmployeeId = dto.EmployeeId,
                Status = dto.Status,
                Date = DateOnly.FromDateTime(DateTime.Today)
            };

            await _repo.Add(attendance);
            return (true, "Attendance marked successfully");
        }

        public async Task<List<AttendanceDto>> GetByEmployee(int employeeId)
        {
            var list = await _repo.GetByEmployee(employeeId);
            return list.Select(a => new AttendanceDto
            {
                Id = a.Id,
                EmployeeId = a.EmployeeId,
                Date = a.Date.ToDateTime(TimeOnly.MinValue),
                Status = a.Status
            }).ToList();
        }
    }
}
