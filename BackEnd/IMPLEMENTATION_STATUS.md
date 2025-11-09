# 🏗️ Backend Architecture Implementation - TikTok Clone

## ✅ Completed: Identity Service (Authentication & Authorization)

### Architecture Layers Implemented

#### 1. **Domain Layer** (`Identity.Domain`)
- ✅ **Entities**: `User` (Aggregate Root with DDD patterns)
- ✅ **Value Objects**: `Email`, `UserRole`
- ✅ **Domain Events**:
  - `UserRegisteredEvent`
  - `UserLoggedInEvent`
  - `UserPasswordChangedEvent`
  - `UserEmailVerifiedEvent`
  - `UserRoleChangedEvent`
  - `UserDeactivatedEvent`
- ✅ **Repositories**: `IUserRepository` (domain interface)

#### 2. **Application Layer** (`Identity.Application`)
- ✅ **CQRS Pattern with MediatR**:
  - **Commands**:
    - `RegisterCommand` + Handler + Validator
    - `LoginCommand` + Handler + Validator
  - **Queries**:
    - `GetUserByIdQuery` + Handler
- ✅ **DTOs**: `LoginResponse`, `RegisterResponse`, `UserDto`
- ✅ **Interfaces**: `IPasswordHasher`, `IJwtTokenGenerator`
- ✅ **FluentValidation** for input validation

#### 3. **Infrastructure Layer** (`Identity.Infrastructure`)
- ✅ **EF Core + PostgreSQL**:
  - `IdentityDbContext` with proper entity configuration
  - Value object mapping for `Email`
  - Enum to string conversion for `UserRole`
- ✅ **Repository Implementation**: `UserRepository`
- ✅ **Security Services**:
  - `PasswordHasher` (BCrypt)
  - `JwtTokenGenerator` (JWT tokens)
- ✅ **Caching**: `RedisCacheService` (Redis implementation)
- ✅ **Dependency Injection**: `DependencyInjection` extension

#### 4. **Presentation Layer** (`Identity.Web`)
- ✅ **RESTful API Controller**: `AuthController`
  - `POST /api/auth/register` - User registration
  - `POST /api/auth/login` - User login with JWT
  - `GET /api/auth/me` - Get current user
  - `POST /api/auth/logout` - Logout
- ✅ **JWT Authentication** with cookie support
- ✅ **CORS** configuration for frontend
- ✅ **Swagger** API documentation

### Key Features Implemented

✅ **DDD Patterns**:
- Aggregate Roots
- Value Objects
- Domain Events
- Rich domain models with business logic

✅ **CQRS**:
- Separate command and query models
- MediatR for command/query handling
- FluentValidation for command validation

✅ **Clean Architecture**:
- Clear separation of concerns
- Dependency inversion
- Domain at the center

✅ **Security**:
- BCrypt password hashing (work factor: 12)
- JWT access tokens
- HTTP-only cookies for refresh tokens
- Role-based authorization

---

## 🚧 Next Steps: Remaining Services

### 2. Video Service (NEXT)
**Database**: PostgreSQL
**Features**:
- Video metadata storage
- Upload video (file/URL)
- Feed with pagination
- View counter (Redis cache)
- Video recommendations

**Domain Entities**:
- `Video` (Aggregate Root)
- `VideoMetadata` (Value Object)
- Domain Events: `VideoUploadedEvent`, `VideoDeletedEvent`

### 3. Interaction Service
**Database**: PostgreSQL + Redis
**Features**:
- Like video
- Comment on video
- View tracking
- Real-time counters (Redis)
- Event broadcasting

**Domain Entities**:
- `Like` (Aggregate)
- `Comment` (Aggregate)
- `View` (Aggregate)

### 4. User Service
**Database**: PostgreSQL
**Features**:
- User profile management
- Avatar upload
- Follow/Unfollow
- User statistics

**Domain Entities**:
- `UserProfile` (Aggregate Root)
- `FollowRelationship` (Aggregate)

### 5. API Gateway
**Technology**: Ocelot
**Features**:
- Route aggregation
- JWT validation
- Rate limiting
- Load balancing

---

## 📦 Shared Kernel Components

