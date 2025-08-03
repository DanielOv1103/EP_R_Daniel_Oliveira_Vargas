// src/components/AdminLayout.jsx
import React from 'react';
import Sidebar from '@/components/sideBar/sidebar';

export default function UserLayout({ children }) {
  return (
    <div className="flex">
      <Sidebar role="user" />
      <main className="flex-1 p-6 bg-gray-100">{children}</main>
    </div>
  );
}
