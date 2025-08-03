import React from 'react';
import { NavLink, useNavigate } from 'react-router-dom'
import { Button } from '@/components/ui/button';
import { Home, Users, PieChart, Settings, LogOut, History } from 'lucide-react';
import { logout } from "@/api/auth";


/**
 * Sidebar genérico que muestra enlaces según el rol ('user' o 'admin').
 */

export default function Sidebar({ role }) {
    const navigate = useNavigate();
    
    const commonLinks = [
        { to: '/dashboard', label: 'Inicio', icon: Home },
    ];
    const userLinks = [
        ...commonLinks,
        { to: '/polls', label: 'Encuestas', icon: PieChart },
        { to: '/my-votes', label: 'Mis Votos', icon: History },
    ];
    const adminLinks = [
        ...commonLinks,
        { to: '/admin/polls', label: 'Encuestas', icon: PieChart },
        { to: '/admin/votes', label: 'Votos', icon: History },
        // { to: '/admin/settings', label: 'Ajustes', icon: Settings },
        // { to: '/admin/users', label: 'Usuarios', icon: Users },
    ];

    const handleLogout = () => {
        logout();              // 1. limpia storage
        navigate("/login");    // 2. redirige al login
    };

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
                    onClick={handleLogout}
                >
                    <LogOut className="w-4 h-4 mr-2" />
                    Salir
                </Button>
            </div>
        </aside>
    );
}
