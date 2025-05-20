-- Create Sizes first
INSERT INTO dbo.Sizes (
    SizeValue,
    IsDeleted,
    CreatedAt,
    UpdatedAt
)
VALUES
(38, 0, GETDATE(), GETDATE()),
(39, 0, GETDATE(), GETDATE()),
(40, 0, GETDATE(), GETDATE()),
(41, 0, GETDATE(), GETDATE()),
(42, 0, GETDATE(), GETDATE()),
(43, 0, GETDATE(), GETDATE()),
(44, 0, GETDATE(), GETDATE()),
(45, 0, GETDATE(), GETDATE());

-- Create Users first to establish user IDs
INSERT INTO dbo.Users (
    Name,
    Email,
    PasswordHash,
    UserRole,  -- Using UserRole enum: Customer = 0, Manufacturer = 1, Designer = 2, Admin = 3
    IsDeleted,
    CreatedAt,
    UpdatedAt,
    Balance,
    IsBanned
)
VALUES
(N'admin', N'admin@gmail.com', N'AEHByHItJS7Mdof7dY0Hy1Yxz3LpQVlgF++R+hCGfR9GOhXEBmToolsH7X9XwDBXfA==', 3, 0, GETDATE(), GETDATE(), 0, 0),  -- Admin
(N'designer', N'designer@gmail.com', N'AEHByHItJS7Mdof7dY0Hy1Yxz3LpQVlgF++R+hCGfR9GOhXEBmToolsH7X9XwDBXfA==', 2, 0, GETDATE(), GETDATE(), 0, 0),  -- Designer
(N'manufacturer', N'manufacturer@gmail.com', N'AEHByHItJS7Mdof7dY0Hy1Yxz3LpQVlgF++R+hCGfR9GOhXEBmToolsH7X9XwDBXfA==', 1, 0, GETDATE(), GETDATE(), 0, 0),  -- Manufacturer
(N'john_doe', N'john@gmail.com', N'AEHByHItJS7Mdof7dY0Hy1Yxz3LpQVlgF++R+hCGfR9GOhXEBmToolsH7X9XwDBXfA==', 0, 0, GETDATE(), GETDATE(), 0, 0),     -- Customer
(N'jane_smith', N'jane@gmail.com', N'AEHByHItJS7Mdof7dY0Hy1Yxz3LpQVlgF++R+hCGfR9GOhXEBmToolsH7X9XwDBXfA==', 0, 0, GETDATE(), GETDATE(), 0, 0),   -- Customer
(N'mike_jackson', N'mike@gmail.com', N'AEHByHItJS7Mdof7dY0Hy1Yxz3LpQVlgF++R+hCGfR9GOhXEBmToolsH7X9XwDBXfA==', 0, 0, GETDATE(), GETDATE(), 0, 0), -- Customer
(N'sarah_connor', N'sarah@gmail.com', N'AEHByHItJS7Mdof7dY0Hy1Yxz3LpQVlgF++R+hCGfR9GOhXEBmToolsH7X9XwDBXfA==', 0, 0, GETDATE(), GETDATE(), 0, 0),-- Customer
(N'alex_wilson', N'alex@gmail.com', N'AEHByHItJS7Mdof7dY0Hy1Yxz3LpQVlgF++R+hCGfR9GOhXEBmToolsH7X9XwDBXfA==', 0, 0, GETDATE(), GETDATE(), 0, 0);  -- Customer

-- Get the manufacturer user ID
DECLARE @ManufacturerUserID BIGINT;
SELECT @ManufacturerUserID = Id FROM dbo.Users WHERE Email = N'manufacturer@gmail.com';

-- Create Manufacturers
INSERT INTO dbo.Manufacturers (
    UserId,
    Description,
    CommissionRate,
    Status,  -- ManufacturerStatus enum: Active = 1
    IsDeleted,
    CreatedAt,
    UpdatedAt
)
VALUES
(@ManufacturerUserID, N'Premium Shoe Manufacturer', 10.0, 1, 0, GETDATE(), GETDATE())  -- Active manufacturer

