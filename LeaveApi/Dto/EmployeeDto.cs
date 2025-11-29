namespace LeaveApi.Dto
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Role { get; set; } = "";
    }

    public class CreateEmployeeDto
    {
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string Role { get; set; } = "";
    }

    public class UpdateEmployeeDto
    {
        public string? Name { get; set; }
        public string? Role { get; set; }
    }

}
