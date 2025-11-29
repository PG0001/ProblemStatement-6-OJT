using System;
using System.Collections.Generic;

namespace LeaveApi.Models;

public partial class LeaveRequest
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public DateOnly FromDate { get; set; }

    public DateOnly ToDate { get; set; }

    public string Reason { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime AppliedOn { get; set; }

    public string? AdminRemarks { get; set; }

    public virtual Employee Employee { get; set; } = null!;
}