-- Get Manufacturer IDs for later use
DECLARE @Manufacturer1ID BIGINT;
SELECT TOP 1 @Manufacturer1ID = Id FROM dbo.Manufacturers WHERE Description = N'Premium Shoe Manufacturer';

-- Get the designer user ID that already exists in your script
DECLARE @DesignerUserID BIGINT;
SELECT @DesignerUserID = Id FROM dbo.Users WHERE Email = N'designer@gmail.com';

-- Create Designer profile
INSERT INTO dbo.Designers (
    UserId,
    Description,
    CommissionRate,
    Rating,
    Status,  -- DesignerStatus enum: Active = 1
    IsDeleted,
    CreatedAt,
    UpdatedAt
)
VALUES
(@DesignerUserID, N'Professional footwear designer with focus on modern aesthetics', 15.0, 4.8, 1, 0, GETDATE(), GETDATE());  -- Active designer

-- Create Design Templates
INSERT INTO dbo.CustomShoeDesignTemplates (
    Name,
    Description,
    Price,
    [2DImageUrl],
    IsDeleted,
    CreatedAt,
    UpdatedAt
)
VALUES
(N'Standard Sneaker', N'A classic sneaker design template', 8999, N'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSDqac8qPp6UPCFaPYroGCkAmNeJfp6sAcAzg&s', 0, GETDATE(), GETDATE()),
(N'High-Top', N'A high-top sneaker template', 9999, N'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSDqac8qPp6UPCFaPYroGCkAmNeJfp6sAcAzg&s', 0, GETDATE(), GETDATE()),
(N'Slip-On', N'A casual slip-on shoe template', 7999, N'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSDqac8qPp6UPCFaPYroGCkAmNeJfp6sAcAzg&s', 0, GETDATE(), GETDATE());

-- Get the User IDs first
DECLARE @User1ID BIGINT, @User2ID BIGINT, @User3ID BIGINT, @User4ID BIGINT, @User5ID BIGINT;
SELECT @User1ID = Id FROM dbo.Users WHERE Name = N'john_doe';
SELECT @User2ID = Id FROM dbo.Users WHERE Name = N'jane_smith';
SELECT @User3ID = Id FROM dbo.Users WHERE Name = N'mike_jackson';
SELECT @User4ID = Id FROM dbo.Users WHERE Name = N'sarah_connor';
SELECT @User5ID = Id FROM dbo.Users WHERE Name = N'alex_wilson';

-- Create Textures with proper user reference
-- Using TextureStatus enum: Public = 1
INSERT INTO dbo.Textures (
    UserId,
    Prompt,
    IsDeleted,
    ImageUrl,
    CreatedAt,
    UpdatedAt
)
VALUES
(@User1ID, N'Premium leather texture', 0, N'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSDqac8qPp6UPCFaPYroGCkAmNeJfp6sAcAzg&s', GETDATE(), GETDATE()),
(@User1ID, N'Durable canvas texture', 0, N'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSDqac8qPp6UPCFaPYroGCkAmNeJfp6sAcAzg&s', GETDATE(), GETDATE()),
(@User1ID, N'Stylish denim texture', 0, N'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSDqac8qPp6UPCFaPYroGCkAmNeJfp6sAcAzg&s', GETDATE(), GETDATE()),
(@User1ID, N'Soft suede texture', 0, N'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSDqac8qPp6UPCFaPYroGCkAmNeJfp6sAcAzg&s', GETDATE(), GETDATE()),
(@User1ID, N'Breathable mesh texture', 0, N'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSDqac8qPp6UPCFaPYroGCkAmNeJfp6sAcAzg&s', GETDATE(), GETDATE());

