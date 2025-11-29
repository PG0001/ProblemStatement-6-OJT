using LeaveApi.Dto;
using LeaveLibrary.Repository;
using LeaveLibrary.Models;

namespace LeaveApi.Services
{
    public class EmployeeService
    {
        private readonly EmployeeRepository _repo;
        private readonly AuthService _authService;

        public EmployeeService(EmployeeRepository repo, AuthService authService)
        {
            _repo = repo;
            _authService = authService;
        }

        public async Task<List<EmployeeDto>> GetAll()
        {
            var employees = await _repo.GetAll();
            return employees.Select(e => new EmployeeDto
            {
                Id = e.Id,
                Name = e.Name,
                Email = e.Email,
                Role = e.Role
            }).ToList();
        }

        public async Task<EmployeeDto?> GetById(int id)
        {
            var e = await _repo.GetById(id);
            if (e == null) return null;
            return new EmployeeDto
            {
                Id = e.Id,
                Name = e.Name,
                Email = e.Email,
                Role = e.Role
            };
        }

        public async Task<EmployeeDto> Create(CreateEmployeeDto dto)
        {
            var employee = new Employee
            {
                Name = dto.Name,
                Email = dto.Email,
                Role = dto.Role,
                PasswordHash = _authService.HashPassword(dto.Password) // hashed
            };

            await _repo.Add(employee);

            return new EmployeeDto
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Role = employee.Role
            };
        }

        public async Task<bool> Update(int id, UpdateEmployeeDto dto)
        {
            var emp = await _repo.GetById(id);
            if (emp == null) return false;

            if (!string.IsNullOrEmpty(dto.Name)) emp.Name = dto.Name;
            if (!string.IsNullOrEmpty(dto.Role)) emp.Role = dto.Role;


            await _repo.Update(emp);
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var emp = await _repo.GetById(id);
            if (emp == null) return false;

            await _repo.Delete(emp);
            return true;
        }
    }
}
