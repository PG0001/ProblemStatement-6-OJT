import React from "react";
import { Navigate } from "react-router-dom";
import { isAuthenticated } from "../hooks/useAuth";

const ProtectedRoute = ({ children }: { children: React.ReactElement }) => {
  return isAuthenticated() ? children : <Navigate to="/login" />;
};

export default ProtectedRoute;
