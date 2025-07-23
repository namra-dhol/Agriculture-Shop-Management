
CREATE TABLE ProductType (
    ProductTypeID INT PRIMARY KEY IDENTITY(1,1),
    TypeName NVARCHAR(100) NOT NULL
);

INSERT INTO ProductType (TypeName) VALUES
('Fertilizers'),
('Farm Equipment & Tools'),
('Animal Feed'),
('Seeds'),
('Fungicides');

CREATE TABLE Supplier (
    SupplierID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100),
    Contact NVARCHAR(20),
    Address NVARCHAR(200)
);

CREATE TABLE User(
    UserID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100),
    Email NVARCHAR(100),
    password NVARCHAR(100),
    Address NVARCHAR(200),
    Phone NVARCHAR(20),
    Role NVARCHAR(20) DEFAULT 'Customer'
);

CREATE TABLE Product (
    ProductID INT PRIMARY KEY IDENTITY(1,1),
    ProductName NVARCHAR(100),
    ProductTypeID INT NOT NULL,
    Unit NVARCHAR(20),
    Price DECIMAL(10,2),
    Stock INT,
    ProductImg NVARCHAR(200),
    SupplierID INT NOT NULL,
    FOREIGN KEY (ProductTypeID) REFERENCES ProductType(ProductTypeID),
    FOREIGN KEY (SupplierID) REFERENCES Supplier(SupplierID)
);

CREATE TABLE Purchase (
    PurchaseID INT PRIMARY KEY IDENTITY(1,1),
    SupplierID INT NOT NULL,
    PurchaseDate DATETIME NOT NULL,
    TotalAmount DECIMAL(10,2),
    FOREIGN KEY (UserID) REFERENCES User(UserID),
    FOREIGN KEY (SupplierID) REFERENCES Supplier(SupplierID)
);

CREATE TABLE PurchaseItem (
    PurchaseItemID INT PRIMARY KEY IDENTITY(1,1),
    PurchaseID INT NOT NULL,
    ProductID INT NOT NULL,
    Quantity INT,
    UnitPrice DECIMAL(10,2),
    FOREIGN KEY (PurchaseID) REFERENCES Purchase(PurchaseID),
    FOREIGN KEY (ProductID) REFERENCES Product(ProductID)
);

CREATE TABLE Sale (
    SaleID INT PRIMARY KEY IDENTITY(1,1),
    UserID NVARCHAR(450) NOT NULL,
    SaleDate DATETIME NOT NULL,
    TotalAmount DECIMAL(10,2),
   FOREIGN KEY (UserID) REFERENCES User(UserID)
);

CREATE TABLE SaleItem (
    SaleItemID INT PRIMARY KEY IDENTITY(1,1),
    SaleID INT NOT NULL,
    ProductID INT NOT NULL,
    Quantity INT,
    UnitPrice DECIMAL(10,2),
    FOREIGN KEY (SaleID) REFERENCES Sale(SaleID),
    FOREIGN KEY (ProductID) REFERENCES Product(ProductID)
);

CREATE TABLE Payment (
    PaymentID INT PRIMARY KEY IDENTITY(1,1),
    SaleID INT NOT NULL,
    PaidAmount DECIMAL(10,2),
    PaymentMethod NVARCHAR(50),
    PaymentDate DATETIME,
    FOREIGN KEY (SaleID) REFERENCES Sale(SaleID)
);
