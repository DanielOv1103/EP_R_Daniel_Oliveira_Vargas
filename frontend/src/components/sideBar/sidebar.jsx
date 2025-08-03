import React from 'react';
import { NavLink } from 'react-router-dom';
import { Button } from '@/components/ui/button';
import { Home, Users, Poll, Settings, LogOut } from 'lucide-react';

/**
 * Sidebar genérico que muestra enlaces según el rol ('user' o 'admin').
 */
export default function Sidebar({ role }) {
    const commonLinks = [
        { to: '/dashboard', label: 'Inicio', icon: Home },
    ];
    const userLinks = [
        ...commonLinks,
        { to: '/polls', label: 'Encuestas', icon: Poll },
    ];
    const adminLinks = [
        ...commonLinks,
        { to: '/admin/users', label: 'Usuarios', icon: Users },
        { to: '/admin/settings', label: 'Ajustes', icon: Settings },
    ];

    const links = role === 'admin' ? adminLinks : userLinks;

    return (
        <aside className="flex flex-col w-64 h-screen bg-white border-r">
            <div className="px-6 py-4 text-2xl font-bold">Mi App</div>
            <nav className="flex-1 px-2 space-y-1">
                {links.map(({ to, label, icon: Icon }) => (
                    <NavLink
                        key={to}
                        to={to}
                        className={({ isActive }) =>
                            `flex items-center px-4 py-2 rounded-md text-gray-700 hover:bg-gray-100 ${isActive ? 'bg-gray-200 font-semibold' : ''
                            }`
                        }
                    >
                        <Icon className="w-5 h-5 mr-3" />
                        {label}
                    </NavLink>
                ))}
            </nav>
            <div className="px-6 py-4">
                <Button
                    variant="outline"
                    className="w-full flex items-center justify-center"
                    onClick={() => {
                        // TODO: manejar logout
                    }}
                >
                    <LogOut className="w-4 h-4 mr-2" />
                    Salir
                </Button>
            </div>
        </aside>
    );
}