✅ **Created**:
- `BaseEntity<TId>` - Base class for all entities
- `IAggregateRoot` - Marker interface
- `DomainEvent` / `IDomainEvent` - Event infrastructure
- `ValueObject` - Base class for value objects
- `Result<T>` - Result pattern for error handling
- `IRepository<TEntity, TId>` - Generic repository
- `IUnitOfWork` - Transaction management
- `ICacheService` - Caching abstraction
- `IEventBus` - Event bus abstraction
- `PagedResult<T>` - Pagination support
- `Repository<TEntity, TId>` - Generic EF Core repository
- `UnitOfWork` - UnitOfWork with domain event dispatching

---

## 🗄️ Database Schema (Identity Service)

### Users Table
```sql
CREATE TABLE "Users" (
    "Id" UUID PRIMARY KEY,
    "Email" VARCHAR(256) NOT NULL UNIQUE,
    "Username" VARCHAR(50) NOT NULL UNIQUE,
    "PasswordHash" VARCHAR(512) NOT NULL,
    "Role" VARCHAR(50) NOT NULL,
    "IsEmailVerified" BOOLEAN NOT NULL,
    "IsActive" BOOLEAN NOT NULL,
    "CreatedAt" TIMESTAMP NOT NULL,
    "UpdatedAt" TIMESTAMP NULL,
    "LastLoginAt" TIMESTAMP NULL,
    "RefreshToken" VARCHAR(512) NULL,
    "RefreshTokenExpiresAt" TIMESTAMP NULL
);
```

---

## 🐳 Docker Compose Configuration

### Services Required:
1. **PostgreSQL** (Identity, Video, Interaction, User DBs)
2. **Redis** (Cache & Pub/Sub)
3. **RabbitMQ** (Optional - Message broker)
4. All microservices (Identity, Video, Interaction, User)
5. API Gateway

---

## 🔧 How to Run Identity Service

### Prerequisites:
```bash
# Install .NET 8 SDK
# Install PostgreSQL
# Install Redis
```

### Database Migration:
```bash
cd BackEnd/TiktokClone/Services/Identity/Src/Identity.Web
dotnet ef migrations add InitialCreate --project ../Identity.Infrastructure
dotnet ef database update
```

### Run Service:
```bash
dotnet run
```

### API Endpoints:
- Swagger: `https://localhost:5001/swagger`
- Register: `POST /api/auth/register`
- Login: `POST /api/auth/login`
- Get User: `GET /api/auth/me`

---

## 📊 Architecture Patterns Applied

### ✅ Clean Architecture
- Dependency flows inward
- Domain has no external dependencies
- Infrastructure depends on Application

### ✅ DDD (Domain-Driven Design)
- Aggregates and Aggregate Roots
- Value Objects
- Domain Events
- Rich domain models
- Ubiquitous language

### ✅ CQRS (Command Query Responsibility Segregation)
- Commands for writes
- Queries for reads
- Separate models
- MediatR pipeline

### ✅ Repository Pattern
- Abstraction over data access
- Clean separation of concerns

### ✅ Unit of Work Pattern
- Transaction management
- Coordinated persistence

### ✅ Result Pattern
- No exceptions for business logic
- Explicit success/failure handling

### ✅ Dependency Injection
- Loose coupling
- Testability
- Configuration at startup

---

## 🎯 Senior-Level Best Practices Applied

1. **Separation of Concerns**: Clear layer boundaries
2. **SOLID Principles**: Throughout the codebase
3. **Immutability**: Value objects are immutable
4. **Encapsulation**: Private setters, factory methods
5. **Domain Events**: Decoupled communication
6. **Validation**: FluentValidation with clear rules
7. **Security**: BCrypt + JWT + HttpOnly cookies
8. **Async/Await**: Proper async patterns
9. **Nullable Reference Types**: Enabled for safety
10. **Logging**: ILogger integration
11. **Configuration**: Options pattern
12. **Error Handling**: Result pattern instead of exceptions

---

## 📝 Next Implementation Priority

1. ✅ **Identity Service** - COMPLETED
2. 🔄 **Video Service** - IN PROGRESS
3. ⏳ **Interaction Service** - PENDING
4. ⏳ **User Service** - PENDING
5. ⏳ **API Gateway** - PENDING
6. ⏳ **Docker Compose** - PENDING
7. ⏳ **CI/CD** - PENDING

---

**Implementation Status**: Identity Service is production-ready with Clean Architecture, DDD, and CQRS patterns. Ready to implement Video Service next.
