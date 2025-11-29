using LeaveApi.Dto;
using LeaveLibrary.Models;
using LeaveLibrary.Repository;

namespace LeaveApi.Services
{
    public class LeaveService
    {
        private readonly LeaveRequestRepository _repo;

        public LeaveService(LeaveRequestRepository repo)
        {
            _repo = repo;
        }

        public async Task<(bool Success, string? Message)> Apply(CreateLeaveRequestDto dto)
        {
            if (dto.FromDate < DateTime.Today)
                return (false, "Cannot apply for past dates");

            if (await _repo.HasOverlappingLeave(dto.EmployeeId,
                DateOnly.FromDateTime(dto.FromDate),
                DateOnly.FromDateTime(dto.ToDate)))
                return (false, "Leave dates overlap with existing leave");

            var leave = new LeaveRequest
            {
                EmployeeId = dto.EmployeeId,
                FromDate = DateOnly.FromDateTime(dto.FromDate),
                ToDate = DateOnly.FromDateTime(dto.ToDate),
                Reason = dto.Reason,
                Status = "Pending",
                AppliedOn = DateTime.Now
            };

            await _repo.Add(leave);
            return (true, null);
        }

        public async Task<List<LeaveRequestDto>> GetByEmployee(int employeeId)
        {
            var list = await _repo.GetByEmployee(employeeId);
            return list.Select(l => new LeaveRequestDto
            {
                Id = l.Id,
                EmployeeId = l.EmployeeId,
                FromDate = l.FromDate.ToDateTime(TimeOnly.MinValue),
                ToDate = l.ToDate.ToDateTime(TimeOnly.MinValue),
                Reason = l.Reason,
                Status = l.Status,
                AppliedOn = l.AppliedOn,
                AdminRemarks = l.AdminRemarks
            }).ToList();
        }

        public async Task<List<LeaveRequestDto>> GetAll()
        {
            var list = await _repo.GetAll();
            return list.Select(l => new LeaveRequestDto
            {
                Id = l.Id,
                EmployeeId = l.EmployeeId,
                FromDate = l.FromDate.ToDateTime(TimeOnly.MinValue),
                ToDate = l.ToDate.ToDateTime(TimeOnly.MinValue),
                Reason = l.Reason,
                Status = l.Status,
                AppliedOn = l.AppliedOn,
                AdminRemarks = l.AdminRemarks
            }).ToList();
        }

        public async Task<(bool Success, string? Message)> UpdateStatus(int id, UpdateLeaveStatusDto dto)
        {
            var leave = await _repo.GetById(id);
            if (leave == null) return (false, "Leave not found");

            leave.Status = dto.Status;
            leave.AdminRemarks = dto.AdminRemarks;

            await _repo.Update(leave);
            return (true, null);
        }
    }
}
