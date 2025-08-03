// src/api/polls.js
const BASE = "http://localhost:5027/api";

export async function getPolls() {
  const res = await fetch(`${BASE}/Polls`);
  if (!res.ok) throw new Error("Error cargando encuestas");
  return res.json(); // array de polls
}

export async function getOptions(pollId) {
  const res = await fetch(`${BASE}/Polls/${pollId}/Options`);
  if (!res.ok) throw new Error("Error cargando opciones");
  return res.json(); // array de options
}