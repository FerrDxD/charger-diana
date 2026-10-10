# Charger Diana

[![Build Status](https://img.shields.io/badge/build-passing-success?style=for-the-badge)](https://github.com/FerrDxD/charger-diana)
[![Godot Engine](https://img.shields.io/badge/Godot_4.x-.NET_C%23-blue?style=for-the-badge&logo=godotengine)](https://godotengine.org)
[![Platform](https://img.shields.io/badge/Platform-Windows-lightgrey?style=for-the-badge)](#)


**Charger Diana** is a short (25-35 minutes) first-person 3D horror game developed with Godot 4.x (C# / .NET). Set in a night school, you play as Diana, a Gen Z student trapped and desperately looking for her phone charger while avoiding the student council patrol and a lurking *Kuntilanak*.

> **Note:** The game is exclusively in **Indonesian language** (Text and Voice Over). It is designed for **Windows PC** and only supports **Mouse & Keyboard** (no gamepad support).

## ✨ Key Features
- **PS1-Era Aesthetics:** Retro visuals with nearest-neighbor filtering, optional dither, and grain effects.
- **Extreme Optimization:** Built targeting low-end hardware (Intel Celeron, 8GB RAM, iGPU). Targets a stable 30+ FPS on Low presets via Godot's Compatibility renderer.
- **Pure Logic Architecture:** Separation of pure game logic (C# models in `scripts/core/`) from Godot's engine layer, ensuring zero per-frame allocations.
- **Event-Driven Communication:** Decoupled systems utilizing a centralized `EventBus`.

## 🚀 Getting Started

### Prerequisites
- **Godot Engine 4.x** (Mono / .NET version)
- **.NET 8.0 SDK** (or newer)
- Windows OS

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

## 🤝 Contributing
We welcome contributions! Please review our [Contributing Guidelines](CONTRIBUTING.md) and [Code of Conduct](CODE_OF_CONDUCT.md) before submitting a Pull Request. Note that we have strict guidelines on what is out of scope for version 1.0 (e.g., no other languages, no gamepad support, no vocal music).

## 🛡️ Security
For vulnerability disclosures, please refer to our [Security Policy](SECURITY.md).

## 📄 License
This project is licensed under the MIT License - see the LICENSE file for details.
All external visual and audio assets are attributed in `assets/ASSETS.md`.

---
*Maintained by [@FerrDxD](https://github.com/FerrDxD) | Contact: maulanaferdi0678@gmail.com*