-- Insert ShippingInfos with proper user IDs
INSERT INTO dbo.ShippingInfos (
    UserId,
    StreetAddress,
    Ward,
    District,
    City,
    Country,
    PhoneNumber,
    ContactNumber,
    IsDefault,
    IsDeleted,
    CreatedAt,
    UpdatedAt
)
VALUES
(@User1ID, N'123 Main St', N'Ward 1', N'District 1', N'New York', N'USA', N'1234567890', N'1234567890', 1, 0, GETDATE(), GETDATE()),
(@User2ID, N'456 Elm St', N'Ward 2', N'District 2', N'Los Angeles', N'USA', N'2345678901', N'2345678901', 1, 0, GETDATE(), GETDATE()),
(@User3ID, N'789 Oak St', N'Ward 3', N'District 3', N'Chicago', N'USA', N'3456789012', N'3456789012', 1, 0, GETDATE(), GETDATE()),
(@User4ID, N'101 Pine St', N'Ward 4', N'District 4', N'Miami', N'USA', N'4567890123', N'4567890123', 1, 0, GETDATE(), GETDATE()),
(@User5ID, N'202 Maple St', N'Ward 5', N'District 5', N'Seattle', N'USA', N'5678901234', N'5678901234', 1, 0, GETDATE(), GETDATE()),
(@User1ID, N'500 Business Ave', N'Ward 6', N'District 6', N'New York', N'USA', N'1234567890', N'1234567890', 0, 0, GETDATE(), GETDATE()),
(@User2ID, N'600 Office Blvd', N'Ward 7', N'District 7', N'Los Angeles', N'USA', N'2345678901', N'2345678901', 0, 0, GETDATE(), GETDATE()),
(@User3ID, N'700 Corp St', N'Ward 8', N'District 8', N'Chicago', N'USA', N'3456789012', N'3456789012', 0, 0, GETDATE(), GETDATE()),
(@User4ID, N'800 Work Rd', N'Ward 9', N'District 9', N'Miami', N'USA', N'4567890123', N'4567890123', 0, 0, GETDATE(), GETDATE()),
(@User5ID, N'900 Job Lane', N'Ward 10', N'District 10', N'Seattle', N'USA', N'5678901234', N'5678901234', 0, 0, GETDATE(), GETDATE());

-- Get template IDs
DECLARE @Template1ID BIGINT, @Template2ID BIGINT, @Template3ID BIGINT;
SELECT @Template1ID = Id FROM dbo.CustomShoeDesignTemplates WHERE Name = N'Standard Sneaker';
SELECT @Template2ID = Id FROM dbo.CustomShoeDesignTemplates WHERE Name = N'High-Top';
SELECT @Template3ID = Id FROM dbo.CustomShoeDesignTemplates WHERE Name = N'Slip-On';

-- Get Size IDs
DECLARE @Size38ID BIGINT, @Size39ID BIGINT, @Size40ID BIGINT, @Size41ID BIGINT, @Size42ID BIGINT, @Size43ID BIGINT, @Size44ID BIGINT, @Size45ID BIGINT;
SELECT @Size38ID = Id FROM dbo.Sizes WHERE SizeValue = 38;
SELECT @Size39ID = Id FROM dbo.Sizes WHERE SizeValue = 39;
SELECT @Size40ID = Id FROM dbo.Sizes WHERE SizeValue = 40;
SELECT @Size41ID = Id FROM dbo.Sizes WHERE SizeValue = 41;
SELECT @Size42ID = Id FROM dbo.Sizes WHERE SizeValue = 42;
SELECT @Size43ID = Id FROM dbo.Sizes WHERE SizeValue = 43;
SELECT @Size44ID = Id FROM dbo.Sizes WHERE SizeValue = 44;
SELECT @Size45ID = Id FROM dbo.Sizes WHERE SizeValue = 45;

