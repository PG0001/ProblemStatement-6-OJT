using LeaveApi.Dto;
using LeaveApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeaveAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Require authentication for all endpoints
    public class AttendanceController : ControllerBase
    {
        private readonly AttendanceService _service;

        public AttendanceController(AttendanceService service)
        {
            _service = service;
        }

        // POST: api/attendance/mark
        [HttpPost("mark")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Mark([FromBody] MarkAttendanceDto dto)
        {
            var currentUserId = int.Parse(User.FindFirst("id")?.Value ?? "0");
            if (currentUserId != dto.EmployeeId)
                return Forbid(); // Employees can mark only their own attendance

            var result = await _service.Mark(dto);
            if (!result.Success) return BadRequest(new { message = result.Message });

            return Ok(new { message = result.Message });
        }

        // GET: api/attendance/employee/{employeeId}
        [HttpGet("employee/{employeeId}")]
        public async Task<IActionResult> GetByEmployee(int employeeId)
        {
            var currentUserId = int.Parse(User.FindFirst("id")?.Value ?? "0");
            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            if (role != "Admin" && currentUserId != employeeId)
                return Forbid(); // Employees can view only their own attendance

            var list = await _service.GetByEmployee(employeeId);
            return Ok(list);
        }
    }
}
