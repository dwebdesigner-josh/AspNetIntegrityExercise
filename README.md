# AspNetIntegrityExercise
Implementation of Banking API using ASP.NET Core Web API and repository pattern.

## Requirements

    .NET 9 SDK
    PostgreSQL (optional, for manual testing)

## Setup Instructions

    
### 1. clone repository to your device
    
### 2. build unit tests: (in Terminal)

```bash 
cd AspNetIntegrityExercise

dotnet restore

dotnet build
```

### 3. run xUnit tests:

```bash 
cd api.Tests

dotnet test
```

### 4. run the api:

```bash 
cd ..

dotnet run --project api
```

### 5. Database Setup (optional)

#### 1. ensure you have postgresql installed and accessible via CLI (psql) or pgAdmin
        
#### 2. configure connection string (in api/appsettings.json)

Change connection string to your postgresql connection string, ex:

```json
"DefaultConnection" : "Host=localhost;Port=5432;Database=aspnetintegrity;Username=postgres;Password=your_password_here"
```

(Recommended) use a secret instead of directly putting your connection string in the project file

```bash 
cd api

dotnet user-secrets init

dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=aspnetintegrity;Username=postgres;Password=your_password_here"
```

Then update api/appsettings.json:
     
```json
"DefaultConnection" : ""
```


#### 3. Create database (in psql/pgAdmin)

Log in to psql (or open pgAdmin):
    
```bash
psql -U yourpostgresusernamehere

```

Create database with same name as in the connection string
(or create from within pgAdmin)

```sql
CREATE DATABASE aspnetintegrity;

\q
```

#### 4. Apply migrations (in CLI)

```bash
cd api

dotnet tool install --global dotnet-ef

dotnet ef database update 
```

#### 5. run the app:   

```bash
dotnet watch run --launch-profile https
```

####    6.  in pgAdmin, open database to manually view/edit data

####    7. in web browser, open Swagger to manually test endpoints:

https://localhost:7277/swagger/index.html

