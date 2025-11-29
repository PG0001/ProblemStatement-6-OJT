import Navbar from "../components/Navbar";
import { useEffect, useState } from "react";
import api from "../api/api";
import { getUser } from "../hooks/useAuth";
import Badge from "../components/Badge";

const Attendance = () => {
  const [myAttendance, setMyAttendance] = useState<any[]>([]);
  const [todayMarked, setTodayMarked] = useState(false);

  useEffect(() => { load(); }, []);

  const load = async () => {
    const u = getUser();
    if (!u) return;
    const res = await api.get("/attendance/employee/" + u.id);
    setMyAttendance(res.data);

    const today = new Date().toISOString().slice(0, 10);
    setTodayMarked(res.data.some((a: any) => a.date.startsWith(today)));
  };

  const mark = async () => {
    try {
      const u = getUser();
      if (!u) return;

      await api.post('/attendance/mark', { employeeId: u.id, status: "Present" });
      alert('Attendance marked successfully!');
      load();
    } catch (err: any) {
      alert(err?.response?.data?.message || 'Error marking attendance');
    }
  };

  return (
    <div>
      <Navbar />
      <div className="container mt-4">
        <h2>Attendance</h2>
        <div className="mb-3">
          <button className="btn btn-primary" onClick={mark} disabled={todayMarked}>
            Mark Attendance
          </button>
          {todayMarked && <span className="ms-3 text-success">Already marked today</span>}
        </div>

        <h3 className="mt-4">My Attendance</h3>
        <table className="table table-bordered">
          <thead className="table-light">
            <tr>
              <th>Date</th>
              <th>Status</th>
            </tr>
          </thead>
          <tbody>
            {myAttendance.map(a => (
              <tr key={a.id}>
                <td>{new Date(a.date).toLocaleDateString()}</td>
                <td><Badge status={a.status} /></td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default Attendance;
