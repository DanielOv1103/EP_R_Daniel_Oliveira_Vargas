// src/api/auth.js
const API_BASE = "http://localhost:5027/api/Auth";

export async function registerUser({ name, email, password, role }) {
    const res = await fetch(`${API_BASE}/register`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ name, email, password, role: Number(role) }),
    });
    if (!res.ok) {
        const error = await res.json().catch(() => ({}));
        throw new Error(error.message || "Error registrando usuario");
    }
    return res.json(); // devuelve user + token (según tu API)
}

export async function loginUser({ email, password }) {
    const res = await fetch(`${API_BASE}/login`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email, password }),
    });
    if (!res.ok) {
        const error = await res.json().catch(() => ({}));
        throw new Error(error.message || "Error iniciando sesión");
    }
    return res.json(); // devuelve user, role, token
}