-- Insert Custom Shoe Designs with proper references
-- Using CustomShoeDesignStatus enum: Public = 1
INSERT INTO dbo.CustomShoeDesigns (
    UserId,
    CustomShoeDesignTemplateId,
    DesignData,
    Name,
    Description,
    Status,  -- CustomShoeDesignStatus.Public = 1
    DesignerMarkup,
    TotalAmount,
    IsDeleted,
    CreatedAt,
    UpdatedAt
)
VALUES
-- Design 1: Classic Red Sneaker
(@User1ID, @Template1ID, N'{"baseColor":"#ff0000","accent":"#ffffff","laces":"#000000","sole":"#ffffff"}', N'Classic Red Sneaker', N'A timeless red sneaker design', 1, 1500, 14999, 0, GETDATE(), GETDATE()),
-- Design 2: Blue Ocean Theme
(@User2ID, @Template2ID, N'{"baseColor":"#0077be","accent":"#ffffff","laces":"#003366","sole":"#e0e0e0"}', N'Blue Ocean Theme', N'Inspired by ocean waves', 1, 2000, 15999, 0, GETDATE(), GETDATE()),
-- Design 3: Urban Black
(@User1ID, @Template3ID, N'{"baseColor":"#000000","accent":"#333333","laces":"#555555","sole":"#ffffff"}', N'Urban Black', N'Sleek and minimalist design', 1, 1000, 13999, 0, GETDATE(), GETDATE()),
-- Design 4: Summer Vibes
(@User3ID, @Template1ID, N'{"baseColor":"#ffcc00","accent":"#ff6600","laces":"#ffffff","sole":"#eeeeee"}', N'Summer Vibes', N'Bright and cheerful summer design', 1, 2500, 16999, 0, GETDATE(), GETDATE()),
-- Design 5: Forest Green
(@User2ID, @Template2ID, N'{"baseColor":"#228B22","accent":"#8B4513","laces":"#FFFFFF","sole":"#D2B48C"}', N'Forest Green', N'Nature-inspired high-top', 1, 1800, 15499, 0, GETDATE(), GETDATE()),
-- Design 6: Purple Haze
(@User4ID, @Template3ID, N'{"baseColor":"#800080","accent":"#BA55D3","laces":"#E6E6FA","sole":"#FFFFFF"}', N'Purple Haze', N'Bold and vibrant purple design', 1, 2200, 16499, 0, GETDATE(), GETDATE()),
-- Design 7: Neon Future
(@User3ID, @Template1ID, N'{"baseColor":"#39FF14","accent":"#FF10F0","laces":"#FFFFFF","sole":"#000000"}', N'Neon Future', N'Eye-catching neon design', 1, 3000, 17999, 0, GETDATE(), GETDATE()),
-- Design 8: Monochrome White
(@User5ID, @Template2ID, N'{"baseColor":"#FFFFFF","accent":"#F5F5F5","laces":"#EEEEEE","sole":"#F8F8F8"}', N'Monochrome White', N'Clean and elegant all-white design', 1, 1500, 14999, 0, GETDATE(), GETDATE()),
-- Design 9: Sunset Orange
(@User1ID, @Template3ID, N'{"baseColor":"#FF8C00","accent":"#FF4500","laces":"#FFFF00","sole":"#EEEEEE"}', N'Sunset Orange', N'Warm sunset-inspired colors', 1, 2000, 15999, 0, GETDATE(), GETDATE()),
-- Design 10: Minimal Gray
(@User4ID, @Template1ID, N'{"baseColor":"#808080","accent":"#A9A9A9","laces":"#D3D3D3","sole":"#FFFFFF"}', N'Minimal Gray', N'Subtle and sophisticated gray tones', 1, 1200, 14499, 0, GETDATE(), GETDATE());

-- Get Design IDs for the rest of the inserts
DECLARE @Design1ID BIGINT, @Design2ID BIGINT, @Design3ID BIGINT, @Design4ID BIGINT, @Design5ID BIGINT;
DECLARE @Design6ID BIGINT, @Design7ID BIGINT, @Design8ID BIGINT, @Design9ID BIGINT, @Design10ID BIGINT;

