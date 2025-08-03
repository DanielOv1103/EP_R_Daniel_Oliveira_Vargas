import React from "react";
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import { Toaster } from "react-hot-toast";
import "./index.css"; 

import Login from "@/pages/Login";
import Register from "@/pages/Register";
import Dashboard from "@/pages/Dashboard";

const router = createBrowserRouter([
  {
    path: "/",
    element: <Login />,
  },
  {
    path: "/login",
    element: <Login />,
    errorElement: <h1>Not Found 404</h1>
  },
  {
    path: "/register",
    element: <Register />,
    errorElement: <h1>Not Found 404</h1>
  },
  {
    path: "/admin/dashboard",
    element: <Dashboard />,
    errorElement: <h1>Not Found 404</h1>
  },
]);


export default function App() {
  return (
    <>
      <RouterProvider router={router} />
      <Toaster
        position="top-right"
        reverseOrder={false}
      />
    </>
  );
}
