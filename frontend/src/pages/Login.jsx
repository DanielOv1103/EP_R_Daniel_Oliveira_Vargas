// src/pages/Login.jsx
import React from "react";
import { useForm } from "react-hook-form";
import { toast } from "react-hot-toast";
import { useNavigate, Link } from "react-router-dom";
import { loginUser } from "@/api/auth";

import {
    Card, CardHeader, CardTitle,
    CardDescription, CardContent,
} from "@/components/ui/card";
import { Tabs, TabsList, TabsTrigger, TabsContent } from "@/components/ui/tabs";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { Separator } from "@/components/ui/separator"

export default function Login() {
    const { register, handleSubmit, formState: { isSubmitting } } = useForm({
        defaultValues: { role: "user", email: "", password: "" }
    });
    const navigate = useNavigate();

    const onSubmit = async ({ role, email, password }) => {
        try {
            // 1️⃣ Llamada
            const result = await loginUser({ email, password });
            console.log("🛠️ login response raw:", result);

            // 2️⃣ Extrae el token
            // Si tu API devuelve { token: "...", user: {...} }:
            let token = result.token;
            // Si por el contrario viene plano como { token, name, role }, entonces:
            if (!token && result.tokenValue) {
                token = result.tokenValue;
            }

            // 3️⃣ Extrae el usuario
            // Opción A: viene bajo result.user
            let user = result.user;
            // Opción B: viene plano, por ejemplo result.name y result.role
            if (!user && result.name) {
                user = {
                    name: result.name,
                    role: result.role,
                    // añade aquí otros campos si los devuelves
                };
            }

            // 4️⃣ Guarda en localStorage
            localStorage.setItem("token", token);
            localStorage.setItem("user", JSON.stringify(user));

            // 5️⃣ Notifica y redirige
            toast.success(`¡Bienvenido ${user.name || "!"}!`);
            if (user.role === 1) navigate("/admin/dashboard");
            else navigate("/dashboard");
        } catch (err) {
            console.error("Login error:", err);
            toast.error(err.message);
        }
    };


    return (
        <div className="min-h-screen flex items-center justify-center bg-gray-50 p-4">
            <Card className="w-full max-w-md">
                <CardHeader>
                    <CardTitle>Iniciar sesión</CardTitle>
                    <CardDescription>Selecciona tu rol</CardDescription>
                </CardHeader>
                <CardContent>
                    <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
                        <Tabs
                            value={register("role").value}
                            onValueChange={(v) => register("role").onChange({ target: { value: v } })}
                            defaultValue={"user"}
                        >
                            <TabsList>
                                <TabsTrigger value="user">Usuario</TabsTrigger>
                                <TabsTrigger value="admin">Admin</TabsTrigger>
                            </TabsList>

                            <TabsContent value="user">
                                <div className="space-y-4">
                                    <Input {...register("email", { required: true })} placeholder="Email de usuario" />
                                    <Input {...register("password", { required: true })} placeholder="Contraseña" type="password" />
                                </div>
                            </TabsContent>

                            <TabsContent value="admin">
                                <div className="space-y-4">
                                    <Input {...register("email", { required: true })} placeholder="Email de admin" />
                                    <Input {...register("password", { required: true })} placeholder="Contraseña" type="password" />
                                </div>
                            </TabsContent>
                        </Tabs>

                        <Button type="submit" className="w-full" disabled={isSubmitting}>
                            {isSubmitting ? "Entrando..." : "Entrar"}
                        </Button>

                        <p className="text-center text-sm">
                            ¿No tienes una cuenta?{" "}
                            <Link to="/register" className="text-blue-600 underline">
                                Regístrate
                            </Link>
                        </p>
                    </form>
                </CardContent>
            </Card>
        </div>
    );
}
