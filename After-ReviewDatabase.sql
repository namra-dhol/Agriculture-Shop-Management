create database AgriShop

use AgriShop
-- User table stays the same
CREATE TABLE [User] (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    UserName NVARCHAR(100),
    Email NVARCHAR(100),
    Password NVARCHAR(100),
    Address NVARCHAR(200),
    Phone NVARCHAR(20),
    Role NVARCHAR(20) DEFAULT 'Customer' -- 'Admin' or 'Customer'
);

-- ProductType with UserID and timestamps
CREATE TABLE ProductType (
    ProductTypeID INT PRIMARY KEY IDENTITY(1,1),
    TypeName NVARCHAR(100) NOT NULL,
    UserID INT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    ModifiedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserID) REFERENCES [User](UserID)
);

-- Supplier with UserID and timestamps
CREATE TABLE Supplier (
    SupplierID INT PRIMARY KEY IDENTITY(1,1),
    SupplierName NVARCHAR(100),
    Contact NVARCHAR(20),
    Address NVARCHAR(200),
    UserID INT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    ModifiedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserID) REFERENCES [User](UserID)
);

-- Product with UserID and timestamps
CREATE TABLE Product (
    ProductID INT PRIMARY KEY IDENTITY(1,1),
    ProductName NVARCHAR(100),
    ProductTypeID INT NOT NULL,
    SupplierID INT NOT NULL,
    Stock INT,
    ProductImg NVARCHAR(200),
    UserID INT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    ModifiedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (ProductTypeID) REFERENCES ProductType(ProductTypeID),
    FOREIGN KEY (SupplierID) REFERENCES Supplier(SupplierID),
    FOREIGN KEY (UserID) REFERENCES [User](UserID)
);

-- ProductVariant with UserID and timestamps
CREATE TABLE ProductVariant (
    VariantID INT PRIMARY KEY IDENTITY(1,1),
    ProductID INT NOT NULL,
    Size NVARCHAR(50),
    Price DECIMAL(10,2),
    UserID INT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    ModifiedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (ProductID) REFERENCES Product(ProductID),
    FOREIGN KEY (UserID) REFERENCES [User](UserID)
);

-- ContactForm or Feedback table for inquiries
CREATE TABLE ContactForm (
    ContactID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT NULL,  -- optional; can also allow guest inquiries
    Name NVARCHAR(100),
    Email NVARCHAR(100),
    Message NVARCHAR(MAX),
    SubmittedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserID) REFERENCES [User](UserID)
);


INSERT INTO [User] (UserName, Email, Password, Address, Phone, Role)
VALUES
('Alice Admin', 'alice.admin@example.com', 'hashedpassword1', '123 Admin St', '123-456-7890', 'Admin'),
('Bob Customer', 'bob.customer@example.com', 'hashedpassword2', '456 Customer Rd', '234-567-8901', 'Customer'),
('Charlie Customer', 'charlie.customer@example.com', 'hashedpassword3', '789 Shopper Ln', '345-678-9012', 'Customer'),
('Diana Admin', 'diana.admin@example.com', 'hashedpassword4', '987 Admin Ave', '456-789-0123', 'Admin');

INSERT INTO ProductType (TypeName, UserID) VALUES
('Beverages', 1),
('Snacks', 1),
('Dairy', 1),
('Produce', 1),
('Bakery', 1);

INSERT INTO Supplier (SupplierName, Contact, Address, UserID) VALUES
('Fresh Farms Ltd.', '987-654-3210', '12 Country Rd', 1),
('Tasty Treats Co.', '876-543-2109', '99 Snack St', 1),
('Milk House', '765-432-1098', '45 Dairy Ln', 1),
('Healthy Harvest', '654-321-0987', '77 Market Rd', 1);







-- Find the IDs first:
-- ProductTypeID: 1(Beverages),2(Snacks),3(Dairy),4(Produce),5(Bakery)
-- SupplierID: 1(Fresh Farms),2(Tasty Treats),3(Milk House),4(Healthy Harvest)
INSERT INTO Product (ProductName, ProductTypeID, SupplierID, Stock, ProductImg, UserID) VALUES
('Orange', 1, 1, 50, 'images/orange-juice.jpg', 1),
('Chocolate Bar', 2, 2, 200, 'images/chocolate-bar.jpg', 1),
('Cheddar Cheese', 3, 3, 75, 'images/cheddar-cheese.jpg', 1),
('Red Apples', 4, 4, 120, 'images/red-apples.jpg', 1),
('Whole Wheat Bread', 5, 1, 60, 'images/whole-wheat-bread.jpg', 1);

-- Variants for Orange Juice (ProductID = 1)
INSERT INTO ProductVariant (ProductID, Size, Price, UserID) VALUES
(1, '500ml', 2.49, 1),
(1, '1L', 4.99, 1);


																																					


(3, '200g', 3.99, 1),
(3, '500g', 7.99, 1);

-- Variants for Red Apples (ProductID = 4)
INSERT INTO ProductVariant (ProductID, Size, Price, UserID) VALUES
(4, '1kg', 2.99, 1),
(4, '2kg', 5.49, 1);

-- Variants for Whole Wheat Bread (ProductID = 5)
INSERT INTO ProductVariant (ProductID, Size, Price, UserID) VALUES
(5, '400g', 1.99, 1);


INSERT INTO ContactForm (UserID, Name, Email, Message) VALUES
(2, 'Bob Customer', 'bob.customer@example.com', 'Do you have almond milk in stock?'),
(NULL, 'Guest User', 'guest@example.com', 'Can you add gluten-free products?');


