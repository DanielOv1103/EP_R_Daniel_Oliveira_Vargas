// src/pages/Polls.jsx
import React, { useEffect, useState } from "react";
import { toast } from "react-hot-toast";
import {
    Card, CardHeader, CardTitle, CardContent
} from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Dialog, DialogTrigger, DialogContent, DialogHeader, DialogTitle, DialogFooter } from "@/components/ui/dialog";
import { RadioGroup, RadioGroupItem } from "@/components/ui/radio-group";
import { getPolls, getOptions } from "@/api/polls";
import { vote } from "@/api/votes";

export default function Polls() {
    const [polls, setPolls] = useState([]);
    const [selectedPoll, setSelectedPoll] = useState(null);
    const [options, setOptions] = useState([]);
    const [choice, setChoice] = useState(null);
    const [loadingOptions, setLoadingOptions] = useState(false);
    const [voting, setVoting] = useState(false);

    useEffect(() => {
        getPolls()
            .then(setPolls)
            .catch(err => toast.error(err.message));
    }, []);

    useEffect(() => {
        if (!selectedPoll) return;
        setLoadingOptions(true);
        getOptions(selectedPoll.id)
            .then(setOptions)
            .catch(err => toast.error(err.message))
            .finally(() => setLoadingOptions(false));
    }, [selectedPoll]);

    const handleVote = async () => {
        if (!choice) return toast.error("Selecciona una opción");
        setVoting(true);
        try {
            await vote(selectedPoll.id, +choice);
            toast.success("Voto registrado");
            setSelectedPoll(null);
            setChoice(null);
        } catch (err) {
            toast.error(err.message);
        } finally {
            setVoting(false);
        }
    };

    return (
        <div className="p-6 space-y-4 bg-gray-50 min-h-screen">
            <h1 className="text-2xl font-bold">Encuestas</h1>
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
                {polls.map(poll => (
                    <Card key={poll.id}>
                        <CardHeader>
                            <CardTitle>{poll.title}</CardTitle>
                        </CardHeader>
                        <CardContent>
                            <p className="mb-4">{poll.description}</p>
                            <Dialog>
                                <DialogTrigger asChild>
                                    <Button
                                        onClick={() => setSelectedPoll(poll)}
                                        className="w-full"
                                    >
                                        Votar
                                    </Button>
                                </DialogTrigger>
                                <DialogContent>
                                    <DialogHeader>
                                        <DialogTitle>{poll.title}</DialogTitle>
                                    </DialogHeader>
                                    {loadingOptions
                                        ? <p>Cargando opciones…</p>
                                        : (
                                            <RadioGroup
                                                value={choice}
                                                onValueChange={setChoice}
                                                className="space-y-2 my-4"
                                            >
                                                {options.map(opt => (
                                                    <div key={opt.id} className="flex items-center">
                                                        <RadioGroupItem value={String(opt.id)} id={`opt-${opt.id}`} />
                                                        <label htmlFor={`opt-${opt.id}`} className="ml-2">
                                                            {opt.text}
                                                        </label>
                                                    </div>
                                                ))}
                                            </RadioGroup>
                                        )
                                    }
                                    <DialogFooter>
                                        <Button
                                            onClick={handleVote}
                                            disabled={voting || loadingOptions}
                                            className="w-full"
                                        >
                                            {voting ? "Votando…" : "Confirmar voto"}
                                        </Button>
                                    </DialogFooter>
                                </DialogContent>
                            </Dialog>
                        </CardContent>
                    </Card>
                ))}
            </div>
        </div>
    );
}
