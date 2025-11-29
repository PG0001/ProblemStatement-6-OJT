import Navbar from "../components/Navbar";
import { useState, useEffect } from "react";
import api from "../api/api";
import { getUser } from "../hooks/useAuth";

interface Attendance {
  date: string;
  status: string;
}

const ApplyLeave = () => {
  const [fromDate, setFromDate] = useState("");
  const [toDate, setToDate] = useState("");
  const [reason, setReason] = useState("");
  const [error, setError] = useState("");
  const [attendance, setAttendance] = useState<Attendance[]>([]);
  const [unmarkedDates, setUnmarkedDates] = useState<string[]>([]);

  useEffect(() => {
    loadAttendance();
  }, []);

  const loadAttendance = async () => {
    try {
      const u = getUser();
      if (!u) return;
      const res = await api.get("/attendance/employee/" + u.id);
      setAttendance(res.data);

      // Find past days that were not marked
      const today = new Date();
      const pastDates: string[] = [];
      res.data.forEach((a: Attendance) => {
        const date = new Date(a.date);
        if (date < today && a.status === "Absent") {
          pastDates.push(a.date.slice(0, 10));
        }
      });
      setUnmarkedDates(pastDates);
    } catch (err) {
      console.error("Failed to load attendance", err);
    }
  };

  const submit = async () => {
    setError("");

    if (!fromDate || !toDate || !reason) {
      return setError("All fields are required");
    }

    const from = new Date(fromDate);
    const to = new Date(toDate);

    if (from > to) return setError("From date must be before or equal to To date");

    try {
      await api.post("/leave/apply", { fromDate, toDate, reason });
      alert("Leave applied successfully");
      setFromDate("");
      setToDate("");
      setReason("");
      loadAttendance(); // refresh unmarked days
    } catch (err: any) {
      setError(err?.response?.data?.message || "Error applying leave");
    }
  };

  const selectUnmarkedDate = (date: string) => {
    if (!fromDate) {
      setFromDate(date);
      setToDate(date);
    } else if (!toDate) {
      if (new Date(date) < new Date(fromDate)) {
        setFromDate(date);
      } else {
        setToDate(date);
      }
    } else {
      setFromDate(date);
      setToDate(date);
    }
  };

  return (
    <div>
      <Navbar />
      <div className="container mt-4">
        <h2>Apply Leave</h2>

        <div className="mb-3">
          <label className="form-label">From</label>
          <input
            type="date"
            className="form-control"
            value={fromDate}
            onChange={(e) => setFromDate(e.target.value)}
          />
        </div>

        <div className="mb-3">
          <label className="form-label">To</label>
          <input
            type="date"
            className="form-control"
            value={toDate}
            onChange={(e) => setToDate(e.target.value)}
          />
        </div>

        <div className="mb-3">
          <label className="form-label">Reason</label>
          <textarea
            className="form-control"
            value={reason}
            onChange={(e) => setReason(e.target.value)}
          />
        </div>

        <button className="btn btn-primary" onClick={submit}>Apply</button>

        {error && <div className="text-danger mt-2">{error}</div>}

        {/* Unmarked attendance dates */}
        {unmarkedDates.length > 0 && (
          <div className="mt-4">
            <h5>Unmarked Days (click to select)</h5>
            <div className="d-flex flex-wrap gap-2 mt-2">
              {unmarkedDates.map((date) => (
                <button
                  key={date}
                  onClick={() => selectUnmarkedDate(date)}
                  className={`btn ${date >= fromDate && date <= toDate ? "btn-success" : "btn-outline-secondary"}`}
                >
                  {new Date(date).toLocaleDateString()}
                </button>
              ))}
            </div>
          </div>
        )}
      </div>
    </div>
  );
};

export default ApplyLeave;
