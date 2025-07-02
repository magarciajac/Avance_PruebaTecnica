# Sentiment API – Avance Prueba Técnica

Pequeña API construida con **ASP.NET Core 8.0** que clasifica comentarios como *Positive*, *Negative* o *Neutral* y los guarda en **SQL Server** ejecutado en contenedor Docker.

---

## 1. Ejecutar todo con Docker Compose

```bash
docker compose up --build
```

* `--build` fuerza la compilación de la imagen de la API.  
* **docker‑compose.yml** levanta **dos servicios** al mismo tiempo:  
  1. **`sentiment-api`** – la aplicación ASP.NET Core.  
  2. **`sqlserver`** – SQL Server 2022 con volumen persistente.

Cuando el comando termina de levantar los contenedores:

* La API está escuchando en **http://localhost:5196** (puerto mapeado 5196→80).  
* La base de datos está disponible en **localhost:1433** con las credenciales definidas en el compose file.

---

## 2. Endpoints principales

| Verbo | Ruta                             | Descripción                                           |
|-------|----------------------------------|-------------------------------------------------------|
| GET   | `/comments`                      | Devuelve todos los comentarios ordenados por fecha    |
| GET   | `/comments/{productId}`          | Devuelve comentarios filtrados por `ProductId`        |
| POST  | `/comments`                      | Crea un comentario, lo clasifica y lo guarda          |
| DELETE| `/comments/{id}`                 | Elimina un comentario por `Id`                        |

*(Estos endpoints están definidos en **Program.cs** para mantener el ejemplo sencillo sin controladores.)*

---

## 3. Probar la API con Swagger UI

Una vez que los contenedores estén arriba, abre tu navegador en:

```
http://localhost:5196/swagger
```

Swagger UI permite invocar cada endpoint, ver los modelos de request/response y probar rápidamente la clasificación de sentimientos.

---

## 4. Ejecutar pruebas unitarias (opcional)

Dentro de la carpeta **tests/** hay un proyecto `SentimentApi.Tests` con pruebas de `CommentService`. Ejecuta:

```bash
dotnet test
```

---

¡Listo! Con esto tendrás la API y la base de datos corriendo, además de documentación interactiva para probarla.