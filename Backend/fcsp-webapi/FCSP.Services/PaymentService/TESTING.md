# Testing the Designer/Manufacturer Payment Service

This document outlines how to test the automated payment system for designers and manufacturers.

## Prerequisites

- A development/test environment with the application running
- Access to the database to verify transactions
- Multiple test users with different roles (designers, manufacturers)

## Test Scenarios

### 1. Test Configuration Settings

1. Update the `appsettings.json` file to use shorter intervals for testing:
   ```json
   "PaymentProcessing": {
     "PaymentProcessingCheckIntervalMinutes": 1,
     "DaysAfterOrderToPayOut": 0.01  // This makes it check after ~15 minutes instead of 30 days
   }
   ```

2. Restart the application to apply these settings

### 2. Set Up Test Commission Rates

1. Create or update designers with different commission rates:
   ```sql
   -- Update a designer to have an 80% commission rate
   UPDATE Designers
   SET CommissionRate = 80
   WHERE Id = [YourDesignerId];
   
   -- Update a manufacturer to have a 70% commission rate
   UPDATE Manufacturers
   SET CommissionRate = 70
   WHERE Id = [YourManufacturerId];
   ```

2. Verify the commission rates are set correctly:
   ```sql
   -- Verify designers commission rates
   SELECT d.Id, u.Name, d.CommissionRate
   FROM Designers d
   JOIN Users u ON d.UserId = u.Id;
   
   -- Verify manufacturers commission rates
   SELECT m.Id, u.Name, m.CommissionRate
   FROM Manufacturers m
   JOIN Users u ON m.UserId = u.Id;
   ```

### 3. Create Test Orders

1. Create at least one test order with the following characteristics:
   - Order contains custom shoe designs with designer markup
   - Order includes services from manufacturers
   - Order has been paid successfully
   
2. Manually update the order in the database:
   ```sql
   -- Set the order status to completed
   UPDATE Orders
   SET Status = 3, -- Completed status
       UpdatedAt = DATEADD(DAY, -1, GETDATE()) -- Set to yesterday to trigger immediate processing
   WHERE Id = [YourOrderId];
   ```

### 4. Monitor Payment Processing

1. Check the application logs for the background service activities:
   - Look for entries from `DesignerManufacturerPaymentService`
   - Verify it identifies the orders that need processing
   - Look for log entries showing commission rates being applied
   
2. Verify database entries after processing:
   ```sql
   -- Check for transactions
   SELECT * FROM Transactions
   WHERE OrderDetailId IN (SELECT Id FROM OrderDetails WHERE OrderId = [YourOrderId]);
   
   -- Check user balances have been updated
   SELECT Id, Name, Balance FROM Users
   WHERE Id IN (
     SELECT UserId FROM Manufacturers
     WHERE Id IN (SELECT ManufacturerId FROM OrderDetails WHERE OrderId = [YourOrderId])
     UNION
     SELECT UserId FROM CustomShoeDesigns
     WHERE Id IN (SELECT CustomShoeDesignId FROM OrderDetails WHERE OrderId = [YourOrderId])
   );
   ```

### 5. Test Commission Rate Calculations

1. Verify commission rates are correctly applied:
   ```sql
   -- Get order details with pricing
   SELECT od.Id, od.DesignerMarkup, od.ServicePrice, od.Quantity 
   FROM OrderDetails od
   WHERE od.OrderId = [YourOrderId];
   
   -- Check designer transactions
   SELECT t.Id, t.Amount, od.DesignerMarkup * od.Quantity AS TotalMarkup, d.CommissionRate,
          (od.DesignerMarkup * od.Quantity * d.CommissionRate / 100) AS ExpectedAmount
   FROM Transactions t
   JOIN OrderDetails od ON t.OrderDetailId = od.Id
   JOIN CustomShoeDesigns csd ON od.CustomShoeDesignId = csd.Id
   JOIN Designers d ON d.UserId = csd.UserId
   WHERE od.OrderId = [YourOrderId]
   AND t.ReceiverId = csd.UserId;
   
   -- Check manufacturer transactions
   SELECT t.Id, t.Amount, od.ServicePrice * od.Quantity AS TotalServicePrice, m.CommissionRate,
          (od.ServicePrice * od.Quantity * m.CommissionRate / 100) AS ExpectedAmount
   FROM Transactions t
   JOIN OrderDetails od ON t.OrderDetailId = od.Id
   JOIN Manufacturers m ON od.ManufacturerId = m.Id
   WHERE od.OrderId = [YourOrderId]
   AND t.ReceiverId = m.UserId;
   ```

### 6. Test Fail-Safe Mechanisms

1. Test duplicate payment prevention:
   - Run the service again (or wait for the next interval)
   - Verify no duplicate transactions are created
   
2. Test error handling:
   - Create an order with missing or invalid data (e.g., no manufacturer, invalid designer)
   - Verify the service logs errors but continues processing other orders
   
3. Test partial payment:
   - Create an order where only the designer or only the manufacturer can be paid
   - Verify the valid payment is processed and the invalid one is logged
   
4. Test missing commission rates:
   - Create a designer without setting a commission rate
   - Verify the default 100% rate is used

## Verification Checklist

- [ ] Background service starts properly with the application
- [ ] Service identifies orders ready for payout
- [ ] Designer receives correct payment amount (designer markup × quantity × commission rate)
- [ ] Manufacturer receives correct payment amount (service price × quantity × commission rate)
- [ ] Transactions are recorded correctly in the database
- [ ] User balances are updated correctly
- [ ] No duplicate transactions are created on subsequent runs
- [ ] Different commission rates are applied correctly
- [ ] Default commission rate (100%) is used when not specified
- [ ] Errors are handled gracefully without crashing the service
- [ ] Logs provide adequate information about the process

## Troubleshooting

If the service doesn't process payments as expected:

1. Check the logs for any error messages
2. Verify the order is in Completed status
3. Verify the order has a successful payment
4. Ensure the order's UpdatedAt date is older than the configured payout delay
5. Check that the designer and manufacturer accounts exist and are valid
6. Verify the commission rates are set correctly and within the valid range (0-100) 