Absolutely! Here's your **finalized database schema** for an **Agriculture Shop Management System** that supports product variants (like 500ml, 750ml, 1L, etc.), suppliers, users (with role-based access), and full sales/purchase/payment tracking.

---

## ✅ Finalized Database Schema

### 1. `ProductType`

```sql
CREATE TABLE ProductType (
    ProductTypeID INT PRIMARY KEY IDENTITY(1,1),
    TypeName NVARCHAR(100) NOT NULL
);
```

### 2. `Supplier`

```sql
CREATE TABLE Supplier (
    SupplierID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100),
    Contact NVARCHAR(20),
    Address NVARCHAR(200)
);
```

### 3. `User` (includes Admins and Customers)

```sql
CREATE TABLE [User] (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100),
    Email NVARCHAR(100),
    Password NVARCHAR(100),
    Address NVARCHAR(200),
    Phone NVARCHAR(20),
    Role NVARCHAR(20) DEFAULT 'Customer' -- 'Admin' or 'Customer'
);
```

### 4. `Product`

```sql
CREATE TABLE Product (
    ProductID INT PRIMARY KEY IDENTITY(1,1),
    ProductName NVARCHAR(100),
    ProductTypeID INT NOT NULL,
    SupplierID INT NOT NULL,
    Stock INT,
    ProductImg NVARCHAR(200),
    FOREIGN KEY (ProductTypeID) REFERENCES ProductType(ProductTypeID),
    FOREIGN KEY (SupplierID) REFERENCES Supplier(SupplierID)
);
```

### 5. `ProductVariant` (for sizes like 500ml, 1L, etc.)

```sql
CREATE TABLE ProductVariant (
    VariantID INT PRIMARY KEY IDENTITY(1,1),
    ProductID INT NOT NULL,
    Size NVARCHAR(50),         -- e.g., '500ml', '1L'
    Unit NVARCHAR(20),         -- optional (kg, ml, etc.)
    Price DECIMAL(10,2),
    FOREIGN KEY (ProductID) REFERENCES Product(ProductID)
);
```

### 6. `Purchase`

```sql
CREATE TABLE Purchase (
    PurchaseID INT PRIMARY KEY IDENTITY(1,1),
    SupplierID INT NOT NULL,
    UserID INT NOT NULL,                    -- who added the purchase
    PurchaseDate DATETIME NOT NULL,
    TotalAmount DECIMAL(10,2),
    FOREIGN KEY (SupplierID) REFERENCES Supplier(SupplierID),
    FOREIGN KEY (UserID) REFERENCES [User](UserID)
);
```

### 7. `PurchaseItem`

```sql
CREATE TABLE PurchaseItem (
    PurchaseItemID INT PRIMARY KEY IDENTITY(1,1),
    PurchaseID INT NOT NULL,
    VariantID INT NOT NULL,
    Quantity INT,
    UnitPrice DECIMAL(10,2),
    FOREIGN KEY (PurchaseID) REFERENCES Purchase(PurchaseID),
    FOREIGN KEY (VariantID) REFERENCES ProductVariant(VariantID)
);
```

### 8. `Sale`

```sql
CREATE TABLE Sale (
    SaleID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT NOT NULL, -- who made the purchase (customer)
    SaleDate DATETIME NOT NULL,
    TotalAmount DECIMAL(10,2),
    FOREIGN KEY (UserID) REFERENCES [User](UserID)
);
```

### 9. `SaleItem`

```sql
CREATE TABLE SaleItem (
    SaleItemID INT PRIMARY KEY IDENTITY(1,1),
    SaleID INT NOT NULL,
    VariantID INT NOT NULL,
    Quantity INT,
    UnitPrice DECIMAL(10,2),
    FOREIGN KEY (SaleID) REFERENCES Sale(SaleID),
    FOREIGN KEY (VariantID) REFERENCES ProductVariant(VariantID)
);
```

### 10. `Payment`

```sql
CREATE TABLE Payment (
    PaymentID INT PRIMARY KEY IDENTITY(1,1),
    SaleID INT NOT NULL,
    PaidAmount DECIMAL(10,2),
    PaymentMethod NVARCHAR(50),
    PaymentDate DATETIME,
    FOREIGN KEY (SaleID) REFERENCES Sale(SaleID)
);
```

---

## 🔄 Entity Interactions Overview

| Entity           | Interacts With                     | Description                                               |
| ---------------- | ---------------------------------- | --------------------------------------------------------- |
| `User`           | `Sale`, `Purchase`                 | A user can buy (`Sale`) and admins can record `Purchase`. |
| `ProductType`    | `Product`                          | Defines categories for products.                          |
| `Supplier`       | `Product`, `Purchase`              | Supplies products, linked when buying stock.              |
| `Product`        | `ProductVariant`                   | A product has multiple variants (sizes/units).            |
| `ProductVariant` | `SaleItem`, `PurchaseItem`         | Variants are sold and purchased.                          |
| `Purchase`       | `PurchaseItem`, `User`, `Supplier` | Records stock addition by admin.                          |
| `Sale`           | `SaleItem`, `User`, `Payment`      | Records customer purchases.                               |
| `SaleItem`       | `ProductVariant`, `Sale`           | Lists products sold in each sale.                         |
| `PurchaseItem`   | `ProductVariant`, `Purchase`       | Lists products purchased in each purchase.                |
| `Payment`        | `Sale`                             | Records payments for sales.                               |

---

## 🧠 Use Cases

### 1. **Customer Purchase Flow**

* Customer (`User`) browses `Product` and selects a `ProductVariant`.
* System creates `Sale` and `SaleItem`.
* Payment is stored in `Payment`.

### 2. **Admin Purchase Stock Flow**

* Admin logs in (`User.Role = Admin`)
* Adds a `Purchase` with a `Supplier`
* Adds multiple `PurchaseItem` entries for stock-in using `ProductVariant`.

### 3. **Product Management**

* Admin adds `Product` → adds multiple `ProductVariant` (500ml, 1L, etc.)

---

## 📊 Optional Future Extensions

| Feature              | Table/Modification                             |
| -------------------- | ---------------------------------------------- |
| Discounts/Offers     | Add `Discount` table and link to `SaleItem`    |
| GST/Tax tracking     | Add `Tax` column to `SaleItem`, `PurchaseItem` |
| Category/Subcategory | Use a hierarchy in `ProductType` if needed     |
| Soft Deletes / Audit | Add `IsDeleted`, `CreatedAt`, `UpdatedAt`      |

---

Let me know if you want:

* **ER Diagram**
* **ASP.NET MVC/EF Core model classes**
* **Seeder SQL or EF migration**
* **Frontend integration plan**

Just say the word!
