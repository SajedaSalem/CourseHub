#  CourseHub — Student & Course Management REST API

CourseHub is a RESTful Web API developed using **ASP.NET Core (.NET 10)** and **Clean Architecture**.

The application provides a structured backend for managing students, courses, and student enrollments. It demonstrates separation of concerns, dependency inversion, database management with Entity Framework Core, and the implementation of common enterprise application patterns.

##  Features

### Student Management

* Register new students.
* Retrieve all students or find a student by ID.
* Update student information.
* Delete students who have no active enrollments.
* Prevent duplicate student email addresses.

### Course Management

* Create courses with a defined enrollment capacity.
* Retrieve all courses or find a course by ID.
* Update course details.
* Delete courses without active enrollments.

### Enrollment Management

* Enroll students in available courses.
* Retrieve all enrollment records.
* Cancel existing enrollments.
* Prevent duplicate student-course enrollments.
* Ensure referenced students and courses exist.
* Enforce course capacity limits for sequential enrollment requests.
* Restrict deletion of students and courses with existing enrollments.

### Validation & Error Handling

* Validate input using FluentValidation.
* Return appropriate HTTP status codes for invalid requests and business-rule violations.
* Use application-specific validation exceptions to keep FluentValidation isolated within Infrastructure.

---

##  Technologies

| Technology            | Purpose                              |
| --------------------- | ------------------------------------ |
| C# / .NET 10          | Application development              |
| ASP.NET Core Web API  | REST API implementation              |
| Entity Framework Core | Object-Relational Mapping            |
| SQL Server LocalDB    | Relational database                  |
| FluentValidation      | Input validation                     |
| Swagger / OpenAPI     | API documentation and manual testing |
| Git & GitHub          | Version control and collaboration    |

---

##  Clean Architecture

CourseHub follows Clean Architecture principles by separating the application into four projects.

```text
CourseHub/
│
├── CourseHub.Domain/
│   └── Entities/
│       ├── Student.cs
│       ├── Course.cs
│       └── Student_Course.cs
│
├── CourseHub.Application/
│   ├── DTOs/
│   │   ├── Students/
│   │   ├── Courses/
│   │   └── Enrollments/
│   ├── Interfaces/
│   │   └── Repositories/
│   ├── Services/
│   └── Exceptions/
│
├── CourseHub.Infrastructure/
│   ├── ORM/
│   │   ├── ApplicationDbContext.cs
│   │   └── Configurations/
│   ├── Repositories/
│   ├── Validation/
│   ├── Migrations/
│   └── UnitOfWork.cs
│
├── CourseHub.API/
│   ├── Controllers/
│   ├── Program.cs
│   └── appsettings.json
│
└── CourseHub.slnx
```

### 1. Domain Layer

Contains the core business entities:

* `Student`
* `Course`
* `Student_Course`

The Domain layer does not depend on other application layers or database technologies.

### 2. Application Layer

Contains the application's use cases and contracts.

Responsibilities include:

* Application services.
* Repository interfaces.
* Unit of Work interface.
* Data Transfer Objects (DTOs).
* Validation abstractions.
* Application-specific exceptions.

The Application layer depends on Domain but does not depend on Infrastructure.

### 3. Infrastructure Layer

Provides implementations of the abstractions defined in Application.

Responsibilities include:

* EF Core `ApplicationDbContext`.
* Database entity configurations.
* Generic and specialized repositories.
* Unit of Work implementation.
* FluentValidation implementations.
* Database migrations.

### 4. API Layer

Exposes the application's functionality through HTTP endpoints.

Responsibilities include:

* Receiving requests.
* Invoking Application services.
* Returning appropriate HTTP responses.
* Configuring dependency injection.
* Providing Swagger/OpenAPI documentation.

### Dependency Direction

```text
             CourseHub.API
               /      \
              ▼        ▼
     Application    Infrastructure
              ▲        │
              └────────┘
              │
              ▼
            Domain
```

Infrastructure implements interfaces defined in Application, while Domain remains independent of the outer layers.

---

## Design Patterns

### Generic Repository Pattern

A generic repository provides reusable database operations:

```csharp
public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    Task<List<T>> GetAllAsync();
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
}
```

The generic implementation is reused for multiple entity types.

Specialized repositories provide additional operations when necessary.

For example, `IEnrollmentRepository` includes methods for checking duplicate enrollments and counting students enrolled in a course.

### Unit of Work Pattern

The Unit of Work coordinates database changes through a shared EF Core `DbContext`.

```csharp
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
}
```

Repositories manage entities, while Unit of Work commits pending changes.

This separates database operations from persistence coordination.

### Dependency Injection

Dependencies are registered through ASP.NET Core's built-in dependency injection container.

Application services depend on interfaces rather than concrete Infrastructure classes.

### Dependency Inversion Principle

The Application layer defines the interfaces required by its use cases.

Infrastructure provides the concrete implementations.

This reduces coupling between business logic and persistence technologies.

---

## Database Design

CourseHub uses a relational SQL Server database with three main entities.

### Student

| Property     | Type     |
| ------------ | -------- |
| Id           | Guid     |
| FirstName    | string   |
| LastName     | string   |
| Email        | string   |
| DateOfBirth  | DateTime |
| RegisteredAt | DateTime |

Student email addresses are unique.

### Course

| Property    | Type     |
| ----------- | -------- |
| Id          | Guid     |
| Title       | string   |
| Description | string   |
| Capacity    | int      |
| CreatedAt   | DateTime |

Each course has a maximum enrollment capacity.

### Student_Course

