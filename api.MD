To power your Agriculture Shop Management System, you’ll want a **RESTful Web API** layer that exposes all the CRUD and transaction operations, with **role-based authorization** (Admin vs Customer). Here’s a recommended outline of resources and endpoints:

---

## 1. Authentication & Users

| Endpoint               | Method | Roles          | Description                                       |
| ---------------------- | ------ | -------------- | ------------------------------------------------- |
| `/api/auth/register`   | POST   | Anonymous      | Register a new customer (assigns “Customer” role) |
| `/api/auth/login`      | POST   | Anonymous      | Log in (returns JWT or cookie)                    |
| `/api/auth/refresh`    | POST   | Authenticated  | Refresh access token                              |
| `/api/users/me`        | GET    | Customer/Admin | Get current user profile                          |
| `/api/users/me`        | PUT    | Customer/Admin | Update own profile (address, phone, etc.)         |
| `/api/users`           | GET    | Admin          | List all users                                    |
| `/api/users/{id}`      | GET    | Admin          | Get any user’s details                            |
| `/api/users/{id}/role` | PUT    | Admin          | Change user’s role (Customer ↔ Admin)             |

---

## 2. Product Types

| Endpoint                  | Method | Roles | Description                 |
| ------------------------- | ------ | ----- | --------------------------- |
| `/api/product-types`      | GET    | All   | List all categories         |
| `/api/product-types`      | POST   | Admin | Create new category         |
| `/api/product-types/{id}` | PUT    | Admin | Update category name        |
| `/api/product-types/{id}` | DELETE | Admin | Delete category (if unused) |

---

## 3. Suppliers

| Endpoint              | Method | Roles | Description                       |
| --------------------- | ------ | ----- | --------------------------------- |
| `/api/suppliers`      | GET    | Admin | List all suppliers                |
| `/api/suppliers`      | POST   | Admin | Add new supplier                  |
| `/api/suppliers/{id}` | GET    | Admin | Get supplier details              |
| `/api/suppliers/{id}` | PUT    | Admin | Update supplier                   |
| `/api/suppliers/{id}` | DELETE | Admin | Remove supplier (if no purchases) |

---

## 4. Products

| Endpoint             | Method | Roles | Description                                                  |
| -------------------- | ------ | ----- | ------------------------------------------------------------ |
| `/api/products`      | GET    | All   | Browse all products (supports filters: `?typeId=1&search=…`) |
| `/api/products/{id}` | GET    | All   | Get details of one product                                   |
| `/api/products`      | POST   | Admin | Create new product (multipart/form-data for `ProductImg`)    |
| `/api/products/{id}` | PUT    | Admin | Update product info & image                                  |
| `/api/products/{id}` | DELETE | Admin | Delete product (if no sales)                                 |

---

## 5. Purchases (Admin Restock)

| Endpoint              | Method | Roles | Description                                            |
| --------------------- | ------ | ----- | ------------------------------------------------------ |
| `/api/purchases`      | GET    | Admin | List all restock orders                                |
| `/api/purchases`      | POST   | Admin | Create new restock (body includes array of line items) |
| `/api/purchases/{id}` | GET    | Admin | Get details & items for one purchase                   |
| `/api/purchases/{id}` | PUT    | Admin | Edit a restock order (rare)                            |
| `/api/purchases/{id}` | DELETE | Admin | Cancel a restock                                       |

**Body schema (POST /api/purchases):**

```json
{
  "supplierId": 3,
  "purchaseDate": "2025-06-19T14:30:00",
  "items": [
    { "productId": 5, "quantity": 50, "unitPrice": 100.00 },
    { "productId": 7, "quantity": 20, "unitPrice": 250.00 }
  ]
}
```

---

## 6. Sales (Customer Orders)

| Endpoint          | Method | Roles          | Description                               |
| ----------------- | ------ | -------------- | ----------------------------------------- |
| `/api/sales`      | GET    | Customer/Admin | Customer: own orders; Admin: all orders   |
| `/api/sales`      | POST   | Customer       | Place a new order (body with items)       |
| `/api/sales/{id}` | GET    | Customer/Admin | Get one order’s details (items & payment) |

**Body schema (POST /api/sales):**

```json
{
  "items": [
    { "productId": 5, "quantity": 10 },
    { "productId": 8, "quantity": 2 }
  ],
  "paymentMethod": "UPI"
}
```

---

## 7. Payments

| Endpoint                      | Method | Roles          | Description                       |
| ----------------------------- | ------ | -------------- | --------------------------------- |
| `/api/sales/{saleId}/payment` | POST   | Customer       | Make a payment for that order     |
| `/api/payments`               | GET    | Admin          | List all payments (for reporting) |
| `/api/payments/{paymentId}`   | GET    | Customer/Admin | Get details of a specific payment |

---

## 8. File Upload (Product Image)

| Endpoint                   | Method | Roles | Description                     |
| -------------------------- | ------ | ----- | ------------------------------- |
| `/api/products/{id}/image` | POST   | Admin | Upload or replace product image |

Expect a **multipart/form-data** request with the file.

---

## 🔒 Security & Authorization

* Use **JWT Bearer tokens** (or cookies) issued by `/api/auth/login`.
* Protect endpoints with `[Authorize]` and `[Authorize(Roles="Admin")]` where needed.
* In controllers, use `User.Identity.Name` or `User.FindFirst(ClaimTypes.NameIdentifier)` to link to `AppUser.UserID`.

---

### ✅ Next Steps

1. **Implement controllers** matching these endpoints.
2. **Hook up services/repositories** to the EF Core models.
3. **Test** with Postman (or Swagger UI).
4. **Build front-end views** or a SPA that consumes these APIs.

Let me know if you’d like sample controller code or DTO definitions for any of these!