-- Find the ID of each design based on name
SELECT @Design1ID = Id FROM dbo.CustomShoeDesigns WHERE Name = N'Classic Red Sneaker';
SELECT @Design2ID = Id FROM dbo.CustomShoeDesigns WHERE Name = N'Blue Ocean Theme';
SELECT @Design3ID = Id FROM dbo.CustomShoeDesigns WHERE Name = N'Urban Black';
SELECT @Design4ID = Id FROM dbo.CustomShoeDesigns WHERE Name = N'Summer Vibes';
SELECT @Design5ID = Id FROM dbo.CustomShoeDesigns WHERE Name = N'Forest Green';
SELECT @Design6ID = Id FROM dbo.CustomShoeDesigns WHERE Name = N'Purple Haze';
SELECT @Design7ID = Id FROM dbo.CustomShoeDesigns WHERE Name = N'Neon Future';
SELECT @Design8ID = Id FROM dbo.CustomShoeDesigns WHERE Name = N'Monochrome White';
SELECT @Design9ID = Id FROM dbo.CustomShoeDesigns WHERE Name = N'Sunset Orange';
SELECT @Design10ID = Id FROM dbo.CustomShoeDesigns WHERE Name = N'Minimal Gray';

-- Get Texture IDs
DECLARE @Texture1ID BIGINT, @Texture2ID BIGINT, @Texture3ID BIGINT, @Texture4ID BIGINT, @Texture5ID BIGINT;
SELECT TOP 1 @Texture1ID = Id FROM dbo.Textures WHERE Prompt = N'Premium leather texture';
SELECT TOP 1 @Texture2ID = Id FROM dbo.Textures WHERE Prompt = N'Durable canvas texture';
SELECT TOP 1 @Texture3ID = Id FROM dbo.Textures WHERE Prompt = N'Stylish denim texture';
SELECT TOP 1 @Texture4ID = Id FROM dbo.Textures WHERE Prompt = N'Soft suede texture';
SELECT TOP 1 @Texture5ID = Id FROM dbo.Textures WHERE Prompt = N'Breathable mesh texture';

-- Get ShippingInfo IDs
DECLARE @ShippingInfo1ID BIGINT, @ShippingInfo2ID BIGINT, @ShippingInfo3ID BIGINT, @ShippingInfo4ID BIGINT, @ShippingInfo5ID BIGINT;
SELECT TOP 1 @ShippingInfo1ID = Id FROM dbo.ShippingInfos WHERE UserId = @User1ID AND IsDefault = 1;
SELECT TOP 1 @ShippingInfo2ID = Id FROM dbo.ShippingInfos WHERE UserId = @User2ID AND IsDefault = 1;
SELECT TOP 1 @ShippingInfo3ID = Id FROM dbo.ShippingInfos WHERE UserId = @User3ID AND IsDefault = 1;
SELECT TOP 1 @ShippingInfo4ID = Id FROM dbo.ShippingInfos WHERE UserId = @User4ID AND IsDefault = 1;
SELECT TOP 1 @ShippingInfo5ID = Id FROM dbo.ShippingInfos WHERE UserId = @User5ID AND IsDefault = 1;

-- Create Orders with proper references
-- Using OrderStatus enum: Confirmed = 1
-- Using OrderShippingStatus enum: Delivered = 2
INSERT INTO dbo.Orders (
    UserId,
    ShippingInfoId,
    TotalPrice,
    AmountPaid,
    Status,  -- OrderStatus.Confirmed = 1
    ShippingStatus, -- OrderShippingStatus.Delivered = 2
    IsDeleted,
    CreatedAt,
    UpdatedAt
)
VALUES
-- Order 1: User 1's order
(@User1ID, @ShippingInfo1ID, 15999, 15999, 1, 2, 0, DATEADD(DAY, -10, GETDATE()), DATEADD(DAY, -10, GETDATE())),
-- Order 2: User 2's order
(@User2ID, @ShippingInfo2ID, 14499, 14499, 1, 2, 0, DATEADD(DAY, -8, GETDATE()), DATEADD(DAY, -8, GETDATE())),
-- Order 3: User 3's order
(@User3ID, @ShippingInfo3ID, 33998, 33998, 1, 2, 0, DATEADD(DAY, -5, GETDATE()), DATEADD(DAY, -5, GETDATE())),
-- Order 4: User 4's order
(@User4ID, @ShippingInfo4ID, 16999, 16999, 1, 2, 0, DATEADD(DAY, -3, GETDATE()), DATEADD(DAY, -3, GETDATE())),
-- Order 5: User 5's order with multiple items
(@User5ID, @ShippingInfo5ID, 29998, 29998, 1, 2, 0, DATEADD(DAY, -1, GETDATE()), DATEADD(DAY, -1, GETDATE()));

