namespace LeaveApi.Dto
{
    public class AttendanceDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; } = "";
    }

    public class MarkAttendanceDto
    {
        public int EmployeeId { get; set; }
        public string Status { get; set; } = "";
    }

}
