# Contributing

Thanks for considering contributing to this project! To keep the repo healthy and contributors productive, please follow these guidelines:

- Fork the repo and create a feature branch (e.g., `feature/your-change`).
- Make small, focused commits. Use clear commit messages.
- Run formatting and basic checks locally before creating a PR.
  - `dotnet build` for backend
  - `npm install && npm run build` for frontend
- Open a PR against `main`. Describe your changes and include screenshots or logs if relevant.
- Add tests for new behavior where applicable.
- Respect the coding conventions in `.editorconfig`.

If your PR changes the public API or database schema, include migration steps and update `BackEnd/TiktokClone/README_IMPLEMENTATION.md`.
