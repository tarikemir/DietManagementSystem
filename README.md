## !!! THERE IS A POSTMAN COLLECTION IN THE REPOSITORY !!! Detailed api requests can be imported to Postman.
## !!! DATABASE SCHEMA DOCUMENTATION IN THE REPOSITORY !!! You can find the database schema documentation in the repository as pdf file.

# Diet Management System

A comprehensive .NET 8 API for managing diet plans, clients, and dietitians with JWT authentication and PostgreSQL database.

## 🏗️ Architecture

This project follows Clean Architecture principles with the following layers:

- **API Layer** (`DietManagementSystem.API`) - API endpoints, DI
- **Application Layer** (`DietManagementSystem.Application`) - Business logic, CQRS with MediatR
- **Domain Layer** (`DietManagementSystem.Domain`) - Entities and domain logic
- **Infrastructure Layer** (`DietManagementSystem.Infrastructure`) - External services and repositories
- **Persistence Layer** (`DietManagementSystem.Persistence`) - Database context and configurations

 RESTful API design
 Authentication with .NET Identity Framework
 Role-based authorization
 Adherence to DRY principles
 Fluent validation for data integrity
 Proper database design with Entity Framework
 Comprehensive error handling
 API versioning
 Appropriate logging mechanisms
 Unit tests for core functionality
 Use of CQRS pattern
 JWT token-based authentication

 ## Documentation
 API documentation using Swagger/OpenAPI
 A detailed README with instructions on setting up, testing, and deploying the system
 Database schema documentation can be found in the repository as pdf file.
 Endpoint documentation with request/response examples ( Whole Postman collection is in the repository)

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

2. **Start the application with dotnet cli **
   ```bash
    dotnet restore
	dotnet build
    dotnet run --project DietManagementSystem.API
   ```

3. **Access the application**
   - API: http://localhost:5000
   - Swagger UI: http://localhost:5000/swagger

## 🔧 Configuration

### Default Admin User
Of course normally I wouldn't share these informations in README but
The system automatically creates a default admin user:
- **Email**: root@admin.com
- **Password**: Final!00

## 🧪 Testing

### Running Tests
   ```bash
	dotnet test --verbosity normal
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
- **Client**: Can only register if dietitian is known