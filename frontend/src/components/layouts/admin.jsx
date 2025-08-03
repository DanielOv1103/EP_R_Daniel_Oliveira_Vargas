// src/components/AdminLayout.jsx
import React from 'react';
import Sidebar from '@/components/Sidebar';

export default function AdminLayout({ children }) {
    return (
        <div className="flex">
            <Sidebar role="admin" />
            <main className="flex-1 p-6 bg-gray-100">{children}</main>
        </div>
    );
}
