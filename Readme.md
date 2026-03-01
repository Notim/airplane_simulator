# ✈️ SkyBreaker

> Arcade flight simulator built in Unity 6. Fly fast, hit hard, don't crash.

[![Unity](https://img.shields.io/badge/Unity-6-black?logo=unity)](https://unity.com)
[![Language](https://img.shields.io/badge/C%23-blue?logo=csharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Status](https://img.shields.io/badge/Status-In%20Development-orange)]()

---

## 🎬 Gameplay

[![Watch Gameplay](https://img.youtube.com/vi/XLE7tlRK9DU/maxresdefault.jpg)](https://www.youtube.com/watch?v=XLE7tlRK9DU)

---

## 🕹️ Controls

| Key | Action |
|-----|--------|
| `SHIFT` | Increase throttle |
| `CTRL` | Decrease throttle |
| `S` | Pitch up — nose climbs |
| `W` | Pitch down — nose dives |
| `A` | Roll left |
| `D` | Roll right |
| `Q` | Yaw left |
| `E` | Yaw right |

---

## ⚙️ Features

- **Arcade Flight** — the plane always flies where the nose points, with velocity-based control authority
- **Crash & Respawn** — high-speed impacts trigger explosion FX and a configurable respawn
- **Terrain Flight** — Continuous Dynamic collision detection prevents phasing through mountains
- **Chase Camera** — follows position tightly with pitch lag for cinematic inertia
- **Propeller Spin** — RPM scales with real Rigidbody speed, pivot-corrected

---

## 📁 Scripts

| Script | Description |
|--------|-------------|
| `PlaneController.cs` | Main flight — throttle, velocity, lift, rotation |
| `PlaneCrash.cs` | Impact detection, explosion spawn, respawn |
| `ChaseCamera.cs` | Third-person follow cam with pitch lag |
| `PropellerSpin.cs` | RPM-based propeller driven by real speed |

---

## 🚀 Setup

1. Clone repository
2. open it on Unity 6
3. just start a preview

---

obs: this game was built as student project, you are free to clone or fork and improve more features!

*Built with Unity 6 · C#*