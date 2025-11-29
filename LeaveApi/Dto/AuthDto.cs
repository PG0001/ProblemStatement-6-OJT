namespace LeaveApi.Dto
{
    public class LoginDto
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
    }

    public class LoginResponseDto
    {
        public string Token { get; set; } = "";
        public string Role { get; set; } = "";
        public string EmployeeName { get; set; } = "";
    }

}
