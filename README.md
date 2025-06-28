# Prescription API

A comprehensive ASP.NET Core Web API for managing medical prescriptions, patients, doctors, and medications. This API provides secure authentication, prescription management, and patient information retrieval with a robust, scalable architecture.

## Features

### 🔐 Authentication & Security
- **JWT Authentication**: Secure token-based authentication system
- **Refresh Tokens**: Long-lived tokens for seamless user experience
- **Password Security**: PBKDF2 hashing with salt for maximum security
- **Token Management**: Token revocation and refresh capabilities
- **Authorization**: Role-based access control for protected endpoints

### 💊 Prescription Management
- **Create Prescriptions**: Add new prescriptions with multiple medications
- **Patient Management**: Automatic patient creation and lookup
- **Doctor Validation**: Ensure prescriptions are created by valid doctors
- **Medication Validation**: Verify medication existence and dosage
- **Business Rules**: Enforce prescription limits and validation rules

### 📋 Data Management
- **Patient Details**: Comprehensive patient information with prescription history
- **Prescription History**: Complete medication history with dosage and timing
- **Doctor Information**: Healthcare provider details and credentials
- **Medication Database**: Extensive medication catalog with descriptions

### 🏗️ Technical Features
- **Clean Architecture**: Layered design with clear separation of concerns
- **Entity Framework Core**: Modern ORM with migrations and relationships
- **Swagger Documentation**: Interactive API documentation and testing
- **Unit Testing**: Comprehensive test coverage with xUnit
- **Input Validation**: Data annotations and custom validation rules
- **Error Handling**: Structured exception handling with meaningful responses

## API Endpoints

### Authentication Endpoints
```
POST /api/auth/register     - Register new user
POST /api/auth/login        - Authenticate user
POST /api/auth/refresh      - Refresh access token
POST /api/auth/revoke       - Revoke refresh token
```

### Prescription Endpoints
```
POST /api/prescription                  - Create new prescription
GET  /api/prescription/patient/{id}     - Get patient details with prescriptions
```

## Technology Stack

### Core Framework
- **ASP.NET Core 8.0**: Modern web framework for building APIs
- **Entity Framework Core 9.0**: Object-relational mapping (ORM)
- **SQL Server**: Relational database for data persistence

### Authentication & Security
- **JWT Bearer Authentication**: JSON Web Token implementation
- **Microsoft.IdentityModel.Tokens**: Token validation and security
- **PBKDF2**: Password-Based Key Derivation Function for hashing

### Documentation & Testing
- **Swagger/OpenAPI**: Interactive API documentation
- **xUnit**: Unit testing framework
- **In-Memory Database**: Testing with isolated database contexts

### Development Tools
- **Visual Studio/Rider**: IDE support with IntelliSense
- **Entity Framework Migrations**: Database schema management
- **Dependency Injection**: Built-in IoC container

## Requirements

### System Requirements
- **.NET 8.0 SDK**: Latest version of .NET development kit
- **SQL Server**: LocalDB, Express, or full SQL Server instance
- **Visual Studio 2022** or **JetBrains Rider** (recommended IDEs)

### Database
- **SQL Server LocalDB** (included with Visual Studio)
- Or **SQL Server Express/Developer Edition**
- Or **Azure SQL Database** for cloud deployment

## Installation & Setup

### 1. Clone Repository
```bash
git clone link
cd prescription-api/PrescriptionAPI
```

### 2. Configure Database
Update the connection string in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PrescriptionDB;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### 3. Configure JWT Settings
Update JWT configuration in `appsettings.json`:
```json
{
  "Jwt": {
    "Key": "YourSuperSecretKeyThatIsAtLeast32CharactersLong123456789",
    "Issuer": "PrescriptionAPI",
    "Audience": "PrescriptionAPI"
  }
}
```

### 4. Install Dependencies
```bash
dotnet restore
```

### 5. Apply Database Migrations
```bash
dotnet ef database update
```

### 6. Run the Application
```bash
dotnet run
```

The API will be available at:
- **HTTP**: `http://localhost:5105`
- **HTTPS**: `https://localhost:7087`
- **Swagger UI**: `https://localhost:7087/swagger`

## Database Schema

### Core Entities

#### Users Table
```sql
- IdUser (int, PK)
- Username (varchar(50), unique)
- PasswordHash (varchar(max))
- Salt (varchar(max))
- CreatedAt (datetime)
```

#### Patients Table
```sql
- IdPatient (int, PK)
- FirstName (varchar(100))
- LastName (varchar(100))
- Birthdate (datetime)
```

#### Doctors Table
```sql
- IdDoctor (int, PK)
- FirstName (varchar(100))
- LastName (varchar(100))
- Email (varchar(100))
```

