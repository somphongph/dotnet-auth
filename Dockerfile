FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim

WORKDIR /app

COPY publish/ ./

ENTRYPOINT ["dotnet", "API.dll"]
