# AspNetIntegrityExercise
Implementation of Banking API using ASP.NET Core Web API and repository pattern.

* Requirements

    .NET 9 SDK
    PostgreSQL (optional, for manual testing)

* Setup Instructions

    
    1. clone repository to your device
    
    2. build unit tests: (in Terminal)

        cd AspNetIntegrityExercise

        dotnet restore
        dotnet build
        

    3. run xUnit tests:

        cd api.Tests
        dotnet test

    4. run the api:

        cd ..
        dotnet run --project api

    5. Database Setup (optional)

        1. ensure you have postgresql installed and accessible via CLI (psql) or pgAdmin
        
        2. configure connection string (in api/appsettings.json)

            - change "DefaultConnection" : "Host=localhost;Port=5432;Database=aspnetintegrity;Username=postgres;Password=your_password_here" to your postgresql connection string

            - (recommended) use a secret instead of directly putting your connection string in the project file

                * in CLI:

                    cd api
                    
                    dotnet user-secrets init

                    dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=aspnetintegrity;Username=postgres;Password=yourpasswordhere"

                * in api/appsettings.json
                
                    change FROM:
                    
                    "DefaultConnection" : "Host=localhost;Port=5432;Database=aspnetintegrity;Username=postgres;Password=your_password_here" 

                    TO:

                    "DefaultConnection" : ""


        3. create database (in psql/pgAdmin)

            - log in to psql:
                
                psql -U yourpostgresusernamehere

                * or open pgAdmin

            - create db with same name as in the connection string

                CREATE DATABASE aspnetintegrity;

                \q

                * or create from within pgAdmin

        4. Apply migrations (in CLI)

                cd api

                dotnet tool install --global dotnet-ef
            
                dotnet ef database update 

        5. run the app:   

                dotnet watch run --launch-profile https

        6.  in pgAdmin, open database to manually view/edit data

        7. in web browser, open Swagger to manually test endpoints:
            
                https://localhost:7277/swagger/index.html

