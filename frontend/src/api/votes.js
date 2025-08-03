// src/api/votes.js
const BASE = "http://localhost:5027/api";


export async function vote(pollId, optionId) {
  const token = localStorage.getItem("token");
  if (!token) throw new Error("No autenticado");

  const res = await fetch(`${BASE}/polls/${pollId}/votes`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      "Authorization": `Bearer ${token}`  // <-- envías el JWT
    },
    body: JSON.stringify({ optionId })      // <-- solo optionId en el body
  });

  if (!res.ok) {
    const err = await res.json().catch(() => ({}));
    throw new Error(err.message || "Error al registrar voto");
  }
  return res.json();
}

export async function getMyVotes() {
  const token = localStorage.getItem("token");
  if (!token) throw new Error("No estás autenticado");

  const res = await fetch(`${BASE}/votes/me`, {
    headers: {
      "Authorization": `Bearer ${token}`
    }
  });
  if (!res.ok) {
    const error = await res.json().catch(() => ({}));
    throw new Error(error.message || "Error cargando tus votos");
  }
  return res.json(); // [{ voteId, pollId, pollTitle, optionId, optionText, votedAt }, ...]
}