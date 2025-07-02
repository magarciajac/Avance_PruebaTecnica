# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar el proyecto y restaurar
COPY ["SentimentApi.csproj", "./"]
RUN dotnet restore "SentimentApi.csproj"

# Copiar el resto del código y compilar
COPY . .
RUN dotnet publish "SentimentApi.csproj" -c Release -o /app/publish

# Etapa final
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SentimentApi.dll"]