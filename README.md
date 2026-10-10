# Charger Diana

[![Build Status](https://img.shields.io/badge/build-passing-success?style=for-the-badge)](https://github.com/FerrDxD/charger-diana)
[![Godot Engine](https://img.shields.io/badge/Godot_4.x-.NET_C%23-blue?style=for-the-badge&logo=godotengine)](https://godotengine.org)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=for-the-badge)](https://opensource.org/licenses/MIT)

**Charger Diana** is a lightweight, first-person horror game developed with Godot 4.x (C# / .NET). Designed with a strict focus on performance, the project boasts an optimized architecture targeting low-end hardware (e.g., Intel Celeron, 8GB RAM, iGPU) while delivering a chilling, PS1-era atmospheric experience.

## ✨ Key Features
- **Deterministic Core Logic:** Separation of pure game logic (C# models) from Godot's engine layer, allowing unit testing without launching the editor.
- **Event-Driven Architecture:** Decoupled systems utilizing a centralized `EventBus` for seamless communication across state machines and gameplay loops.
- **Dynamic Resource Scaling:** Built-in `GraphicsPresets` engine intelligently scales 3D rendering (`Scaling3D`) and toggles real-time shadow casting to sustain 30+ FPS on base hardware.
- **Audio Ducking & Subtitle Sync:** Automated DSP integration via `AudioManager` for voice-over ducking and dynamic subtitle rendering.

## 🚀 Getting Started

### Prerequisites
- **Godot Engine 4.x** (Mono / .NET version)
- **.NET 8.0 SDK** (or newer)

### Build & Run
1. Clone the repository:
   ```bash
   git clone https://github.com/FerrDxD/charger-diana.git
   ```
2. Navigate to the project directory and build the C# solution:
   ```bash
   cd charger-diana
   dotnet build
   ```
3. Open `project.godot` in the Godot Editor or run directly via the command line:
   ```bash
   godot --path .
   ```

## 📖 Documentation
Detailed architectural guidelines and engineering rules can be found in our core documentation:
- [Architecture Manifesto (ARCHITECT.md)](ARCHITECT.md)
- [Product Requirements (PRD_V2.md)](PRD_V2.md)
- [Design Tokens (DESIGN.md)](DESIGN.md)

## 🤝 Contributing
We welcome contributions! Please review our [Contributing Guidelines](CONTRIBUTING.md) and [Code of Conduct](CODE_OF_CONDUCT.md) before submitting a Pull Request.

## 🛡️ Security
For vulnerability disclosures, please refer to our [Security Policy](SECURITY.md).

## 📄 License
This project is licensed under the MIT License - see the LICENSE file for details.
All external visual and audio assets are attributed in `assets/ASSETS.md`.

---
*Maintained by [@FerrDxD](https://github.com/FerrDxD)*
