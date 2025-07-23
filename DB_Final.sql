drop database AgriShop

create database AgriShop

use AgriShop

CREATE TABLE ProductType (
    ProductTypeID INT PRIMARY KEY IDENTITY(1,1),
    TypeName NVARCHAR(100) NOT NULL
);


CREATE TABLE Supplier (
    SupplierID INT PRIMARY KEY IDENTITY(1,1),
    SupplierName NVARCHAR(100),
    Contact NVARCHAR(20),
    Address NVARCHAR(200)
);

CREATE TABLE [User] (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    UserName NVARCHAR(100),
    Email NVARCHAR(100),
    Password NVARCHAR(100),
    Address NVARCHAR(200),
    Phone NVARCHAR(20),
    Role NVARCHAR(20) DEFAULT 'Customer' -- 'Admin' or 'Customer'
);



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

CREATE TABLE ProductVariant (
    VariantID INT PRIMARY KEY IDENTITY(1,1),
    ProductID INT NOT NULL,
    Size NVARCHAR(50),        --  (kg, ml, etc.)
    Price DECIMAL(10,2),
    FOREIGN KEY (ProductID) REFERENCES Product(ProductID)
);


CREATE TABLE Purchase (
    PurchaseID INT PRIMARY KEY IDENTITY(1,1),
    SupplierID INT NOT NULL,
    UserID INT NOT NULL,                    
    PurchaseDate DATETIME NOT NULL,
    TotalAmount DECIMAL(10,2),
    FOREIGN KEY (SupplierID) REFERENCES Supplier(SupplierID),
    FOREIGN KEY (UserID) REFERENCES [User](UserID)
);



CREATE TABLE PurchaseItem (
    PurchaseItemID INT PRIMARY KEY IDENTITY(1,1),
    PurchaseID INT NOT NULL,
    VariantID INT NOT NULL,
    Quantity INT,
    UnitPrice DECIMAL(10,2),
    FOREIGN KEY (PurchaseID) REFERENCES Purchase(PurchaseID),
    FOREIGN KEY (VariantID) REFERENCES ProductVariant(VariantID)
);

CREATE TABLE Sale (
    SaleID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT NOT NULL, -- who made the purchase (customer)
    SaleDate DATETIME NOT NULL,
    TotalAmount DECIMAL(10,2),
    FOREIGN KEY (UserID) REFERENCES [User](UserID)
);


CREATE TABLE SaleItem (
    SaleItemID INT PRIMARY KEY IDENTITY(1,1),
    SaleID INT NOT NULL,
    VariantID INT NOT NULL,
    Quantity INT,
    UnitPrice DECIMAL(10,2),
    FOREIGN KEY (SaleID) REFERENCES Sale(SaleID),
    FOREIGN KEY (VariantID) REFERENCES ProductVariant(VariantID)
);




CREATE TABLE Payment (
    PaymentID INT PRIMARY KEY IDENTITY(1,1),
    SaleID INT NOT NULL,
    PaidAmount DECIMAL(10,2),
    PaymentMethod NVARCHAR(50),
    PaymentDate DATETIME,
    FOREIGN KEY (SaleID) REFERENCES Sale(SaleID)
);



--------Doubt ---------------------------------------------------
CREATE TABLE Purchase (
    PurchaseID INT PRIMARY KEY IDENTITY(1,1),
    SupplierID INT NOT NULL,
    ProductID INT NOT NULL,
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(10,2),
    TotalAmount AS (Quantity * UnitPrice) PERSISTED,
    PurchaseDate DATETIME NOT NULL,
    UserID INT NOT NULL,
    FOREIGN KEY (SupplierID) REFERENCES Supplier(SupplierID),
    FOREIGN KEY (ProductID) REFERENCES Product(ProductID),
    FOREIGN KEY (UserID) REFERENCES User(UserID)
);

CREATE TABLE Sale (
    SaleID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT NOT NULL,
    ProductID INT NOT NULL,
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(10,2),
    TotalAmount AS (Quantity * UnitPrice) PERSISTED,
    SaleDate DATETIME NOT NULL,
    PaymentMethod NVARCHAR(50),
    FOREIGN KEY (UserID) REFERENCES User(UserID),
    FOREIGN KEY (ProductID) REFERENCES Product(ProductID)
);
