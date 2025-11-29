import Navbar from "../components/Navbar";
import { useEffect, useState } from "react";
import api from "../api/api";
import { getUser, isAdmin } from "../hooks/useAuth";

const Dashboard = () => {
  const [todayStatus, setTodayStatus] = useState<string | null>(null);
  const [totalLeaves, setTotalLeaves] = useState(0);
  const [pendingLeaves, setPendingLeaves] = useState(0);

  // For creating holiday
  const [holidayFrom, setHolidayFrom] = useState("");
  const [holidayTo, setHolidayTo] = useState("");
  const [holidayReason, setHolidayReason] = useState("");

  useEffect(() => {
    load();
  }, []);

  const load = async () => {
    try {
      const u = getUser();
      if (!u) return;

      // Attendance
      const myAttendance = await api.get("/attendance/employee/" + u.id);
      const todayLocal = new Date();
      const found = myAttendance.data.find((a: any) => {
        const d = new Date(a.date);
        return (
          d.getFullYear() === todayLocal.getFullYear() &&
          d.getMonth() === todayLocal.getMonth() &&
          d.getDate() === todayLocal.getDate()
        );
      });
      setTodayStatus(found ? found.status : "Not marked");

      // Leaves
      const myLeaves = await api.get("/leave/employee/" + u.id);
      setTotalLeaves(myLeaves.data.filter((l: any) => l.status === "Approved").length);
      setPendingLeaves(myLeaves.data.filter((l: any) => l.status === "Pending").length);
    } catch (e) {
      console.error(e);
    }
  };

  // Admin: set holiday for all employees
  const createHoliday = async () => {
    if (!holidayFrom || !holidayTo || !holidayReason) {
      return alert("All fields required");
    }
    if (new Date(holidayFrom) > new Date(holidayTo)) {
      return alert("From date must be before To date");
    }
    const u = getUser();
    if (!u) return;

    try {
      await api.post("/leave/holiday", {
        employeeId: u.id,
        fromDate: holidayFrom,
        toDate: holidayTo,
        reason: holidayReason
      });
      alert("Holiday applied for all employees");
      setHolidayFrom("");
      setHolidayTo("");
      setHolidayReason("");
      load();
    } catch (err: any) {
      alert(err?.response?.data?.message || "Failed to create holiday");
    }
  };

  return (
    <div>
      <Navbar />
      <div className="container mt-4">
        <h2>Dashboard</h2>
        <div className="mb-3">
          <p>Today's attendance: <strong>{todayStatus}</strong></p>
          <p>Total leaves taken: <strong>{totalLeaves}</strong></p>
          <p>Pending leave requests: <strong>{pendingLeaves}</strong></p>
        </div>

        {isAdmin() && (
          <div className="card p-3 mt-4">
            <h3>Set Holiday for All Employees</h3>
            <div className="mb-2">
              <label className="form-label">From:</label>
              <input className="form-control" type="date" value={holidayFrom} onChange={e => setHolidayFrom(e.target.value)} />
            </div>
            <div className="mb-2">
              <label className="form-label">To:</label>
              <input className="form-control" type="date" value={holidayTo} onChange={e => setHolidayTo(e.target.value)} />
            </div>
            <div className="mb-2">
              <label className="form-label">Reason:</label>
              <input className="form-control" type="text" value={holidayReason} onChange={e => setHolidayReason(e.target.value)} />
            </div>
            <button className="btn btn-success mt-2" onClick={createHoliday}>Apply Holiday</button>
          </div>
        )}
      </div>
    </div>
  );
};

export default Dashboard;
