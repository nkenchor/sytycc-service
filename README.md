
# SYTYCC Service (C# / .NET 7)

> A modular, scalable backend system for managing users, facilitators, and participants in the **SYTYCC ecosystem**. Built in modern **.NET 7 (C#)** using clean architecture principles, this service supports organizational workflows, seeding, and templated views for customer-facing operations.

---

## 🧭 Project Purpose

The **SYTYCC Service** is designed to power a dynamic organizational and learning platform. It handles:
- User management (admins, facilitators, participants)
- Seeding and onboarding workflows
- Support for customizable front-end templates
- Extensible infrastructure for additional domain modules

It’s ideal for high-growth environments like **training organizations**, **mentorship platforms**, or **certification bodies** needing modularity, clarity, and performance.

---

## 🏗️ Architecture Overview

This project follows a layered, clean architecture approach:

```text
Solution: Sytycc-Service.sln
│
├── Sytycc-Service.Infrastructure/
│   ├── AppUser/
│   │   ├── Repository/
│   │   └── Seed/
│   ├── AppFacilitator/
│   │   ├── Repository/
│   │   └── Seed/
│   ├── AppParticipant/
│   │   └── Repository/
│   └── Program.cs
│
├── template/
│   ├── customertemplate.html
│   └── admintemplate.html
│
├── sytycc-service.env
└── appsettings.json
```

---

## 💡 Features

| Feature                      | Description                                      |
|------------------------------|--------------------------------------------------|
| **Modular Infrastructure**   | Repositories and seeders scoped per domain       |
| **Template-Driven**          | HTML templates for client and admin onboarding   |
| **Clean C# Structure**       | Follows best practices for layered .NET design   |
| **Seed-Ready Architecture**  | Supports seeding logic for development/demo data |
| **Environment Configurable** | Uses `.env` and `appsettings.json`              |
| **Scalable Project Layout**  | Organized for future services or layers         |

---

## ⚙️ Getting Started

### Prerequisites

- [.NET 7 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)

### 1. Clone the Repository

```bash
git clone https://github.com/your-username/sytycc-service.git
cd sytycc-service
```

### 2. Set Up Environment

Edit or duplicate the provided `.env` file:

```env
ASPNETCORE_ENVIRONMENT=Development
DATABASE_CONNECTION_STRING=your-db-connection
JWT_SECRET=your-secret
```

### 3. Run the Application

```bash
dotnet build Sytycc-Service.sln
dotnet run --project Sytycc-Service.Infrastructure
```

> The server will start on the configured port (usually 5000 or 7070)

---

## ✅ Use Cases

| Domain         | Supported Operations            |
|----------------|---------------------------------|
| Users          | Create, seed, update, list      |
| Facilitators   | Onboard, assign, seed           |
| Participants   | Register, list, manage          |

Templates (`template/`) provide ready-to-use front-end views that can be served or extended by a hosting layer.

---

## 🔒 Security & Configuration

- JWT authentication planned for future integration
- Secure config via `sytycc-service.env` + `appsettings.json`
- Ready for Docker or Kubernetes deployment

---

## 🧪 Testing & CI

This project is CI-ready. To run tests (when added):

```bash
dotnet test
```

To integrate CI/CD, connect with GitHub Actions or GitLab pipelines using standard `.NET Core` build templates.

---

## 📎 Future Roadmap

- Add Swagger/OpenAPI documentation
- Integrate Identity Provider (OIDC / JWT)
- Add Unit & Integration tests
- Modularize Business Logic layer
- Containerize with Docker
- Enable role-based authorization

---

## 👨‍💻 Author & Maintainer

**Nkenchor Osemeke**  
📧 Email: nkenchor@osemeke.com  
🌍 GitHub: [github.com/nkenchor](https://github.com/nkenchor)

---

## 📝 License

MIT License — See `LICENSE` for full terms.