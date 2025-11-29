namespace LeaveApi.Dto
{
    public class LeaveRequestDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Reason { get; set; } = "";
        public string Status { get; set; } = "";
        public DateTime AppliedOn { get; set; }
        public string? AdminRemarks { get; set; }
    }

    public class CreateLeaveRequestDto
    {
        public int EmployeeId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Reason { get; set; } = "";
    }

    public class UpdateLeaveStatusDto
    {
        public string Status { get; set; } = "";
        public string? AdminRemarks { get; set; }
    }

}
