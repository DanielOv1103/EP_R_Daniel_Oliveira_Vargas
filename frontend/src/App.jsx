import React from "react";
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import { Toaster } from "react-hot-toast";
import "./index.css";

import Login from "@/pages/Login";
import Register from "@/pages/Register";
import AdminLayout from "@/components/layouts/admin";
import UserLayout from "./components/layouts/user";

import Dashboard from "@/pages/Dashboard";
import Polls from "@/pages/Polls";
import MyVotes from "@/pages/MyVotes";

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
    element: (
      <AdminLayout>
        <Dashboard />
      </AdminLayout>
    ),
    errorElement: <h1>Not Found 404</h1>
  },
  {
    path: "/dashboard",
    element: (
      <UserLayout>
        <Dashboard />
      </UserLayout>
    ),
    errorElement: <h1>Not Found 404</h1>
  },
  {
    path: "/polls",
    element: (
      <UserLayout>
        <Polls />
      </UserLayout>
    ),
    errorElement: <h1>Not Found 404</h1>
  },
  {
    path: "/my-votes",
    element: (
      <UserLayout>
        <MyVotes />
      </UserLayout>
    ),
    errorElement: <h1>Not Found 404</h1>
  }

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
