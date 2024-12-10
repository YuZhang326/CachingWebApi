# Caching Web API

This project demonstrates the use of caching techniques in an ASP.NET Core Web API. It includes examples of implementing in-memory and distributed caching and uses PostgreSQL as the database provider.

---

## Features

1. **Caching**:
   - Supports **in-memory caching** for session data and temporary configuration settings.
   - Supports **distributed caching** for shared data across multiple instances, such as in a web server farm or microservices architecture.

2. **Database Integration**:
   - Uses PostgreSQL as the database provider via Entity Framework Core.

3. **Swagger Integration**:
   - Includes Swagger for API documentation and testing.

4. **Dependency Injection**:
   - Implements the `ICacheService` interface for caching operations.

---

## Prerequisites

- .NET 8.0 SDK or higher
- PostgreSQL database
- IDE or text editor like Visual Studio, VS Code, or JetBrains Rider

---

## Configuration

### appsettings.json
Update the connection string for your PostgreSQL database in the `appsettings.json` file:

```json
{
  "ConnectionStrings": {
    "SampleDbConnection": "Host=localhost;Port=5432;Database=SampleDb;Username=your_username;Password=your_password"
  }
}
