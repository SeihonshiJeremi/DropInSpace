// Compiles the core with Mono and runs it against a real OpenTrophy server (../../../OpenTrophy, built).
// Needs: mcs + mono, node, a Postgres with an empty database (DB_HOST, DB_PORT, DB_USER, DB_NAME).
import { spawn, execFileSync } from "node:child_process";
import { createHash } from "node:crypto";
import { fileURLToPath } from "node:url";
import path from "node:path";

const here = path.dirname(fileURLToPath(import.meta.url)), ot = process.env.OT_DIR || path.join(here, "../../../OpenTrophy");
const API = "http://127.0.0.1:18103/api/v1";
const srv = spawn("node", ["dist/server.js"], { cwd: ot, stdio: ["ignore", "ignore", "inherit"], env: { ...process.env, DATABASE_URL: "", PORT: "18103", LOG_LEVEL: "warn", REQUIRE_EMAIL: "false", REGISTER_POW_BITS: "4", REGISTRATIONS_PER_IP_PER_DAY: "20", PUBLIC_URL: "http://127.0.0.1:18103" } });
const call = async (method, p, body, token) => (await fetch(API + p, { method, headers: { ...(body ? { "content-type": "application/json" } : {}), ...(token ? { authorization: "Bearer " + token } : {}) }, body: body ? JSON.stringify(body) : undefined })).json();
const wait = (ms) => new Promise((r) => setTimeout(r, ms));
const register = async (username) => {
  const { challenge, bits } = await call("GET", "/auth/challenge");
  let n = 0;
  for (;; n++) { const d = createHash("sha256").update(`${challenge}.${n}`).digest(); let ok = true; for (let b = 0; b < bits; b++) if (d[b >> 3] & (0x80 >> (b & 7))) { ok = false; break; } if (ok) break; }
  await wait(1600);
  return (await call("POST", "/auth/register", { username, password: "motdepasse-long", challenge, nonce: String(n) })).token;
};
let code = 1;
try {
  for (let i = 0; i < 60; i++) { try { if ((await fetch(API + "/health")).ok) break; } catch {} await wait(200); }
  const dev = await register("studio"), player = await register("joueur");
  const g = await call("POST", "/games", { title: "Jeu Unity" }, dev);
  const add = (b) => call("POST", `/games/${g.id}/trophies`, b, dev);
  await add({ key: "first_step", name: "Premiers pas", description: "Finir le tutoriel", tier: "bronze", i18n: { en: { name: "First steps" }, "pt-BR": { name: "Primeiros passos", description: "Terminar o tutorial" } } });
  await add({ key: "boss", name: "Le gardien", tier: "gold" });
  await add({ key: "secret", name: "La vraie fin", tier: "silver", hidden: true });
  await add({ key: "hunter", name: "Chasseur", tier: "bronze", target: 100 });
  await call("PUT", `/games/${g.id}/platinum`, { name: "Maître" }, dev);
  const ext = await call("POST", `/games/${g.id}/groups`, { name: "Extension" , i18n: { en: { name: "Add-on" } } }, dev);
  await add({ key: "deep", name: "Au fond", tier: "silver", groupId: ext.id });
  await call("PATCH", `/games/${g.id}`, { published: true }, dev);
  const key = (await call("POST", `/games/${g.id}/keys`, {}, dev)).key;
  const exe = "/tmp/ot-core-test.exe";
  execFileSync("mcs", ["-langversion:7", "-r:System.Net.Http.dll", "-out:" + exe, path.join(here, "CoreTest.cs"), ...["MiniJson", "SecureStore", "Models", "TrophyClient"].map((f) => path.join(here, "../Runtime/Core", f + ".cs"))], { stdio: "inherit" });
  const run = spawn("mono", [exe], { stdio: "inherit", env: { ...process.env, OT_SERVER: "http://127.0.0.1:18103", OT_GAME_KEY: key, OT_PLAYER: player, OT_GAME_ID: g.id, http_proxy: "", https_proxy: "", HTTP_PROXY: "", HTTPS_PROXY: "", NO_PROXY: "127.0.0.1" } });
  code = await new Promise((r) => run.on("exit", r));
} catch (e) { console.error(e); }
srv.kill();
process.exit(code);
