import React, { useEffect, useState } from "react";
import { getGlobalStats, getPollStats } from "@/api/admin";
import { Card, CardContent, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { LineChart, Line, XAxis, YAxis, Tooltip, ResponsiveContainer, BarChart, Bar, CartesianGrid } from "recharts";

export default function PollStatistics() {
    const [globalStats, setGlobalStats] = useState([]);
    const [selectedPoll, setSelectedPoll] = useState(null);
    const [pollDetails, setPollDetails] = useState(null);

    useEffect(() => {
        (async () => {
            const stats = await getGlobalStats();
            setGlobalStats(stats);
        })();
    }, []);

    const viewDetails = async (pollId) => {
        const details = await getPollStats(pollId);
        setPollDetails(details);
        setSelectedPoll(pollId);
    };

    return (
        <div className="p-6 space-y-6">
            {/* Global statistics with Line Chart */}
            <Card>
                <CardContent className="p-4">
                    <CardTitle>Votos Totales por Encuesta</CardTitle>
                    <ResponsiveContainer width="100%" height={250}>
                        <LineChart data={globalStats}>
                            <XAxis dataKey="title" interval={0} angle={-15} textAnchor="end" />
                            <YAxis allowDecimals={false} />
                            <Tooltip />
                            <Line type="monotone" dataKey="totalVotes" stroke="#8884d8" strokeWidth={2} />
                        </LineChart>
                    </ResponsiveContainer>
                    <div className="flex flex-wrap gap-2 mt-4">
                        {globalStats.map(p => (
                            <Button key={p.id} onClick={() => viewDetails(p.id)}>{p.title}</Button>
                        ))}
                    </div>
                </CardContent>
            </Card>

            {/* Individual poll details with Bar Chart */}
            {pollDetails && (
                <Card>
                    <CardContent className="p-4">
                        <CardTitle>Detalle: {pollDetails.title}</CardTitle>
                        <ResponsiveContainer width="100%" height={250}>
                            <BarChart data={pollDetails.options}>
                                <CartesianGrid strokeDasharray="3 3" />
                                <XAxis dataKey="text" interval={0} angle={-15} textAnchor="end" />
                                <YAxis allowDecimals={false} />
                                <Tooltip />
                                <Bar dataKey="votes" fill="#82ca9d" />
                            </BarChart>
                        </ResponsiveContainer>
                    </CardContent>
                </Card>
            )}
        </div>
    );
}
