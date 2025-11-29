import Navbar from "../components/Navbar";
import { useEffect, useState } from "react";
import api from "../api/api";
import Badge from "../components/Badge";
import { getUser, isAdmin } from "../hooks/useAuth";

interface LeaveRequest {
  id: number;
  employeeId: number;
  employeeName?: string;
  fromDate: string;
  toDate: string;
  reason: string;
  status: string;
  appliedOn: string;
  adminRemarks: string | null;
}

interface Employee {
  id: number;
  name: string;
}

const LeaveRequests = () => {
  const [requests, setRequests] = useState<LeaveRequest[]>([]);
  const [remarks, setRemarks] = useState<{ [key: number]: string }>({});
  const [loadingId, setLoadingId] = useState<number | null>(null);

  const currentUser = getUser();
  const currentAdminId = currentUser?.id;

  const load = async () => {
    try {
      const res = await api.get("/leave/all");
      const leaves: LeaveRequest[] = res.data;

      const empRes = await api.get("/employee");
      const employees: Employee[] = empRes.data;

      const employeeMap: Record<number, string> = {};
      employees.forEach(e => {
        employeeMap[e.id] = e.name;
      });

      const leavesWithNames = leaves.map(l => ({
        ...l,
        employeeName: employeeMap[l.employeeId] || "Unknown"
      }));

      setRequests(leavesWithNames);
    } catch (err) {
      console.error("Failed to load leave requests or employees", err);
    }
  };

  const updateStatus = async (id: number, status: "Approved" | "Rejected") => {
    if (status === "Rejected" && !remarks[id]) return alert("Please provide remarks for rejection");

    setLoadingId(id);
    try {
      await api.put(`/leave/${id}/status`, {
        AdminId: currentAdminId,
        Status: status,
        Remarks: remarks[id] || null,
      });
      setRemarks(prev => ({ ...prev, [id]: "" }));
      load();
    } catch (err: any) {
      alert(err?.response?.data?.message || "Failed to update status");
    } finally {
      setLoadingId(null);
    }
  };

  useEffect(() => {
    load();
  }, []);

  return (
    <div>
      <Navbar />
      <div className="container mt-4">
        <h2>All Leave Requests</h2>
        <table className="table table-bordered table-striped mt-3">
          <thead className="table-light">
            <tr>
              <th>ID</th>
              <th>Employee</th>
              <th>From</th>
              <th>To</th>
              <th>Reason</th>
              <th>Status</th>
              <th>Applied On</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {requests.map(r => {
              const isSelfRequest = r.employeeId === currentAdminId;
              const canAct = isAdmin() && !isSelfRequest;

              return (
                <tr key={r.id}>
                  <td>{r.id}</td>
                  <td>{r.employeeName}</td>
                  <td>{new Date(r.fromDate).toLocaleDateString()}</td>
                  <td>{new Date(r.toDate).toLocaleDateString()}</td>
                  <td>{r.reason}</td>
                  <td><Badge status={r.status} /></td>
                  <td>{new Date(r.appliedOn).toLocaleDateString()}</td>
                  <td>
                    {r.status === "Pending" && !isSelfRequest && canAct ? (
                      <div className="d-flex flex-column gap-1">
                        <button
                          className="btn btn-success btn-sm"
                          onClick={() => updateStatus(r.id, "Approved")}
                          disabled={loadingId === r.id}
                        >
                          Approve
                        </button>
                        <input
                          type="text"
                          className="form-control form-control-sm"
                          placeholder="Admin remarks"
                          value={remarks[r.id] || ""}
                          onChange={e =>
                            setRemarks(prev => ({ ...prev, [r.id]: e.target.value }))
                          }
                        />
                        <button
                          className="btn btn-danger btn-sm"
                          onClick={() => updateStatus(r.id, "Rejected")}
                          disabled={loadingId === r.id}
                        >
                          Reject
                        </button>
                      </div>
                    ) : r.status === "Pending" && isSelfRequest ? (
                      <span className="text-muted">Self-request (cannot act)</span>
                    ) : (
                      r.adminRemarks || "-"
                    )}
                  </td>
                </tr>
              );
            })}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default LeaveRequests;
