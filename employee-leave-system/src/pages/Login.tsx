import { useState } from "react";
import api from "../api/api";
import { saveToken, getUser, isAuthenticated } from "../hooks/useAuth";

const Login = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");

  const handle = async (e: React.FormEvent) => {
    e.preventDefault();
    setError("");

    try {
      // Call login API
      const res = await api.post("/auth/login", { email, password });

      // Save JWT token
      saveToken(res.data.token);
      if (res.data.employeeName) {
        localStorage.setItem("employeeName", res.data.employeeName);
      }

      // Decode user info from token
      const user = getUser();
      console.log("Logged in user:", user);

      // Redirect to home page
      window.location.href = "/";
    } catch (err: any) {
      setError(err?.response?.data?.message || "Login failed");
    }
  };

  // If already logged in, redirect immediately
  if (isAuthenticated()) {
    window.location.href = "/";
    return null;
  }

  return (
    <div className="container mt-5" style={{ maxWidth: 400 }}>
      <h2 className="mb-4 text-center">Login</h2>
      <form onSubmit={handle}>
        <div className="mb-3">
          <input
            type="email"
            className="form-control"
            placeholder="Email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />
        </div>
        <div className="mb-3">
          <input
            type="password"
            className="form-control"
            placeholder="Password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
        </div>
        <div className="d-grid">
          <button type="submit" className="btn btn-primary">
            Login
          </button>
        </div>
        {error && <p className="text-danger mt-3">{error}</p>}
      </form>
    </div>
  );
};

export default Login;
