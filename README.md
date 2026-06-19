# FinAccount Mongo API

Quinto proyecto del programa **Bankaool .NET Fintech Bridge Lab**.

API REST con ASP.NET Core Web API y MongoDB para practicar persistencia real en backend fintech.

## Stack

- Ubuntu
- .NET 8
- C#
- ASP.NET Core Web API
- MongoDB 6.0.20
- MongoDB.Driver
- Swagger/OpenAPI
- Postman

## Conceptos practicados

- Controllers
- DTOs
- Models
- Repositories
- Services
- Validators
- Helpers
- Dependency Injection
- MongoDB settings
- MongoDB.Driver
- IMongoCollection<T>
- Find
- InsertOneAsync
- FindOneAndUpdateAsync
- UpdateOneAsync
- ApiResponse<T>
- OperationResult<T>
- HTTP status codes: 200, 201, 400, 404, 409

## MongoDB

Connection string:

```txt
mongodb://localhost:27017

Database:

finaccount_mongo_db

Collections:

accounts
movements
Endpoints
Health
GET /api/health
Accounts
GET /api/accounts
GET /api/accounts/{accountNumber}
POST /api/accounts
PATCH /api/accounts/{accountNumber}/status
Movements
GET /api/accounts/{accountNumber}/movements
POST /api/accounts/{accountNumber}/movements
Summary
GET /api/accounts/{accountNumber}/summary
Dashboard
GET /api/dashboard
Response standard
{
  "success": true,
  "message": "Operation completed successfully.",
  "data": {}
}
Comandos

Restaurar:

dotnet restore

Build:

dotnet build

Ejecutar:

dotnet run

Modo watch:

dotnet watch run

Limpiar:

pkill -f "dotnet"
dotnet clean
rm -rf bin obj
Swagger

Con la API corriendo:

http://localhost:5174/swagger
Postman

Colección:

postman/finaccount-mongo-api.postman_collection.json

Variable:

baseUrl = http://localhost:5174
Arquitectura final
HTTP Request
→ Controller
→ Repository / Application Service
→ Validator
→ MongoDB
→ OperationResult<T>
→ ApiResponse<T>
→ HTTP Response
Cierre
finaccount-mongo-api · Paso 11 listo

