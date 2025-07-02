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
# 0) Por si hubiese contenedores viejos
docker compose down

# 1) Construir imagen y levantar API + SQL Server
docker compose up --build

NOTA: la primera vez puede fallar por la Base de Datos favor de ir al  # 6)


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

TIP Detén todos los contenedores (docker compose down) y luego, desde la raíz del repo:

dotnet test tests/SentimentApi.Tests/SentimentApi.Tests.csproj


# 5) Posibles Errores

Basta con correr de nuevo:

docker compose down
docker compose up --build

