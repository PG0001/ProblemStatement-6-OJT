export interface UserPayload {
id: number;
name: string;
email: string;
role: "Admin" | "Employee";
exp?: number;
}


export interface LeaveRequest {
id: number;
employeeId: number;
employeeName?: string;
fromDate: string;
toDate: string;
reason: string;
status: "Pending" | "Approved" | "Rejected";
appliedOn: string;
adminRemarks?: string;
}


export interface Attendance {
id: number;
employeeId: number;
date: string;
status: "Present" | "Absent";
}