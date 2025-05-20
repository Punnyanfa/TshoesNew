-- Get User IDs to use for the orders
DECLARE @User1ID BIGINT, @User2ID BIGINT, @User3ID BIGINT;
SELECT @User1ID = Id FROM dbo.Users WHERE Name = N'john_doe';
SELECT @User2ID = Id FROM dbo.Users WHERE Name = N'jane_smith';
SELECT @User3ID = Id FROM dbo.Users WHERE Name = N'mike_jackson';

-- Get ShippingInfo IDs
DECLARE @ShippingInfo1ID BIGINT, @ShippingInfo2ID BIGINT, @ShippingInfo3ID BIGINT;
SELECT TOP 1 @ShippingInfo1ID = Id FROM dbo.ShippingInfos WHERE UserId = @User1ID AND IsDefault = 1;
SELECT TOP 1 @ShippingInfo2ID = Id FROM dbo.ShippingInfos WHERE UserId = @User2ID AND IsDefault = 1;
SELECT TOP 1 @ShippingInfo3ID = Id FROM dbo.ShippingInfos WHERE UserId = @User3ID AND IsDefault = 1;

-- Get Manufacturer ID
DECLARE @Manufacturer1ID BIGINT;
SELECT TOP 1 @Manufacturer1ID = Id FROM dbo.Manufacturers WHERE Description = N'Premium Shoe Manufacturer';

-- Get Design IDs
DECLARE @Design3ID BIGINT, @Design4ID BIGINT, @Design5ID BIGINT, @Design6ID BIGINT, @Design7ID BIGINT;
SELECT @Design3ID = Id FROM dbo.CustomShoeDesigns WHERE Name = N'Urban Black';
SELECT @Design4ID = Id FROM dbo.CustomShoeDesigns WHERE Name = N'Summer Vibes';
SELECT @Design5ID = Id FROM dbo.CustomShoeDesigns WHERE Name = N'Forest Green';
SELECT @Design6ID = Id FROM dbo.CustomShoeDesigns WHERE Name = N'Purple Haze';
SELECT @Design7ID = Id FROM dbo.CustomShoeDesigns WHERE Name = N'Neon Future';

-- Get Size IDs
DECLARE @Size40ID BIGINT, @Size42ID BIGINT, @Size43ID BIGINT;
SELECT @Size40ID = Id FROM dbo.Sizes WHERE SizeValue = 40;
SELECT @Size42ID = Id FROM dbo.Sizes WHERE SizeValue = 42;
SELECT @Size43ID = Id FROM dbo.Sizes WHERE SizeValue = 43;

-- Get design prices from the designs table
DECLARE @Design3Price INT, @Design4Price INT, @Design5Price INT, @Design6Price INT, @Design7Price INT;
DECLARE @Design3Markup INT, @Design4Markup INT, @Design5Markup INT, @Design6Markup INT, @Design7Markup INT;
DECLARE @Design3Template INT, @Design4Template INT, @Design5Template INT, @Design6Template INT, @Design7Template INT;

SELECT @Design3Price = TotalAmount, @Design3Markup = DesignerMarkup FROM dbo.CustomShoeDesigns WHERE Id = @Design3ID;
SELECT @Design4Price = TotalAmount, @Design4Markup = DesignerMarkup FROM dbo.CustomShoeDesigns WHERE Id = @Design4ID;
SELECT @Design5Price = TotalAmount, @Design5Markup = DesignerMarkup FROM dbo.CustomShoeDesigns WHERE Id = @Design5ID;
SELECT @Design6Price = TotalAmount, @Design6Markup = DesignerMarkup FROM dbo.CustomShoeDesigns WHERE Id = @Design6ID;
SELECT @Design7Price = TotalAmount, @Design7Markup = DesignerMarkup FROM dbo.CustomShoeDesigns WHERE Id = @Design7ID;

SELECT @Design3Template = Price FROM dbo.CustomShoeDesignTemplates WHERE Id = (
    SELECT CustomShoeDesignTemplateId FROM dbo.CustomShoeDesigns WHERE Id = @Design3ID
);
SELECT @Design4Template = Price FROM dbo.CustomShoeDesignTemplates WHERE Id = (
    SELECT CustomShoeDesignTemplateId FROM dbo.CustomShoeDesigns WHERE Id = @Design4ID
);
SELECT @Design5Template = Price FROM dbo.CustomShoeDesignTemplates WHERE Id = (
    SELECT CustomShoeDesignTemplateId FROM dbo.CustomShoeDesigns WHERE Id = @Design5ID
);
SELECT @Design6Template = Price FROM dbo.CustomShoeDesignTemplates WHERE Id = (
    SELECT CustomShoeDesignTemplateId FROM dbo.CustomShoeDesigns WHERE Id = @Design6ID
);
SELECT @Design7Template = Price FROM dbo.CustomShoeDesignTemplates WHERE Id = (
    SELECT CustomShoeDesignTemplateId FROM dbo.CustomShoeDesigns WHERE Id = @Design7ID
);

