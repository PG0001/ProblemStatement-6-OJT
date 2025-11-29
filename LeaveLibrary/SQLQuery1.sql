CREATE DATABASE EmployeeLeaveAttendanceDB;
GO

USE EmployeeLeaveAttendanceDB;
GO
CREATE TABLE Employees (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(150) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(500) NOT NULL,
    Role NVARCHAR(20) NOT NULL  -- Admin / Employee
);
CREATE TABLE Attendance (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    EmployeeId INT NOT NULL,
    Date DATE NOT NULL,
    Status NVARCHAR(20) NOT NULL,   -- Present / Absent
    CONSTRAINT FK_Attendance_Employee 
        FOREIGN KEY (EmployeeId) REFERENCES Employees(Id),
    CONSTRAINT UC_Attendance UNIQUE (EmployeeId, Date)   -- Prevent double marking
);
CREATE TABLE LeaveRequests (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    EmployeeId INT NOT NULL,
    FromDate DATE NOT NULL,
    ToDate DATE NOT NULL,
    Reason NVARCHAR(500) NOT NULL,
    Status NVARCHAR(20) NOT NULL DEFAULT 'Pending',  -- Pending / Approved / Rejected
    AppliedOn DATETIME NOT NULL DEFAULT GETDATE(),
    AdminRemarks NVARCHAR(500) NULL,

    CONSTRAINT FK_Leave_Employee 
        FOREIGN KEY (EmployeeId) REFERENCES Employees(Id)
);
INSERT INTO Employees (Name, Email, PasswordHash, Role)
VALUES
('Admin User', 'admin@company.com', 'TEMP_ADMIN_PASSWORD', 'Admin'),
('Employee One', 'emp1@company.com', 'TEMP_EMP_PASSWORD', 'Employee');

select * from Employees;
Select * from Attendance;
Select * from LeaveRequests;
GO
--Drop table Attendance;
--Drop table LeaveRequests;
--Drop table Employees;