// src/pages/Register.jsx
import React from 'react';
import { useForm, Controller } from 'react-hook-form';
import { toast } from 'react-hot-toast';
import { Link, useNavigate } from 'react-router-dom';
import { Loader2 } from 'lucide-react';

import { registerUser } from '@/api/auth';

import {
    Card,
    CardHeader,
    CardTitle,
    CardDescription,
    CardContent,
} from '@/components/ui/card';
import { Label } from '@/components/ui/label';
import { Input } from '@/components/ui/input';
import { Button } from '@/components/ui/button';
import {
    Select,
    SelectTrigger,
    SelectValue,
    SelectContent,
    SelectItem,
} from '@/components/ui/select';

export default function Register() {
    const {
        register,
        control,
        handleSubmit,
        formState: { errors, isSubmitting },
    } = useForm({
        defaultValues: { role: '0' }
    });
    const navigate = useNavigate();

    const onSubmit = async (data) => {
        console.log('onSubmit data:', data);
        try {
            const { token, user } = await registerUser(data);
            localStorage.setItem('token', token);
            localStorage.setItem('user', JSON.stringify(user));
            toast.success('Registro exitoso');
            navigate('/login');
        } catch (err) {
            toast.error(err.message);
        }
    };

    return (
        <div className="min-h-screen flex items-center justify-center bg-gray-50 p-4">
            <Card className="w-full max-w-md">
                <CardHeader>
                    <CardTitle>Registro</CardTitle>
                    <CardDescription>
                        Completa los datos para crear tu cuenta
                    </CardDescription>
                </CardHeader>
                <CardContent>
                    <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
                        {/* Nombre */}
                        <div>
                            <Label htmlFor="name">Nombre</Label>
                            <Input
                                id="name"
                                placeholder="Tu nombre"
                                {...register('name', { required: 'El nombre es obligatorio' })}
                            />
                            {errors.name && (
                                <p className="text-sm text-red-600">{errors.name.message}</p>
                            )}
                        </div>

                        {/* Email */}
                        <div>
                            <Label htmlFor="email">Email</Label>
                            <Input
                                id="email"
                                type="email"
                                placeholder="usuario@ejemplo.com"
                                {...register('email', {
                                    required: 'El email es obligatorio',
                                    pattern: {
                                        value: /^[^@\s]+@[^@\s]+\.[^@\s]+$/,
                                        message: 'Formato de email inválido',
                                    },
                                })}
                            />
                            {errors.email && (
                                <p className="text-sm text-red-600">{errors.email.message}</p>
                            )}
                        </div>

                        {/* Contraseña */}
                        <div>
                            <Label htmlFor="password">Contraseña</Label>
                            <Input
                                id="password"
                                type="password"
                                placeholder="••••••••"
                                {...register('password', {
                                    required: 'La contraseña es obligatoria',
                                    minLength: {
                                        value: 6,
                                        message: 'Mínimo 6 caracteres',
                                    },
                                })}
                            />
                            {errors.password && (
                                <p className="text-sm text-red-600">
                                    {errors.password.message}
                                </p>
                            )}
                        </div>

                        {/* Rol */}
                        <div>
                            <Label htmlFor="role">Rol</Label>
                            <Controller
                                name="role"
                                control={control}
                                rules={{ required: 'Debes seleccionar un rol' }}
                                render={({ field }) => (
                                    <Select
                                        value={field.value}
                                        onValueChange={(val) => field.onChange(val)}
                                    >
                                        <SelectTrigger id="role">
                                            <SelectValue placeholder="Selecciona un rol" />
                                        </SelectTrigger>
                                        <SelectContent>
                                            <SelectItem value="0">Usuario</SelectItem>
                                            <SelectItem value="1">Admin</SelectItem>
                                        </SelectContent>
                                    </Select>
                                )}
                            />
                            {errors.role && (
                                <p className="text-sm text-red-600">{errors.role.message}</p>
                            )}
                        </div>

                        {/* Botón Enviar con loader */}
                        <Button
                            type="submit"
                            className="w-full flex items-center justify-center"
                            disabled={isSubmitting}
                        >
                            {isSubmitting && (
                                <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                            )}
                            {isSubmitting ? 'Enviando...' : 'Registrarme'}
                        </Button>

                        {/* Link a login */}
                        <p className="text-center text-sm">
                            ¿Ya tienes una cuenta?{' '}
                            <Link to="/login" className="text-blue-600 underline">
                                Inicia sesión
                            </Link>
                        </p>
                    </form>
                </CardContent>
            </Card>
        </div>
    );
}
