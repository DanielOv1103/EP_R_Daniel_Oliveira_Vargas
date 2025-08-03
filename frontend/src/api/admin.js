// src/api/admin.js
const BASE = "http://localhost:5027/api";

async function handleRes(res) {
    if (!res.ok) {
        // intenta leer JSON, si no funciona lee texto
        let err
        try { err = (await res.json()).message }
        catch { err = await res.text() }
        throw new Error(err || `${res.status} ${res.statusText}`);
    }
    // intenta devolver JSON, o undefined si no hay cuerpo
    return res.status !== 204 ? res.json() : undefined;
}

export async function getAllPolls() {
    const token = localStorage.getItem("token");
    const res = await fetch(`${BASE}/Polls`, {
        headers: { Authorization: `Bearer ${token}` }
    });
    return handleRes(res);
}

export async function getPoll(id) {
    const token = localStorage.getItem("token");
    const res = await fetch(`${BASE}/Polls/${id}`, {
        headers: { Authorization: `Bearer ${token}` }
    });
    if (!res.ok) throw new Error(await res.text());
    return res.json();
}

export async function getOptions(pollId) {
    const token = localStorage.getItem("token");
    const res = await fetch(`${BASE}/Polls/${pollId}/Options`, {
        headers: { Authorization: `Bearer ${token}` }
    });
    if (!res.ok) throw new Error(await res.text());
    return res.json();
}


export async function createPoll({ title, description }) {
    const token = localStorage.getItem("token");
    const res = await fetch(`${BASE}/Polls`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`
        },
        body: JSON.stringify({ title, description })
    });
    return handleRes(res);
}

export async function updatePoll(id, { title, description, isActive }) {
    const token = localStorage.getItem("token");
    const res = await fetch(`${BASE}/Polls/${id}`, {
        method: "PATCH",
        headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`
        },
        body: JSON.stringify({ title, description, isActive })
    });
    return handleRes(res);
}

export async function createOption(pollId, text) {
    const token = localStorage.getItem("token");
    const res = await fetch(`${BASE}/Polls/${pollId}/Options`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`
        },
        body: JSON.stringify({ text })
    });
    return handleRes(res);
}

export async function deletePoll(id) {
    const token = localStorage.getItem("token");
    const res = await fetch(`${BASE}/Polls/${id}`, {
        method: "DELETE",
        headers: { Authorization: `Bearer ${token}` }
    });
    return handleRes(res);
}

export async function deleteOption(pollId, optionId) {
    const token = localStorage.getItem("token");
    const res = await fetch(`${BASE}/Polls/${pollId}/Options/${optionId}`, {
        method: "DELETE",
        headers: { Authorization: `Bearer ${token}` }
    });
    return handleRes(res);
}

export async function updateOption(pollId, optionId, text) {
    const token = localStorage.getItem("token");
    const res = await fetch(`${BASE}/Polls/${pollId}/Options/${optionId}`, {
        method: "PATCH",
        headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`
        },
        body: JSON.stringify({ text })
    });
    return handleRes(res);
}

export async function getGlobalStats() {
    const token = localStorage.getItem("token");
    const res = await fetch(`${BASE}/votes/results/global`, {
        headers: { Authorization: `Bearer ${token}` }
    });
    return handleRes(res);
}

export async function getPollStats(id) {
    const token = localStorage.getItem("token");
    const res = await fetch(`${BASE}/votes/results/${id}`, {
        headers: { Authorization: `Bearer ${token}` }
    });
    return handleRes(res);
}


