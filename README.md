!!! THERE IS A POSTMAN COLLECTION IN THE REPOSITORY !!!

# Diet Management System

A comprehensive .NET 8 API for managing diet plans, clients, and dietitians with JWT authentication and PostgreSQL database.

## 🏗️ Architecture

This project follows Clean Architecture principles with the following layers:

- **API Layer** (`DietManagementSystem.API`) - Controllers and API endpoints
- **Application Layer** (`DietManagementSystem.Application`) - Business logic, CQRS with MediatR
- **Domain Layer** (`DietManagementSystem.Domain`) - Entities and domain logic
- **Infrastructure Layer** (`DietManagementSystem.Infrastructure`) - External services and repositories
- **Persistence Layer** (`DietManagementSystem.Persistence`) - Database context and configurations

## 🚀 Quick Start

### Prerequisites

- .NET 8 SDK
- Docker and Docker Compose
- PostgreSQL database (cloud-hosted)

### Local Development Setup

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd DietManagementSystem
   ```

2. **Start the application with Docker Compose**
   ```bash
   docker-compose up -d
   ```

3. **Access the application**
   - API: http://localhost:5000
   - Swagger UI: http://localhost:5000/swagger
   - Seq Logging: http://localhost:5341

### Manual Setup (Alternative)

1. **Restore dependencies**
   dotnet restore

2. **Run the application**
   dotnet run

## 🔧 Configuration

### Default Admin User
Of course normally I wouldn't share these informations in README but
The system automatically creates a default admin user:
- **Email**: root@admin.com
- **Password**: Final!00

## 🧪 Testing

### Running Tests
dotnet test

## 🐳 Docker

### Development

```bash
# Start all services
docker-compose up -d

# View logs
docker-compose logs -f api

# Stop services
docker-compose down

# Rebuild and start
docker-compose up --build
```

## 📊 Logging

The application uses Serilog for structured logging with multiple sinks:

- **Console**: Real-time logging during development
- **File**: Daily rolling log files in `Logs/` directory
- **Seq**: Structured logging server for advanced querying

### Log Levels

- **Information**: General application flow
- **Warning**: Potential issues
- **Error**: Exceptions and errors
- **Fatal**: Critical failures

## 🔒 Security

### JWT Authentication

- **Algorithm**: HMAC-SHA256
- **Token Expiry**: 60 minutes (configurable)
- **Claims**: User ID, Email, Role, User Type

### Authorization

The system supports role-based authorization:
- **Admin**: Full system access
- **Dietitian**: Client and diet plan management
- **Client**: Personal diet plan access

## 🆘 Support

For support and questions:
- Create an issue in the repository
- Contact the development team
- Check the API documentation at `/swagger`