-- Get Order IDs
DECLARE @Order1ID BIGINT, @Order2ID BIGINT, @Order3ID BIGINT, @Order4ID BIGINT, @Order5ID BIGINT;
SELECT TOP 1 @Order1ID = Id FROM dbo.Orders WHERE UserId = @User1ID ORDER BY CreatedAt DESC;
SELECT TOP 1 @Order2ID = Id FROM dbo.Orders WHERE UserId = @User2ID ORDER BY CreatedAt DESC;
SELECT TOP 1 @Order3ID = Id FROM dbo.Orders WHERE UserId = @User3ID ORDER BY CreatedAt DESC;
SELECT TOP 1 @Order4ID = Id FROM dbo.Orders WHERE UserId = @User4ID ORDER BY CreatedAt DESC;
SELECT TOP 1 @Order5ID = Id FROM dbo.Orders WHERE UserId = @User5ID ORDER BY CreatedAt DESC;

-- Create Order Details with proper references
INSERT INTO dbo.OrderDetails (
    OrderId,
    CustomShoeDesignId,
    SizeId,
    ManufacturerId,
    Quantity,
    TotalPrice,
    TemplatePrice,
    ServicePrice,
    DesignerMarkup,
    IsDeleted,
    CreatedAt,
    UpdatedAt
)
VALUES
-- Order 1 Details: Classic Red Sneaker, Size 42
(@Order1ID, @Design1ID, @Size42ID, @Manufacturer1ID, 1, 15999, 8999, 5500, 1500, 0, DATEADD(DAY, -10, GETDATE()), DATEADD(DAY, -10, GETDATE())),

-- Order 2 Details: Minimal Gray, Size 40
(@Order2ID, @Design10ID, @Size40ID, @Manufacturer1ID, 1, 14499, 8999, 4300, 1200, 0, DATEADD(DAY, -8, GETDATE()), DATEADD(DAY, -8, GETDATE())),

-- Order 3 Details: Two pairs - Summer Vibes and Neon Future
(@Order3ID, @Design4ID, @Size43ID, @Manufacturer1ID, 1, 16999, 8999, 5500, 2500, 0, DATEADD(DAY, -5, GETDATE()), DATEADD(DAY, -5, GETDATE())),
(@Order3ID, @Design7ID, @Size43ID, @Manufacturer1ID, 1, 16999, 8999, 5000, 3000, 0, DATEADD(DAY, -5, GETDATE()), DATEADD(DAY, -5, GETDATE())),

-- Order 4 Details: Purple Haze, Size 39
(@Order4ID, @Design6ID, @Size39ID, @Manufacturer1ID, 1, 16999, 7999, 6800, 2200, 0, DATEADD(DAY, -3, GETDATE()), DATEADD(DAY, -3, GETDATE())),

-- Order 5 Details: Two pairs - Forest Green and Monochrome White
(@Order5ID, @Design5ID, @Size44ID, @Manufacturer1ID, 1, 14999, 9999, 3200, 1800, 0, DATEADD(DAY, -1, GETDATE()), DATEADD(DAY, -1, GETDATE())),
(@Order5ID, @Design8ID, @Size44ID, @Manufacturer1ID, 1, 14999, 9999, 3500, 1500, 0, DATEADD(DAY, -1, GETDATE()), DATEADD(DAY, -1, GETDATE())); 