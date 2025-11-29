using LeaveApi.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LeaveApi.Services;

namespace LeaveAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Require authentication for all actions
    public class LeaveController : ControllerBase
    {
        private readonly LeaveService _service;

        public LeaveController(LeaveService service)
        {
            _service = service;
        }

        // POST: api/leave/apply
        // Employees can apply for leave
        [HttpPost("apply")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Apply([FromBody] CreateLeaveRequestDto dto)
        {
            var currentUserId = int.Parse(User.FindFirst("id")?.Value ?? "0");
            dto.EmployeeId = currentUserId; // force EmployeeId from token
            var result = await _service.Apply(dto);
            if (!result.Success) return BadRequest(new { message = result.Message });

            return Ok(new { message = "Leave applied successfully" });
        }

        // GET: api/leave/employee/{employeeId}
        // Employees can view only their own leaves
        [HttpGet("employee/{employeeId}")]
        public async Task<IActionResult> GetByEmployee(int employeeId)
        {
            var currentUserId = int.Parse(User.FindFirst("id")?.Value ?? "0");
            var currentUserRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            if (currentUserRole != "Admin" && currentUserId != employeeId)
                return Forbid(); // Employees cannot view other employees' leaves

            var list = await _service.GetByEmployee(employeeId);
            return Ok(list);
        }

        // GET: api/leave/all
        // Admin can view all leaves
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var list = await _service.GetAll();
            return Ok(list);
        }

        // PUT: api/leave/{id}/status
        // Admin can approve/reject leave
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateLeaveStatusDto dto)
        {
            var result = await _service.UpdateStatus(id, dto);
            if (!result.Success) return BadRequest(new { message = result.Message });

            return Ok(new { message = "Leave status updated successfully" });
        }
    }
}
