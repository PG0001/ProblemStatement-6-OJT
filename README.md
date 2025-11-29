# Employee Leave & Attendance System

A full-stack web application to manage employee attendance, leaves, and admin approvals.  

**Backend:** ASP.NET Core Web API  
**Frontend:** React + Bootstrap  

---

## Features

### 1. Authentication
- JWT-based login.
- Roles: Admin and Employee.
- Seed users:

| Email             | Role  |
|------------------ |-------|
| admin@company.com | Admin |
| emp1@company.com  | Employee |

### 2. Employee Management
- Admin can view all employees.
- Employee can view own profile.

### 3. Attendance Module
- Employees can mark attendance once per day.
- Admin can view attendance of any employee.
- **API Endpoints:**
  - `POST /attendance/mark` - mark attendance
  - `GET /attendance/my` - employee view
  - `GET /attendance/employee/{id}` - admin view

### 4. Leave Management
- Employees can apply for leave.
- Admin can approve/reject leave requests.
- **Leave rules:**
  - Cannot apply for past dates.
  - Cannot apply overlapping leaves.
  - Admin must provide remarks while rejecting.
- **API Endpoints:**
  - `POST /leave/apply`
  - `GET /leave/my`
  - `GET /leave/all` (Admin only)
  - `PUT /leave/{id}/approve`
  - `PUT /leave/{id}/reject`

### 5. Frontend Pages
- Login Page: email & password login
- Dashboard: shows today's attendance, total leaves, pending requests
- Attendance Page: mark attendance, view history
- Apply Leave Page: apply for leaves with validation
- My Leaves Page: view leave history (badges: Pending/Approved/Rejected)
- Admin Panel: view employees, leave requests, mark holidays

---

## Technologies Used

**Backend:**
- ASP.NET Core Web API
- Entity Framework Core (Code-First)
- SQL Server
- JWT Authentication
- Repository Pattern

**Frontend:**
- React (Vite)
- Axios
- React Router
- Bootstrap 5
- Reusable components

---

## Setup Instructions

### Backend
1. Clone the repository.
2. Open solution in Visual Studio.
3. Update `appsettings.json` for SQL Server connection.
4. Run EF Core migrations:  
5. Seed default users (Admin/Employee).
6. Start the API:  
7. API runs at `https://localhost:5001/`

### Frontend
1. Navigate to frontend folder.
2. Install dependencies:  
3. Start development server:  
4. Open in browser at `http://localhost:5173/`

---

## Postman Collection
- Import `EmployeeLeave.postman_collection.json` to test API endpoints.

---

## Notes
- JWT token is stored in localStorage after login.
- Admin-only pages are protected by role-based routes.
- Attendance can be marked only once per day.
- Leave badges use colors for status:
- Pending - Blue
- Approved - Green
- Rejected - Red

---

