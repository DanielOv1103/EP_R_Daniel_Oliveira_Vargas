// src/pages/MyVotes.jsx
import React, { useEffect, useState } from "react";
import { toast } from "react-hot-toast";
import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card";
import { getMyVotes } from "@/api/votes";

export default function MyVotes() {
    const [votes, setVotes] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        getMyVotes()
            .then(data => setVotes(data))
            .catch(err => toast.error(err.message))
            .finally(() => setLoading(false));
    }, []);

    if (loading) {
        return <p className="p-6 text-center">Cargando tus votos…</p>;
    }

    return (
        <div className="p-6 bg-gray-50 min-h-screen">
            <h1 className="text-2xl font-semibold mb-4">Mis Votaciones</h1>
            {votes.length === 0 ? (
                <p className="text-center text-gray-500">Aún no has votado en ninguna encuesta.</p>
            ) : (
                <div className="space-y-4">
                    {votes.map(v => (
                        <Card key={v.voteId} className="hover:shadow-md transition-shadow">
                            <CardHeader>
                                <CardTitle>{v.pollTitle}</CardTitle>
                            </CardHeader>
                            <CardContent className="flex justify-between">
                                <span className="text-gray-700">{v.optionText}</span>
                                <span className="text-sm text-gray-500">{new Date(v.votedAt).toLocaleString()}</span>
                            </CardContent>
                        </Card>
                    ))}
                </div>
            )}
        </div>
    );
}
