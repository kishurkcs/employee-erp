# start-all.ps1
Write-Host "Starting all services..." -ForegroundColor Yellow

# Start containers
docker compose up --build -d

# Wait for SQL
Write-Host "Waiting for SQL Server..." -ForegroundColor Yellow
Start-Sleep -Seconds 20

# Check if database exists, create if not
$dbExists = docker exec employee-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P 'MyPassword@123' -C -Q "SELECT name FROM sys.databases WHERE name = 'EmployeeDb'" 2>$null

if ($dbExists -notlike "*EmployeeDb*") {
    Write-Host "Creating database..." -ForegroundColor Yellow
    docker exec -it employee-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P 'MyPassword@123' -C -Q "CREATE DATABASE EmployeeDb"
    
    Write-Host "Creating table..." -ForegroundColor Yellow
    docker exec -it employee-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P 'MyPassword@123' -C -d EmployeeDb -Q "CREATE TABLE Employees (Id INT PRIMARY KEY IDENTITY(1,1), Name NVARCHAR(100) NOT NULL, Department NVARCHAR(100))"
    
    Write-Host "Inserting data..." -ForegroundColor Yellow
    docker exec -it employee-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P 'MyPassword@123' -C -d EmployeeDb -Q "INSERT INTO Employees (Name, Department) VALUES ('Kishor', 'HRA'), ('Deva', 'DDG'), ('Shaam', 'Gaam')"
}

Write-Host "`nAll services are running!" -ForegroundColor Green
Write-Host "React UI: http://localhost:3000" -ForegroundColor Cyan
Write-Host "API Swagger: http://localhost:8080/swagger" -ForegroundColor Cyan
Write-Host "API Endpoint: http://localhost:8080/api/Employee" -ForegroundColor Cyan

# Show running containers
docker ps