-- Create Orders with OrderStatus.Completed = 3 and OrderShippingStatus.Delivered = 2
-- Order 1: Urban Black (Design3) + Forest Green (Design5) for User1
INSERT INTO dbo.Orders (
    UserId,
    ShippingInfoId,
    TotalPrice,
    AmountPaid,
    Status,  -- OrderStatus.Completed = 3
    ShippingStatus, -- OrderShippingStatus.Delivered = 2
    IsDeleted,
    CreatedAt,
    UpdatedAt
)
VALUES
(@User1ID, @ShippingInfo1ID, (@Design3Price + @Design5Price) * 20, (@Design3Price + @Design5Price) * 20, 3, 2, 0, DATEADD(DAY, -30, GETDATE()), DATEADD(DAY, -25, GETDATE()));

-- Get Order1 ID
DECLARE @Order1ID BIGINT;
SELECT TOP 1 @Order1ID = Id FROM dbo.Orders 
WHERE UserId = @User1ID AND TotalPrice = (@Design3Price + @Design5Price) * 20
ORDER BY CreatedAt DESC;

-- Order 2: Summer Vibes (Design4) + Purple Haze (Design6) for User2
INSERT INTO dbo.Orders (
    UserId,
    ShippingInfoId,
    TotalPrice,
    AmountPaid,
    Status,  -- OrderStatus.Completed = 3
    ShippingStatus, -- OrderShippingStatus.Delivered = 2
    IsDeleted,
    CreatedAt,
    UpdatedAt
)
VALUES
(@User2ID, @ShippingInfo2ID, (@Design4Price + @Design6Price) * 20, (@Design4Price + @Design6Price) * 20, 3, 2, 0, DATEADD(DAY, -28, GETDATE()), DATEADD(DAY, -24, GETDATE()));

-- Get Order2 ID
DECLARE @Order2ID BIGINT;
SELECT TOP 1 @Order2ID = Id FROM dbo.Orders 
WHERE UserId = @User2ID AND TotalPrice = (@Design4Price + @Design6Price) * 20
ORDER BY CreatedAt DESC;

-- Order 3: Neon Future (Design7) for User3
INSERT INTO dbo.Orders (
    UserId,
    ShippingInfoId,
    TotalPrice,
    AmountPaid,
    Status,  -- OrderStatus.Completed = 3
    ShippingStatus, -- OrderShippingStatus.Delivered = 2
    IsDeleted,
    CreatedAt,
    UpdatedAt
)
VALUES
(@User3ID, @ShippingInfo3ID, @Design7Price * 20, @Design7Price * 20, 3, 2, 0, DATEADD(DAY, -25, GETDATE()), DATEADD(DAY, -20, GETDATE()));

-- Get Order3 ID
DECLARE @Order3ID BIGINT;
SELECT TOP 1 @Order3ID = Id FROM dbo.Orders 
WHERE UserId = @User3ID AND TotalPrice = @Design7Price * 20
ORDER BY CreatedAt DESC;

-- Calculate service prices (total - template - markup)
DECLARE @Design3Service INT = @Design3Price - @Design3Template - @Design3Markup;
DECLARE @Design4Service INT = @Design4Price - @Design4Template - @Design4Markup;
DECLARE @Design5Service INT = @Design5Price - @Design5Template - @Design5Markup;
DECLARE @Design6Service INT = @Design6Price - @Design6Template - @Design6Markup;
DECLARE @Design7Service INT = @Design7Price - @Design7Template - @Design7Markup;

-- Create Order Details for Order 1
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
-- Urban Black, Size 42
(@Order1ID, @Design3ID, @Size42ID, @Manufacturer1ID, 20, @Design3Price * 20, @Design3Template, @Design3Service, @Design3Markup, 0, DATEADD(DAY, -30, GETDATE()), DATEADD(DAY, -25, GETDATE())),
-- Forest Green, Size 42
(@Order1ID, @Design5ID, @Size42ID, @Manufacturer1ID, 20, @Design5Price * 20, @Design5Template, @Design5Service, @Design5Markup, 0, DATEADD(DAY, -30, GETDATE()), DATEADD(DAY, -25, GETDATE()));

-- Create Order Details for Order 2
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
-- Summer Vibes, Size 40
(@Order2ID, @Design4ID, @Size40ID, @Manufacturer1ID, 20, @Design4Price * 20, @Design4Template, @Design4Service, @Design4Markup, 0, DATEADD(DAY, -28, GETDATE()), DATEADD(DAY, -24, GETDATE())),
-- Purple Haze, Size 40
(@Order2ID, @Design6ID, @Size40ID, @Manufacturer1ID, 20, @Design6Price * 20, @Design6Template, @Design6Service, @Design6Markup, 0, DATEADD(DAY, -28, GETDATE()), DATEADD(DAY, -24, GETDATE()));

-- Create Order Details for Order 3
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
-- Neon Future, Size 43
(@Order3ID, @Design7ID, @Size43ID, @Manufacturer1ID, 20, @Design7Price * 20, @Design7Template, @Design7Service, @Design7Markup, 0, DATEADD(DAY, -25, GETDATE()), DATEADD(DAY, -20, GETDATE()));

-- Print success message
PRINT 'Successfully created 3 orders with Status: Completed for CustomShoeDesign IDs 3, 4, 5, 6, and 7 with quantity of 20 each'; 