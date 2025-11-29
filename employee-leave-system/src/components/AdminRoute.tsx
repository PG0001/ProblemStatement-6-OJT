import React from "react";
import { Navigate } from "react-router-dom";
import { isAuthenticated, isAdmin } from "../hooks/useAuth";

const AdminRoute = ({ children }: { children: React.ReactElement }) => {
  if (!isAuthenticated()) return <Navigate to="/login" />;
  if (!isAdmin()) return <Navigate to="/" />;
  return children;
};

export default AdminRoute;
