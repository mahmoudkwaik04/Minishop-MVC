# ---- Base image: runtime ----
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
# Optional: healthcheck (app listens on 80)
# HEALTHCHECK --interval=30s --timeout=3s CMD wget -qO- http://localhost/ || exit 1

# ---- Build image ----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy sln and project files first to leverage Docker layer caching
COPY MiniEShop.sln ./
COPY EShop.MVC/EShop.MVC.csproj EShop.MVC/EShop.MVC.csproj
COPY InventoryService/InventoryService.csproj InventoryService/InventoryService.csproj
COPY OrderService/OrderService.csproj OrderService/OrderService.csproj
COPY QueryAPI/QueryAPI.csproj QueryAPI/QueryAPI.csproj
COPY Shared/Shared.csproj Shared/Shared.csproj

RUN dotnet restore "EShop.MVC/EShop.MVC.csproj"

# Copy the rest of the source
COPY . .

# Build
RUN dotnet build "EShop.MVC/EShop.MVC.csproj" -c Release -o /app/build

# ---- Publish image ----
FROM build AS publish
RUN dotnet publish "EShop.MVC/EShop.MVC.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ---- Final runtime image ----
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# If ASPNETCORE_URLS not provided, default to http://+:80
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "EShop.MVC.dll"]
