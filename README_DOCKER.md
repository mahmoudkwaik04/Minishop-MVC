# MiniEShop Docker Run

This repository now includes a Dockerfile for the ASP.NET Core MVC app (EShop.MVC) and updates to Docker/docker-compose.yml to run the app alongside Kafka, Zookeeper, SQL Server, and MongoDB.

## Build & Run

From the `MiniEShop/Docker` directory:
```bash
docker-compose down
docker-compose build --no-cache
docker-compose up -d
```

Then open http://localhost:5000

## Local test without Docker
```bash
dotnet clean
dotnet restore
dotnet run --project EShop.MVC/EShop.MVC.csproj
```

