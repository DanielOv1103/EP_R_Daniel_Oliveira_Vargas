import React, { useState } from "react";
import { Link } from "react-router-dom";
import { useNavigate } from "react-router-dom";
import { loginUser } from "@/api/auth";

import {
    Card,
    CardHeader,
    CardTitle,
    CardDescription,
    CardContent,
} from "@/components/ui/card";
import {
    Tabs,
    TabsList,
    TabsTrigger,
    TabsContent,
} from "@/components/ui/tabs";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";

export default function Login() {
    const [role, setRole] = useState("user");

    const navigate = useNavigate();

    const onSubmit = async ({ email, password }) => {
        try {
            const { token, user } = await loginUser({ email, password });
            localStorage.setItem("token", token);
            localStorage.setItem("user", JSON.stringify(user));
            toast.success("¡Bienvenido " + user.name + "!");
            // redirige según el rol:
            if (user.role === 1) navigate("/admin/dashboard");
            else navigate("/dashboard");
        } catch (err) {
            toast.error(err.message);
        }
    };

    return (
        <div className="min-h-screen flex items-center justify-center bg-gray-50">
            <Card className="w-full max-w-md p-6">
                <CardHeader>
                    <CardTitle>Iniciar sesión</CardTitle>
                    <CardDescription>Selecciona tu rol</CardDescription>
                </CardHeader>
                <CardContent>
                    <Tabs value={role} onValueChange={setRole} className="mb-4">
                        <TabsList>
                            <TabsTrigger value="user">Usuario</TabsTrigger>
                            <TabsTrigger value="admin">Admin</TabsTrigger>
                        </TabsList>

                        <TabsContent value="user">
                            <form className="space-y-4">
                                <Input placeholder="Email de usuario" type="email" />
                                <Input placeholder="Contraseña" type="password" />
                                <Button className="w-full">Entrar como usuario</Button>
                            </form>
                            <p className="text-sm text-center mt-4">
                                ¿No tienes una cuenta?{' '}
                                <Link to="/register" className="text-blue-600 underline">
                                    Regístrate
                                </Link>
                            </p>
                        </TabsContent>

                        <TabsContent value="admin">
                            <form className="space-y-4">
                                <Input placeholder="Email de admin" type="email" />
                                <Input placeholder="Contraseña" type="password" />
                                <Button variant="secondary" className="w-full" onClick={onSubmit}>
                                    Entrar como admin
                                </Button>
                            </form>
                            <p className="text-sm text-center mt-4">
                                ¿No tienes una cuenta?{' '}
                                <Link to="/register" className="text-blue-600 underline">
                                    Regístrate
                                </Link>
                            </p>
                        </TabsContent>
                    </Tabs>
                </CardContent>
            </Card>
        </div>
    );
}
