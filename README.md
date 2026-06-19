# FinAccount Mongo API

Quinto proyecto del programa **Bankaool .NET Fintech Bridge Lab**.

API REST con ASP.NET Core Web API y MongoDB.

## Objetivo

Migrar de datos en memoria a persistencia real usando MongoDB 6.0.20.

## Stack

- Ubuntu
- .NET 8
- C#
- ASP.NET Core Web API
- MongoDB 6.0.20
- MongoDB.Driver
- Swagger/OpenAPI
- Postman

## Conceptos

| Node / Express / Mongoose | ASP.NET Core / MongoDB.Driver |
|---|---|
| Mongoose model | C# model |
| schema | class |
| collection | IMongoCollection<T> |
| find | Find(...) |
| insertOne | InsertOneAsync(...) |
| updateOne | UpdateOneAsync(...) |
| ObjectId | ObjectId / string id |

## Comandos

Restaurar paquetes:

```bash
dotnet restore

Build:

dotnet build

Ejecutar:

dotnet run

Modo desarrollo:

dotnet watch run
MongoDB local esperado
mongodb://localhost:27017

Base de datos:

finaccount_mongo_db
Metodología

App x paso y listo.

Para avanzar:

finaccount-mongo-api · Paso X listo

