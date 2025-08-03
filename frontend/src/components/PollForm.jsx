// src/components/PollForm.jsx
import React, { useEffect, useState } from "react";
import { useForm, useFieldArray } from "react-hook-form";
import { toast } from "react-hot-toast";
import {
    getPoll,
    getOptions,
    createPoll,
    updatePoll,
    createOption,
    deleteOption,
    updateOption
} from "@/api/admin";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Button } from "@/components/ui/button";
import { Switch } from "@/components/ui/switch";

export default function PollForm({ pollId, onSuccess, onCancel }) {
    const isEdit = !!pollId;
    const [loading, setLoading] = useState(isEdit);
    const [deletedOptionIds, setDeletedOptionIds] = useState([]);
    const [originalOptions, setOriginalOptions] = useState([]); // guardar originales

    const {
        register,
        control,
        handleSubmit,
        reset,
        formState: { isSubmitting }
    } = useForm({
        defaultValues: {
            title: "",
            description: "",
            isActive: true,
            options: [{ text: "" }, { text: "" }]
        }
    });

    const { fields, append, remove } = useFieldArray({
        control,
        name: "options",
        keyName: "fieldId"
    });

    useEffect(() => {
        if (!isEdit) return;
        (async () => {
            try {
                const p = await getPoll(pollId);
                const opts = await getOptions(pollId);
                reset({
                    title: p.title,
                    description: p.description,
                    isActive: p.isActive,
                    options: opts.map(o => ({ id: o.id, text: o.text }))
                });
                setOriginalOptions(opts); // guarda original para comparar texto
                setDeletedOptionIds([]);
            } catch (err) {
                toast.error(err.message);
            } finally {
                setLoading(false);
            }
        })();
    }, [isEdit, pollId, reset]);

    const onSubmit = async data => {
        try {
            const result = isEdit
                ? await updatePoll(pollId, {
                    title: data.title,
                    description: data.description,
                    isActive: data.isActive
                })
                : await createPoll({
                    title: data.title,
                    description: data.description
                });

            const id = isEdit ? pollId : result.id;

            for (const optionId of deletedOptionIds) {
                await deleteOption(id, optionId);
            }

            for (const o of data.options) {
                if (!o.text.trim()) continue;
                if (!o.id) {
                    await createOption(id, o.text); // nueva opción
                } else {
                    const original = originalOptions.find(op => op.id === o.id);
                    if (original && original.text !== o.text) {
                        await updateOption(id, o.id, o.text); // solo si cambió
                    }
                }
            }

            toast.success(isEdit ? "Encuesta actualizada" : "Encuesta creada");
            onSuccess();
        } catch (err) {
            toast.error(err.message);
        }
    };

    if (loading) {
        return <p className="p-6 text-center">Cargando formulario…</p>;
    }

    return (
        <div className="px-4 py-4">
            <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
                <div>
                    <Label>Título</Label>
                    <Input {...register("title", { required: true })} />
                </div>
                <div>
                    <Label>Descripción</Label>
                    <Input {...register("description", { required: true })} />
                </div>
                <div className="flex items-center space-x-2">
                    <Switch {...register("isActive")} />
                    <span>Activa</span>
                </div>
                <div>
                    <Label>Opciones</Label>
                    <div className="space-y-2">
                        {fields.map((f, i) => (
                            <div key={f.fieldId} className="flex space-x-2">
                                <input type="hidden" {...register(`options.${i}.id`)} />
                                <Input
                                    {...register(`options.${i}.text`)}
                                    placeholder={`Opción #${i + 1}`}
                                />
                                <Button
                                    variant="destructive"
                                    type="button"
                                    onClick={() => {
                                        const optionId = fields[i]?.id;
                                        if (optionId) {
                                            setDeletedOptionIds(prev => [...prev, optionId]);
                                        }
                                        remove(i);
                                    }}
                                    disabled={fields.length <= 1}
                                >
                                    Eliminar
                                </Button>
                            </div>
                        ))}
                    </div>
                    <Button type="button" variant="outline" onClick={() => append({ text: "" })}>
                        + Agregar opción
                    </Button>
                </div>
                <div className="flex justify-end space-x-2">
                    <Button variant="outline" onClick={onCancel}>
                        Cancelar
                    </Button>
                    <Button type="submit" disabled={isSubmitting}>
                        {isSubmitting ? (isEdit ? "Guardando..." : "Creando...") : (isEdit ? "Guardar" : "Crear")}
                    </Button>
                </div>
            </form>
        </div>
    );
}
