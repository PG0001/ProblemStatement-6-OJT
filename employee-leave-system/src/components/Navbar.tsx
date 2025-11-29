import { Link } from "react-router-dom";
import { getUser, logout, isAdmin } from "../hooks/useAuth";

const Navbar = () => {
  const user = getUser();
  const employeeName = localStorage.getItem("employeeName") || user?.name || "User";

  return (
    <nav className="navbar navbar-expand-lg navbar-light bg-light px-3">
      <Link to="/" className="navbar-brand">LeaveApp</Link>
      <button className="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav">
        <span className="navbar-toggler-icon"></span>
      </button>

      <div className="collapse navbar-collapse" id="navbarNav">
        <ul className="navbar-nav me-auto">
          <li className="nav-item">
            <Link to="/" className="nav-link">Dashboard</Link>
          </li>
          <li className="nav-item">
            <Link to="/attendance" className="nav-link">Attendance</Link>
          </li>
          <li className="nav-item">
            <Link to="/apply-leave" className="nav-link">Apply Leave</Link>
          </li>
          <li className="nav-item">
            <Link to="/my-leaves" className="nav-link">My Leaves</Link>
          </li>
          {isAdmin() && (
            <>
              <li className="nav-item">
                <Link to="/admin/employees" className="nav-link">Employees</Link>
              </li>
              <li className="nav-item">
                <Link to="/admin/leaves" className="nav-link">Leave Requests</Link>
              </li>
            </>
          )}
        </ul>

        <div className="d-flex align-items-center">
          {user ? (
            <>
              <span className="me-2"><strong>{employeeName}</strong> ({user.role})</span>
              <button className="btn btn-outline-danger btn-sm" onClick={logout}>Logout</button>
            </>
          ) : (
            <Link to="/login" className="btn btn-outline-primary">Login</Link>
          )}
        </div>
      </div>
    </nav>
  );
};

export default Navbar;
