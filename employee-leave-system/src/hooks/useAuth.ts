import {jwtDecode} from "jwt-decode";
import type { UserPayload } from "../types"; // ensure correct path

// Save JWT token and employee name to localStorage
export const saveToken = (token: string) => {
  localStorage.setItem("token", token);

  try {
    // Decode token to get user info
    const user = jwtDecode<UserPayload>(token);
    if (user?.name) {
      localStorage.setItem("employeeName", user.name);
    }
  } catch (e) {
    console.error("Failed to decode token", e);
  }
};

// Logout function
export const logout = () => {
  localStorage.removeItem("token");
  localStorage.removeItem("employeeName"); // remove name as well
  window.location.href = "/login";
};

// Get decoded user info from JWT
export const getUser = (): UserPayload | null => {
  const token = localStorage.getItem("token");
  if (!token) return null;

  try {
    return jwtDecode<UserPayload>(token);
  } catch (e) {
    console.error("Invalid token", e);
    return null;
  }
};

// Get employee name directly from localStorage
export const getEmployeeName = (): string | null => {
  return localStorage.getItem("employeeName");
};

// Check if user is authenticated
export const isAuthenticated = (): boolean => !!getUser();

// Check if user is admin
export const isAdmin = (): boolean => {
  const user = getUser();
  return user?.role === "Admin";
};
