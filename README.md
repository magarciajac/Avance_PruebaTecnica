# Sentiment API – Prueba Técnica

Servicio REST en **ASP.NET Core 8.0** que clasifica comentarios (positivo / negativo / neutral) y los guarda en **SQL Server**.  
Todo corre dentro de contenedores Docker (API + BD) y se gestiona con **Docker Compose**.

---

## 1 · Requisitos

| Herramienta        | Versión recomendada |
|--------------------|---------------------|
| Docker Desktop     | ≥ 24.x (incluye Compose) |
| .NET SDK (opcional)| ≥ 8.0 *(solo para ejecutar tests o compilar fuera de Docker)* |

---

## 2 · Arranque rápido

```bash

Nota: Se recoienda borrar las imagenes y containers de Docker antes de correr algun comando.

# 0) Por si hubiese contenedores viejos
docker compose down

# 1) Construir imagen y levantar API + SQL Server
docker compose up --build

api-1  |       Overriding HTTP_PORTS '8080' and HTTPS_PORTS ''. Binding to values defined by URLS instead 'http://+:80'.
api-1  | info: Microsoft.Hosting.Lifetime[14]
api-1  |       Now listening on: http://[::]:80
api-1  | info: Microsoft.Hosting.Lifetime[0]
api-1  |       Application started. Press Ctrl+C to shut down.
api-1  | info: Microsoft.Hosting.Lifetime[0]
api-1  |       Hosting environment: Production
api-1  | info: Microsoft.Hosting.Lifetime[0]
api-1  |       Content root path: /app

NOTA: Al ver esto puedes pasar al num.2 la primera vez puede fallar por la Base de Datos favor de ir al  # 6 


# 2) Explorar la API con Swagger

http://localhost:5196/swagger/index.html

Swagger UI actúa como Postman: puedes invocar los métodos, ver modelos y probar
la clasificación de sentimientos.

# 3) Endpoints

POST
/api/comments
{
  "id": 0,
  "productId": "string",
  "userId": "string",
  "commentText": "string",
  "sentiment": "string",
  "createdAt": "2025-07-02T04:33:14.759Z"
}
Crea un comentario, clasifica su sentimiento y lo guarda.

GET
/api/comments
http://localhost:5196/api/comments
http://localhost:5196/api/comments?productId=productId
Nota: neutral/negativo/positivo
http://localhost:5196/api/comments?sentiment=neutral

Lista comentarios (filtros opcionales productId y sentiment)

GET
/api/comments/sentiment-summary
http://localhost:5196/api/comments/sentiment-summary

Totales y conteo por sentimiento

DELETE
/api/comments/{productId}
http://localhost:5196/api/comments/string

Extra: borra todos los comentarios de un mismo productId.
Para usar DELETE, pasa el productId del comentario; si no hay resultados, devuelve 404.


# 4) Pruebas Unitarias

TIP Detén todos los contenedores (docker compose down) y luego, desde la raíz del repositorio:

dotnet test tests/SentimentApi.Tests/SentimentApi.Tests.csproj


# 5) Posibles Errores
Si ves algo parecido a esto

2025-07-02 05:36:39.95 spid30s     Starting up database 'tempdb'.
2025-07-02 05:36:39.98 spid64s     0 transactions rolled back in database 'SentimentDb' (5:0). This is an informational message only. No user action is required.
2025-07-02 05:36:39.98 spid64s     Parallel redo is shutdown for database 'SentimentDb' with worker pool size [4].
2025-07-02 05:36:40.21 spid30s     The tempdb database has 8 data file(s).
2025-07-02 05:36:40.26 spid40s     The Service Broker endpoint is in disabled or stopped state.
2025-07-02 05:36:40.26 spid40s     The Database Mirroring endpoint is in disabled or stopped state.
2025-07-02 05:36:40.28 spid40s     Service Broker manager has started.
2025-07-02 05:36:40.31 spid29s     Recovery is complete. This is an informational message only. No user action is required.
api-1 exited with code 133


Basta con borrar las imagenes y containers de Docker luego correr de nuevo:

docker compose down
docker compose up --build