#### Medications Table
```sql
- IdMedicament (int, PK)
- Name (varchar(100))
- Description (varchar(100))
- Type (varchar(100))
```

#### Prescriptions Table
```sql
- IdPrescription (int, PK)
- Date (datetime)
- DueDate (datetime)
- IdPatient (int, FK)
- IdDoctor (int, FK)
```

#### Prescription_Medications Table
```sql
- IdMedicament (int, FK)
- IdPrescription (int, FK)
- Dose (int)
- Details (varchar(100))
```

## API Usage Examples

### Authentication

#### Register New User
```bash
curl -X POST https://localhost:7087/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "username": "doctor123",
    "password": "SecurePassword123!"
  }'
```

#### Login
```bash
curl -X POST https://localhost:7087/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "username": "doctor123",
    "password": "SecurePassword123!"
  }'
```

Response:
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "abc123def456...",
  "expiresAt": "2024-01-01T12:30:00Z"
}
```

### Prescription Management

#### Create Prescription
```bash
curl -X POST https://localhost:7087/api/prescription \
  -H "Authorization: Bearer YOUR_ACCESS_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "patient": {
      "firstName": "John",
      "lastName": "Doe",
      "birthdate": "1980-05-15T00:00:00Z"
    },
    "doctor": {
      "idDoctor": 1,
      "firstName": "Dr. Jane",
      "lastName": "Smith",
      "email": "jane.smith@hospital.com"
    },
    "date": "2024-01-01T10:00:00Z",
    "dueDate": "2024-01-08T10:00:00Z",
    "medicaments": [
      {
        "idMedicament": 1,
        "name": "Aspirin",
        "dose": 500,
        "details": "Take twice daily with food"
      }
    ]
  }'
```

#### Get Patient Details
```bash
curl -X GET https://localhost:7087/api/prescription/patient/1 \
  -H "Authorization: Bearer YOUR_ACCESS_TOKEN"
```

Response:
```json
{
  "idPatient": 1,
  "firstName": "John",
  "lastName": "Doe",
  "birthdate": "1980-05-15T00:00:00Z",
  "prescriptions": [
    {
      "idPrescription": 1,
      "date": "2024-01-01T10:00:00Z",
      "dueDate": "2024-01-08T10:00:00Z",
      "doctor": {
        "idDoctor": 1,
        "firstName": "Dr. Jane",
        "lastName": "Smith"
      },
      "medicaments": [
        {
          "idMedicament": 1,
          "name": "Aspirin",
          "dose": 500,
          "description": "Pain reliever and anti-inflammatory",
          "details": "Take twice daily with food"
        }
      ]
    }
  ]
}
```

## Project Structure

```
PrescriptionAPI/
├── Controllers/              # API endpoints and request handling
│   ├── AuthController.cs    # Authentication endpoints
│   └── PrescriptionController.cs # Prescription management
├── Data/                    # Database context and configuration
│   └── PrescriptionDbContext.cs
├── Models/                  # Data models and DTOs
│   ├── DTOs/               # Data Transfer Objects
│   │   ├── AuthDtos.cs     # Authentication DTOs
│   │   ├── CreatePrescriptionDto.cs
│   │   ├── PatientDetailDto.cs
│   │   └── ...             # Other DTOs
│   └── Entities/           # Database entities
│       ├── User.cs         # User entity
│       ├── Patient.cs      # Patient entity
│       ├── Doctor.cs       # Doctor entity
│       ├── Prescription.cs # Prescription entity
│       └── ...             # Other entities
├── Services/               # Business logic layer
│   ├── Interfaces/         # Service contracts
│   │   ├── IAuthService.cs
│   │   └── IPrescriptionService.cs
│   ├── AuthService.cs      # Authentication business logic
│   └── PrescriptionService.cs # Prescription business logic
├── Exceptions/             # Custom exception classes
│   └── AuthenticationException.cs
├── Tests/                  # Unit tests
│   └── PrescriptionServiceTests.cs
├── Program.cs              # Application startup and configuration
├── appsettings.json        # Configuration settings
└── PrescriptionAPI.csproj  # Project file and dependencies
```

## Architecture Overview

### Clean Architecture Principles
The application follows clean architecture patterns with clear separation of concerns:

#### Presentation Layer (Controllers)
- **AuthController**: Handles user registration, login, and token management
- **PrescriptionController**: Manages prescription creation and patient queries
- **Error Handling**: Consistent exception handling with appropriate HTTP status codes

#### Business Logic Layer (Services)
- **AuthService**: User authentication, password hashing, JWT token generation
- **PrescriptionService**: Prescription validation, patient management, business rules
- **Dependency Injection**: Loose coupling through interface-based design

#### Data Access Layer (DbContext)
- **Entity Framework Core**: ORM for database operations
- **Repository Pattern**: Encapsulated data access through DbContext
- **Migrations**: Database schema versioning and deployment

#### Domain Models
- **Entities**: Database-mapped objects with relationships
- **DTOs**: Data transfer objects for API communication
- **Validation**: Data annotations for input validation

### Security Implementation

#### Password Security
```csharp
// PBKDF2 with SHA256 and salt
private string HashPassword(string password, string salt)
{
    using var pbkdf2 = new Rfc2898DeriveBytes(password, 
        Convert.FromBase64String(salt), 10000, HashAlgorithmName.SHA256);
    var hash = pbkdf2.GetBytes(32);
    return Convert.ToBase64String(hash);
}
```

#### JWT Token Generation
```csharp
// Access token with claims
var claims = new[]
{
    new Claim(ClaimTypes.NameIdentifier, user.IdUser.ToString()),
    new Claim(ClaimTypes.Name, user.Username),
    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
};
```

#### Transaction Management
```csharp
// Database transactions for data consistency
using var transaction = await _context.Database.BeginTransactionAsync();
try
{
    // Multiple database operations
    await transaction.CommitAsync();
}
catch
{
    await transaction.RollbackAsync();
    throw;
}
```

## Testing

### Running Unit Tests
```bash
dotnet test
```

### Test Coverage
The project includes comprehensive unit tests covering:
- **Prescription Creation**: Valid and invalid scenarios
- **Business Rule Validation**: Date validation, medication limits
- **Error Handling**: Exception scenarios and edge cases
- **Data Validation**: Input validation and data integrity

### Example Test
```csharp
[Fact]
public async Task CreatePrescriptionAsync_WithValidData_ShouldCreatePrescription()
{
    // Arrange
    var dto = new CreatePrescriptionDto { /* test data */ };
    
    // Act
    var result = await _service.CreatePrescriptionAsync(dto);
    
    // Assert
    Assert.True(result > 0);
    var prescription = await _context.Prescriptions.FindAsync(result);
    Assert.NotNull(prescription);
}
```

## Configuration

### JWT Configuration
Configure JWT settings for token validation:
```json
{
  "Jwt": {
    "Key": "YourSecretKey (minimum 32 characters)",
    "Issuer": "PrescriptionAPI",
    "Audience": "PrescriptionAPI"
  }
}
```

### Database Configuration
Configure connection string for your environment:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Your SQL Server connection string"
  }
}
```

