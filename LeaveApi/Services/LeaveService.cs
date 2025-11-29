using LeaveApi.Dto;
using LeaveLibrary.Models;
using LeaveLibrary.Repository;

namespace LeaveApi.Services
{
    public class LeaveService
    {
        private readonly LeaveRequestRepository _repo;
        private readonly EmployeeRepository _employeeRepo;

        public LeaveService(LeaveRequestRepository repo,EmployeeRepository repo1)
        {
            _repo = repo;
            _employeeRepo = repo1;
        }

        public async Task<(bool Success, string? Message)> Apply(CreateLeaveRequestDto dto)
        {
            // Optional: Only restrict extremely old leaves (e.g., more than 1 year ago)
            var oneYearAgo = DateTime.Today.AddYears(-1);
            if (dto.FromDate < oneYearAgo)
                return (false, "Cannot apply for leave older than 1 year");

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
        public async Task<(bool Success, string? Message)> UpdateStatus(int leaveId, UpdateLeaveStatusDto dto)
        {
            var leave = await _repo.GetById(leaveId);
            if (leave == null) return (false, "Leave not found");

            // Get current admin from dto (or pass separately)
            var adminId = dto.AdminId;

            // Prevent approving/rejecting own leave
            if (leave.EmployeeId == adminId)
                return (false, "You cannot approve or reject your own leave");

            // Update status
            leave.Status = dto.Status; // "Approved" or "Rejected"
            leave.AdminRemarks = dto.Remarks;
            await _repo.Update(leave);

            return (true, null);
        }
        public async Task<(bool Success, string? Message)> ApplyHolidayForAll(CreateLeaveRequestDto dto)
        {
            // Only admins should call this; backend/controller should enforce it via [Authorize(Roles="Admin")]

            // Validate dates
            if (dto.FromDate > dto.ToDate)
                return (false, "FromDate must be before ToDate");

            // Optional: prevent extremely old dates
            var oneYearAgo = DateTime.Today.AddYears(-1);
            if (dto.FromDate < oneYearAgo)
                return (false, "Cannot create a holiday older than 1 year");

            // Fetch all employees
            var employees = await _employeeRepo.GetAll(); // You might need to add this in EmployeeRepository

            foreach (var emp in employees)
            {
                // Optional: skip if the employee already has overlapping leave
                if (await _repo.HasOverlappingLeave(emp.Id,
                    DateOnly.FromDateTime(dto.FromDate),
                    DateOnly.FromDateTime(dto.ToDate)))
                {
                    continue; // skip this employee
                }

                var leave = new LeaveRequest
                {
                    EmployeeId = emp.Id,
                    FromDate = DateOnly.FromDateTime(dto.FromDate),
                    ToDate = DateOnly.FromDateTime(dto.ToDate),
                    Reason = dto.Reason, // e.g., "Company Holiday"
                    Status = "Approved", // auto-approved for all
                    AppliedOn = DateTime.Now,
                    AdminRemarks = "Holiday set by Admin"
                };

                await _repo.Add(leave);
            }

            return (true, "Holiday applied for all employees");
        }

    }
}
