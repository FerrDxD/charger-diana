# Contributing to Charger Diana

First off, thank you for considering contributing to Charger Diana! It's people like you that make the open-source community such a fantastic place to learn, inspire, and create.

## 🧠 Engineering Philosophy

Charger Diana is built with strict performance constraints and architectural boundaries. Before you write any code, you **must** familiarize yourself with our core documentation:

1. **[ARCHITECT.md](ARCHITECT.md)**: Defines the boundaries between pure logic (`scripts/core/`) and engine nodes, autoload rules, and the `EventBus` communication pattern.
2. **[PRD_V2.md](PRD_V2.md)**: The single source of truth for the project's scope, requirements, and milestones.
3. **[DESIGN.md](DESIGN.md)**: Visual token constraints (6 colors, 1 accent, sharp corners) and UI standards.

## 🛠 Development Workflow

### 1. Branching Strategy
We use a standard feature-branch workflow.
- **Main Branch**: `main` (always stable, deployable)
- **Feature Branches**: `feat/your-feature-name`
- **Bugfix Branches**: `fix/your-bugfix-name`

### 2. Making Changes
- **Keep it focused**: One pull request per feature or bug fix.
- **No Magic Numbers**: All tuning values (speed, battery drain, etc.) must be exposed via `[Export]` or `Resource`.
- **Pure Logic First**: Logic that doesn't strictly require the Godot engine (e.g., calculations, save data models) should reside in `scripts/core/` as plain C# classes.
- **Zero Allocations in `_Process`**: We target low-end Intel Celeron devices. Avoid LINQ, string concatenation, or new object allocations inside frame-by-frame loops (`_Process`, `_PhysicsProcess`).
- **Clean Build**: Your code must compile without any new warnings. Run `dotnet build` locally before committing.

### 3. Commit Guidelines
We strictly follow [Conventional Commits](https://www.conventionalcommits.org/). This helps us automate changelogs and versioning.

Examples:
- `feat: add dynamic battery drain calculation`
- `fix: resolve null reference in SceneManager during fast transitions`
- `docs: update ASSETS.md with new ambient sound license`
- `refactor: move save schema serialization to pure C# layer`

## ✅ Pull Request Process

1. Ensure your PR is linked to an existing issue. If an issue doesn't exist, please create one first to discuss the proposed changes.
2. Update the `README.md` or other documentation files if your changes affect project setup or architecture.
3. If you add new assets (textures, sounds), you **must** log their sources and licenses in `assets/ASSETS.md`.
4. Ensure your PR passes all automated checks (CI).
5. A maintainer will review your code. We may request changes to ensure adherence to our performance and style guidelines.

## 🛡 Code of Conduct
By participating in this project, you agree to abide by our [Code of Conduct](CODE_OF_CONDUCT.md).

## 📩 Questions?
If you have any questions or need architectural guidance, feel free to open a Discussion or reach out to the core team at **maulanaferdi0678@gmail.com**.
