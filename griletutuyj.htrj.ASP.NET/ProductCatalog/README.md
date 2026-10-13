# ProductCatalog

ASP.NET Core Web API homework project for status codes, response shaping, routing, Swagger UI, Scalar, Postman, and JSON enum serialization.

## Run

```powershell
dotnet run
```

Open:

- Scalar: `http://localhost:5085/scalar`
- Swagger UI: `http://localhost:5085/swagger`
- OpenAPI JSON for Postman import: `http://localhost:5085/openapi/v1.json`

## What was implemented

1. `GET /api/products?category=...` now compares categories with `StringComparison.OrdinalIgnoreCase`, the same way as `GET /api/products/category/{category}`.
2. `GET /api/products/price-range/{min}/{max}` returns `400 Bad Request` when `min > max`.
3. `PATCH /api/products/{id}/toggle-stock` toggles product availability and returns the updated product. Missing product returns `404 Not Found`.
4. `AvailabilityStatus` enum replaces the old boolean-style availability state:
   - `InStock`
   - `LowStock`
   - `OutOfStock`
5. JSON enum values are serialized as strings through `JsonStringEnumConverter`, so responses contain `"InStock"` instead of `0`.
6. Swagger UI is available at `/swagger`, Scalar is available at `/scalar`, and the OpenAPI document is available at `/openapi/v1.json`.

## Postman cycle report

Import `http://localhost:5085/openapi/v1.json` into Postman and run this cycle:

| Step | Method and URL | Expected status | Matches REST status table |
| --- | --- | --- | --- |
| Create product | `POST /api/products` | `201 Created` | Yes |
| Get product by id | `GET /api/products/{id}` | `200 OK` | Yes |
| Update product | `PUT /api/products/{id}` | `200 OK` | Yes |
| Toggle availability | `PATCH /api/products/{id}/toggle-stock` | `200 OK` | Yes |
| Delete product | `DELETE /api/products/{id}` | `204 No Content` | Yes |

Example `POST` body:

```json
{
  "name": "Laptop Stand",
  "brand": "Baseus",
  "category": "Аксесуари",
  "price": 799.99,
  "availabilityStatus": "InStock"
}
```

Example `PUT` body after creation, where `id` is the id returned by `POST`:

```json
{
  "id": 5,
  "name": "Laptop Stand Pro",
  "brand": "Baseus",
  "category": "Аксесуари",
  "price": 899.99,
  "availabilityStatus": "LowStock"
}
```

## Scalar and Swagger UI comparison

Scalar feels more like a modern API client: the request body editor and response area are visually cleaner, and the endpoint navigation is compact. It is convenient when you want to quickly test a request and inspect the response without too much page noise.

Swagger UI is more classic and direct: schemas, parameters, and the "Try it out" flow are very explicit. It is slightly heavier visually, but it is easy to see the generated request schema and status-code documentation for each action.

## Useful checks

- `GET /api/products?category=аксесуари` returns products even though the stored category is `Аксесуари`.
- `GET /api/products/price-range/500/100` returns `400 Bad Request`.
- `PATCH /api/products/1/toggle-stock` returns the same product with changed `availabilityStatus`.