| Property   | Type     |
| ---------- | -------- |
| Id         | Guid     |
| StudentId  | Guid     |
| CourseId   | Guid     |
| EnrolledAt | DateTime |

`Student_Course` is the junction entity implementing the many-to-many relationship.

### Relationships

```text
Student
   │
   │ 1
   │
   │ N
Student_Course
   │
   │ N
   │
   │ 1
 Course
```

A student can enroll in multiple courses, and a course can contain multiple students.

A unique composite index on `(StudentId, CourseId)` prevents duplicate student-course relationships.

### Delete Behavior

Both relationships use:

```csharp
.OnDelete(DeleteBehavior.Restrict);
```

Consequently:

* Students with existing enrollments cannot be deleted.
* Courses with existing enrollments cannot be deleted.
* Enrollment records must be removed before deleting the related student or course.

This protects referential integrity.

---

## API Endpoints

### Students

| Method | Endpoint             | Description              |
| ------ | -------------------- | ------------------------ |
| POST   | `/api/students`      | Register a student       |
| GET    | `/api/students`      | Retrieve all students    |
| GET    | `/api/students/{id}` | Retrieve a student by ID |
| PUT    | `/api/students/{id}` | Update a student         |
| DELETE | `/api/students/{id}` | Delete a student         |

### Courses

| Method | Endpoint            | Description             |
| ------ | ------------------- | ----------------------- |
| POST   | `/api/courses`      | Create a course         |
| GET    | `/api/courses`      | Retrieve all courses    |
| GET    | `/api/courses/{id}` | Retrieve a course by ID |
| PUT    | `/api/courses/{id}` | Update a course         |
| DELETE | `/api/courses/{id}` | Delete a course         |

### Enrollments

| Method | Endpoint                | Description                  |
| ------ | ----------------------- | ---------------------------- |
| POST   | `/api/enrollments`      | Enroll a student in a course |
| GET    | `/api/enrollments`      | Retrieve all enrollments     |
| DELETE | `/api/enrollments/{id}` | Cancel an enrollment         |

### HTTP Response Codes

| Status          | Meaning                       |
| --------------- | ----------------------------- |
| 200 OK          | Successful retrieval          |
| 201 Created     | Resource created              |
| 204 No Content  | Successful update or deletion |
| 400 Bad Request | Invalid input                 |
| 404 Not Found   | Resource does not exist       |
| 409 Conflict    | Business-rule violation       |

---

## Installation and Setup

### Prerequisites

Install:

* .NET 10 SDK
* SQL Server Express LocalDB
* Visual Studio with ASP.NET Core development tools, or another compatible .NET IDE
* Git

### 1. Clone the Repository

```bash
git clone https://github.com/SajedaSalem/CourseHub.git
cd CourseHub
```

Replace the repository URL with the actual GitHub repository URL.

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Configure the Database

The development configuration uses SQL Server LocalDB.

In `CourseHub.API/appsettings.json`, configure the connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=CourseHubDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

This configuration is intended for local Windows development. Other environments require an appropriate SQL Server connection string.

### 4. Apply Database Migrations

Open Visual Studio's Package Manager Console:

```powershell
Update-Database -Project CourseHub.Infrastructure -StartupProject CourseHub.API
```

Alternatively, if the EF Core CLI tools are installed:

```bash
dotnet ef database update --project CourseHub.Infrastructure --startup-project CourseHub.API
```

This creates or updates the database using the project's existing migrations.

### 5. Run the API

```bash
dotnet run --project CourseHub.API
```

The application starts using its configured development URLs.

### 6. Open Swagger

Navigate to:

```text
https://localhost:<port>/swagger
```

Replace `<port>` with the HTTPS port displayed when the API starts.

Swagger is enabled in the Development environment.

---

## Testing

The API was manually tested using Swagger.

### Student Tests

* Successful student registration.
* Retrieval of students.
* Student updates.
* Student deletion.
* Duplicate email detection.
* Input validation.

### Course Tests

* Successful course creation.
* Retrieval of courses.
* Course updates.
* Course deletion.
* Capacity validation.

### Enrollment Tests

* Successful student enrollment.
* Duplicate enrollment prevention.
* Course capacity enforcement.
* Missing student detection.
* Invalid GUID validation.
* Enrollment retrieval.
* Restricted student and course deletion.
* Deletion of students and courses without enrollments.

These tests verified the expected HTTP responses and business rules.

---

## Data Integrity

CourseHub protects data integrity through multiple mechanisms.

**Application validation:** FluentValidation checks incoming DTOs before processing.

**Business-rule checks:** Application services verify conditions such as course capacity, duplicate enrollments, and restricted deletion.

**Database constraints:** Unique indexes and foreign-key relationships provide an additional layer of protection.

**EF Core migrations:** Database schema changes are maintained through versioned migrations.

---

## Future Improvements

Potential enhancements include:

* Authentication and role-based authorization.
* Automated unit and integration tests.
* Pagination, filtering, and sorting.
* Global exception-handling middleware.
* Concurrency-safe enrollment capacity enforcement.
* Logging and monitoring.
* CI/CD pipelines.
* Containerization using Docker.

---

## Project Purpose

CourseHub was developed as a practical implementation of backend development concepts, including:

* Clean Architecture
* SOLID principles
* Repository Pattern
* Unit of Work Pattern
* Dependency Injection
* Entity Framework Core
* Relational database design
* REST API development
* Input validation
* Git-based feature development and pull requests

The project demonstrates how these concepts can be combined to build a maintainable and extensible backend application.
