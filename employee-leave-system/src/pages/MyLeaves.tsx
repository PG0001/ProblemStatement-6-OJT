import Navbar from "../components/Navbar";
import { useEffect, useState } from "react";
import api from "../api/api";
import Badge from "../components/Badge";
import { getUser } from "../hooks/useAuth";

const MyLeaves = () => {
  const [leaves, setLeaves] = useState<any[]>([]);

  useEffect(() => { load(); }, []);

  const load = async () => {
    const u = getUser();
    if (!u) return;
    const res = await api.get("/leave/employee/" + u.id);
    setLeaves(res.data);
  };

  return (
    <div>
      <Navbar />
      <div className="container mt-4">
        <h2 className="mb-4">My Leaves</h2>
        <div className="table-responsive">
          <table className="table table-bordered table-hover">
            <thead className="table-light">
              <tr>
                <th>From</th>
                <th>To</th>
                <th>Reason</th>
                <th>Status</th>
                <th>Applied On</th>
              </tr>
            </thead>
            <tbody>
              {leaves.map((l: any) => (
                <tr key={l.id}>
                  <td>{new Date(l.fromDate).toLocaleDateString()}</td>
                  <td>{new Date(l.toDate).toLocaleDateString()}</td>
                  <td>{l.reason}</td>
                  <td><Badge status={l.status} /></td>
                  <td>{new Date(l.appliedOn).toLocaleDateString()}</td>
                </tr>
              ))}
            </tbody>
          </table>
          {leaves.length === 0 && <p className="text-center mt-3">No leaves found.</p>}
        </div>
      </div>
    </div>
  );
};

export default MyLeaves;
