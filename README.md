# ModularMonolith

A modular monolith ASP.NET Core application built with .NET 8, demonstrating clean architecture principles and modular design patterns.

## 🏗️ Architecture Overview

This project implements a modular monolith architecture that combines the benefits of microservices modularity with monolithic deployment simplicity. Each module is self-contained with its own domain logic, database context, and API endpoints.

### Key Features

- **Modular Design**: Each business domain is encapsulated in its own module
- **Clean Architecture**: Domain-driven design with clear separation of concerns
- **ASP.NET Core 8**: Latest .NET framework with minimal APIs
- **Entity Framework Core**: Database access with PostgreSQL support
- **JWT Authentication**: Secure token-based authentication
- **Global Error Handling**: Centralized exception management
- **Validation**: FluentValidation for request validation
- **Observability**: OpenTelemetry integration for monitoring
- **Database Migrations**: Per-module database migration support

## 🏛️ Project Structure

```
ModularMonolith/
├── ModularMonolith.Host/          # Main application host
│   ├── Program.cs                 # Application entry point
│   ├── Extensions/                # Host-specific extensions
│   ├── Seeding/                   # Database seeding services
│   └── appsettings.json          # Configuration files
├── Modules.Common/                # Shared infrastructure
│   ├── Abstractions/             # Common interfaces
│   ├── API/Controllers/          # Base controllers
│   ├── Configuration/            # Shared configuration
│   ├── Database/                 # Database infrastructure
│   ├── Domain/                   # Common domain types
│   ├── ErrorHandling/            # Global exception handling
│   ├── Extensions/               # Common extensions
│   └── Policies/                 # Authorization policies
└── Modules.Users/                # Users module
    ├── Authorization/            # User authorization logic
    ├── Database/                 # User-specific database context
    ├── Domain/                   # User domain entities
    ├── Features/Users/           # User API endpoints
    ├── Middlewares/              # Module-specific middleware
    └── Policies/                 # User-specific policies
```

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [PostgreSQL](https://www.postgresql.org/download/) (or update connection strings for your preferred database)
- [Git](https://git-scm.com/)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/tosinshada/ModularMonolith.git
   cd ModularMonolith
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Update database connection strings**
   Update the connection strings in `appsettings.json` and `appsettings.Development.json` to point to your database instance.

4. **Run database migrations**
   ```bash
   dotnet ef database update --project ModularMonolith.Host
   ```

5. **Build and run the application**
   ```bash
   dotnet run --project ModularMonolith.Host
   ```

The application will start and be available at `https://localhost:7xxx` (check console output for exact port).

## 📋 API Documentation

### Users Module

The Users module provides authentication and user management functionality:

#### Authentication Endpoints

- `POST /api/users/register` - Register a new user
- `POST /api/users/login` - User login
- `POST /api/users/refresh-token` - Refresh JWT token

#### User Management Endpoints

- `GET /api/users/profile` - Get current user profile
- `PUT /api/users/profile` - Update user profile
- `PUT /api/users/{id}/role` - Update user role (admin only)

### Request/Response Examples

**Register User:**
```json
POST /api/users/register
{
  "email": "user@example.com",
  "password": "SecurePassword123!",
  "firstName": "John",
  "lastName": "Doe"
}
```

**Login:**
```json
POST /api/users/login
{
  "email": "user@example.com",
  "password": "SecurePassword123!"
}
```

## 🔧 Configuration

### Database Configuration

Configure your database connection in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=ModularMonolith;Username=your_username;Password=your_password"
  }
}
```

### JWT Configuration

JWT settings can be configured in the `AuthConfiguration` section:

```json
{
  "AuthConfiguration": {
    "SecretKey": "your-secret-key-here",
    "Issuer": "ModularMonolith",
    "Audience": "ModularMonolith-Users",
    "ExpirationInMinutes": 60
  }
}
```

## 🏗️ Development

### Adding a New Module

1. Create a new class library project following the naming convention `Modules.{ModuleName}`
2. Reference `Modules.Common` for shared infrastructure
3. Implement the module registration pattern similar to `Modules.Users`
4. Add the module reference to `ModularMonolith.Host`
5. Register the module in `Program.cs`

### Module Structure Convention

Each module should follow this structure:
```
Modules.{ModuleName}/
├── AssemblyReference.cs
├── DependencyInjection.cs
├── {ModuleName}ModuleRegistration.cs
├── Database/
├── Domain/
├── Features/
└── Policies/
```

## 🧪 Testing

Run tests using the .NET CLI:

```bash
# Run all tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"
```

## 📦 Deployment

### Local Deployment

```bash
dotnet publish -c Release -o ./publish
cd publish
dotnet ModularMonolith.Host.dll
```

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🛠️ Built With

- [ASP.NET Core 8](https://docs.microsoft.com/en-us/aspnet/core/) - Web framework
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/) - Object-database mapper
- [PostgreSQL](https://www.postgresql.org/) - Database
- [FluentValidation](https://fluentvalidation.net/) - Validation library
- [JWT](https://jwt.io/) - JSON Web Tokens for authentication
- [OpenTelemetry](https://opentelemetry.io/) - Observability framework
- [Serilog](https://serilog.net/) - Logging library

## 📞 Support

If you have any questions or need help with setup, please open an issue on GitHub.

---

**Note**: This is a template project demonstrating modular monolith architecture patterns. Customize it according to your specific business requirements.
