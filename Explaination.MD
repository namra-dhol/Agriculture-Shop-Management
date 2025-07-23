Of course! Here’s a **fully formatted, easy-to-understand version of your notes** with all the info intact and clearly explained. You can use this as your study/reference notes. ✨

---

## 📋 1. **`AspNetUsers` & Identity Tables**

> **Purpose:** Stores all user accounts, credentials, and role mappings
> **Roles Involved:** Admin & Customer

| Table               | Role Usage                                         | Operations (CRUD)                            |
| ------------------- | -------------------------------------------------- | -------------------------------------------- |
| **AspNetUsers**     | Both roles login/register                          | • C (**register**) • R (**profile look-up**) |
| **AspNetRoles**     | System seed only                                   | • R (**list roles**)                         |
| **AspNetUserRoles** | Assign roles (Admin seeds; Customer auto-assigned) | • C (**assign role**) • R (**check role**)   |

---

## 🧑‍💼 2. **`AppUser`**

> **Purpose:** Extended profile info (Name, Email, Address, Phone, Role)
> **Roles Involved:** Both

| Operation  | Admin                                    | Customer                     |
| ---------- | ---------------------------------------- | ---------------------------- |
| **Create** | Seed default admin in `DbSeeder`         | Auto-created on register     |
| **Read**   | View any user profile                    | View own profile             |
| **Update** | Edit any user profile (support purposes) | Edit own address/contact     |
| **Delete** | Soft-delete or deactivate any account    | n/a (self-deactivation only) |

---

## 🏷️ 3. **`ProductType`**

> **Purpose:** Categories (e.g. Fertilizers, Seeds, etc.)
> **Roles Involved:** Admin only

| Operation  | Admin                          | Customer                  |
| ---------- | ------------------------------ | ------------------------- |
| **Create** | Add new product categories     | n/a                       |
| **Read**   | List categories for management | Browse product categories |
| **Update** | Rename or retire categories    | n/a                       |
| **Delete** | Remove unused categories       | n/a                       |

---

## 🏭 4. **`Supplier`**

> **Purpose:** Vendors you buy stock from
> **Roles Involved:** Admin only

| Operation  | Admin                                          |
| ---------- | ---------------------------------------------- |
| **Create** | Add new supplier record                        |
| **Read**   | View/search suppliers during purchase creation |
| **Update** | Edit supplier contact/address                  |
| **Delete** | Remove supplier (only if no past purchases)    |

---

## 📦 5. **`Product`**

> **Purpose:** Store all shop inventory items (with optional image)
> **Roles Involved:** Admin (manage), Customer (view)

| Operation  | Admin                                  | Customer        |
| ---------- | -------------------------------------- | --------------- |
| **Create** | Add new products with `ProductType`    | n/a             |
| **Read**   | List/search products, generate reports | Browse products |
| **Update** | Adjust price, stock, image             | n/a             |
| **Delete** | Remove products with no sales history  | n/a             |

---

## 🛒 6. **`Purchase` & `PurchaseItem`**

> **Purpose:** Record shop’s restocks from suppliers
> **Roles Involved:** Admin only

| Table            | Operation     | Logic                                                      |
| ---------------- | ------------- | ---------------------------------------------------------- |
| **Purchase**     | Create        | Save `UserID` (Admin), `SupplierID`, `Date`, `TotalAmount` |
|                  | Read          | List all purchases; filter by date/supplier                |
|                  | Update/Delete | Rare — correct mistakes only                               |
| **PurchaseItem** | Create        | Save each purchased product (`ProductID`, `Qty`, `Price`)  |
|                  | Read          | View purchased items for each `PurchaseID`                 |

---

## 💳 7. **`Sale`, `SaleItem`, & `Payment`**

> **Purpose:** Track customers’ orders and payments
> **Roles Involved:** Customer & Admin

| Table        | Operation | Logic                                                                  |
| ------------ | --------- | ---------------------------------------------------------------------- |
| **Sale**     | Create    | Generated at checkout (`UserID`, `Date`, `TotalAmount`)                |
|              | Read      | Customer: own orders; Admin: all orders                                |
| **SaleItem** | Create    | Records each purchased product (`SaleID`, `ProductID`, `Qty`, `Price`) |
|              | Read      | Show line-items per order                                              |
| **Payment**  | Create    | Save payment details (`SaleID`, `Amount`, `Method`, `Date`)            |
|              | Read      | Customer: own invoices; Admin: reports                                 |

---

## 🔄 Workflow Summaries

### 🧑‍💼 **Admin Restocking**

1. Creates a **Purchase** record (`UserID`=Admin, `SupplierID`, `Date`, `TotalAmount`).
2. Adds multiple **PurchaseItem** entries (`ProductID`, `Qty`, `Price`).

---

### 🛍️ **Customer Shopping**

1. Browse **Product** catalog, filter by **ProductType**.
2. Checkout → new **Sale** record (`UserID`=Customer).
3. Add multiple **SaleItem** entries (`ProductID`, `Qty`, `UnitPrice`).
4. Create a **Payment** record (`Amount`, `Method`).

---

### 🧑‍⚖️ **Role-Based Access**

| Role         | Actions                                                                                                          |
| ------------ | ---------------------------------------------------------------------------------------------------------------- |
| **Admin**    | Manage Products, Suppliers, ProductTypes, Purchases, view all Sales & Payments                                   |
| **Customer** | Browse Products & ProductTypes, Place orders (**Sale**), view own orders & make **Payments**, manage own profile |

---