### Swagger Configuration
Swagger is configured with JWT authentication support:
```csharp
c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
{
    Description = "JWT Authorization header using the Bearer scheme",
    Name = "Authorization",
    In = ParameterLocation.Header,
    Type = SecuritySchemeType.Http,
    Scheme = "bearer",
    BearerFormat = "JWT"
});
```

## Deployment

### Local Development
1. Install SQL Server LocalDB
2. Update connection string
3. Run `dotnet ef database update`
4. Start with `dotnet run`

### Production Deployment
1. **Azure App Service**: Deploy as Web App
2. **Azure SQL Database**: Use managed SQL service
3. **Environment Variables**: Configure sensitive settings
4. **HTTPS**: Enable SSL/TLS in production
5. **Logging**: Configure application insights

### Docker Deployment
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["PrescriptionAPI.csproj", "."]
RUN dotnet restore
COPY . .
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PrescriptionAPI.dll"]
```

## Security Considerations

### Production Security
- **Environment Variables**: Store secrets in environment variables
- **HTTPS Only**: Enforce HTTPS in production
- **CORS Policy**: Configure appropriate CORS settings
- **Rate Limiting**: Implement API rate limiting
- **Input Validation**: Validate all user inputs
- **SQL Injection Protection**: Use parameterized queries
- **XSS Prevention**: Sanitize outputs appropriately

### Data Privacy
- **HIPAA Compliance**: Ensure healthcare data compliance
- **Data Encryption**: Encrypt sensitive data at rest
- **Audit Logging**: Log all prescription access and changes
- **Access Controls**: Implement proper authorization
- **Data Retention**: Define data retention policies

## Support & Documentation

### API Documentation
- **Swagger UI**: Available at `/swagger` endpoint
- **OpenAPI Spec**: Standard API specification format
- **Postman Collection**: Import API endpoints for testing

### Troubleshooting
- **Database Issues**: Check connection string and migrations
- **Authentication Problems**: Verify JWT configuration
- **Permission Errors**: Ensure proper authorization headers
- **Validation Failures**: Check request body format and required fields
