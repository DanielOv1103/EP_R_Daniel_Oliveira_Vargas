// src/api/votes.js
const BASE = "http://localhost:5027/api";

export async function vote(pollId, optionId) {
  const res = await fetch(`${BASE}/Votes`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ pollId, optionId }),
  });
  if (!res.ok) throw new Error("Error al registrar voto");
  return res.json(); // si tu API devuelve algo, p. ej. el voto creado
}
