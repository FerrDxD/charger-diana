# Contributing to Charger Diana

First off, thank you for considering contributing to Charger Diana!

## 🧠 Engineering Philosophy & Constraints

Charger Diana is built with strict performance constraints and architectural boundaries targeting low-end PCs (Intel Celeron, 8GB RAM). 

## 🚫 Out of Scope for 1.0 (Do Not Add)
To keep the project focused on its MVP, the following are strictly **forbidden** and will not be accepted:
- **Gamepad / Controller Support** (Mouse & Keyboard only).
- **Localization** (The game is strictly in Indonesian).
- **Other Platforms** (Windows only).
- **Music with Vocals** (Ambience and SFX only).
- **Additional Ghosts** (Only Kuntilanak is present).
- **Adding Humor** (Humor is strictly reserved for Ending B).

## 🛠 Development Workflow

### 1. Code Rules
- **No Magic Numbers**: All tuning values (speed, battery, etc.) must be exposed via `[Export]` or `Resource`.
- **Pure Logic First**: Logic that doesn't require Godot nodes should reside in `scripts/core/` as plain C# classes.
- **Performance First**: No allocations in `_Process` or `_PhysicsProcess` (avoid LINQ, string concatenation, or `new` keyword per frame).
- **EventBus**: Use the central `EventBus` for system communication, avoiding long `GetNode` paths.
- **Thread Safety**: No `Thread` or `async` tasks should touch the SceneTree without `CallDeferred`.

### 2. Pull Request Process
1. Make sure your code is tied to an existing issue or milestone.
2. Ensure `dotnet build` passes with zero warnings.
3. If adding external assets, you **must** document the source and license in `assets/ASSETS.md`.
4. Submit your PR using [Conventional Commits](https://www.conventionalcommits.org/) (e.g., `feat: ...`, `fix: ...`).

## 🛡 Code of Conduct
By participating, you agree to abide by our [Code of Conduct](CODE_OF_CONDUCT.md).

## 📩 Questions?
If you need guidance, contact the core team at **maulanaferdi0678@gmail.com**.
