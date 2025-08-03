// src/pages/AdminPollsPage.jsx
import React, { useEffect, useState } from "react";
import { toast } from "react-hot-toast";
import { getAllPolls, deletePoll } from "@/api/admin";
import PollForm from "@/components/PollForm";
import {
    Card,
    CardHeader,
    CardTitle,
    CardContent
} from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import {
    Dialog,
    DialogTrigger,
    DialogContent,
    DialogHeader,
    DialogTitle,
    DialogClose
} from "@/components/ui/dialog";

export default function AdminPollsPage() {
    const [polls, setPolls] = useState([]);
    const [mode, setMode] = useState(null); // null | {type:'new'} | {type:'edit', pollId}

    // 1️⃣ Función para recargar encuestas
    const reload = () =>
        getAllPolls()
            .then(setPolls)
            .catch((e) => toast.error(e.message));

    // 2️⃣ Solo al montar, sin async en la firma
    useEffect(() => {
        reload();
    }, []);

    const handleDelete = async (id) => {
        if (!confirm("¿Borrar esta encuesta?")) return;
        try {
            await deletePoll(id);
            toast.success("Encuesta borrada");
            reload();
        } catch (e) {
            toast.error(e.message);
        }
    };

    return (
        <div className="p-6 bg-gray-50 min-h-screen space-y-6">
            <div className="flex justify-between items-center">
                <h1 className="text-2xl">Administrar Encuestas</h1>

                {/* Nuevo Poll */}
                <Dialog
                    open={mode?.type === "new"}
                    onOpenChange={(open) => {
                        if (!open) setMode(null);
                    }}
                >
                    <DialogTrigger asChild>
                        <Button onClick={() => setMode({ type: "new" })}>+ Nueva</Button>
                    </DialogTrigger>
                    <DialogContent className="sm:max-w-lg">
                        <DialogHeader>
                            <DialogTitle>Crear Encuesta</DialogTitle>
                            <DialogClose asChild>
                                <Button variant="ghost" className="absolute top-4 right-4">
                                    ✕
                                </Button>
                            </DialogClose>
                        </DialogHeader>

                        <PollForm
                            pollId={undefined}
                            onSuccess={() => {
                                setMode(null);
                                reload();
                            }}
                            onCancel={() => setMode(null)}
                        />
                    </DialogContent>
                </Dialog>
            </div>

            {/* Lista de encuestas */}
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
                {polls.map((p) => (
                    <Card key={p.id}>
                        <CardHeader>
                            <CardTitle>{p.title}</CardTitle>
                        </CardHeader>
                        <CardContent className="flex justify-between items-center">
                            <span>{p.description}</span>
                            <div className="space-x-2">
                                {/* Editar Poll */}
                                <Dialog
                                    open={mode?.type === "edit" && mode.pollId === p.id}
                                    onOpenChange={(open) => {
                                        if (!open) setMode(null);
                                    }}
                                >
                                    <DialogTrigger asChild>
                                        <Button
                                            size="sm"
                                            onClick={() => setMode({ type: "edit", pollId: p.id })}
                                        >
                                            Editar
                                        </Button>
                                    </DialogTrigger>
                                    <DialogContent className="sm:max-w-lg">
                                        <DialogHeader>
                                            <DialogTitle>Editar Encuesta</DialogTitle>
                                            {/* <DialogClose asChild>
                                                <Button variant="ghost" className="absolute top-4 right-4">
                                                    ✕
                                                </Button>
                                            </DialogClose> */}
                                        </DialogHeader>

                                        <PollForm
                                            pollId={p.id}
                                            onSuccess={() => {
                                                setMode(null);
                                                reload();
                                            }}
                                            onCancel={() => setMode(null)}
                                        />
                                    </DialogContent>
                                </Dialog>

                                {/* Borrar Poll */}
                                <Button
                                    size="sm"
                                    variant="destructive"
                                    onClick={() => handleDelete(p.id)}
                                >
                                    Borrar
                                </Button>
                            </div>
                        </CardContent>
                    </Card>
                ))}
            </div>
        </div>
    );
}
