# Automatic Payment Processing System

## Overview

This system automatically processes payments to designers and manufacturers 30 days after an order has been completed. The payment is based on the orderDetail information which contains the breakdown of costs between template price, designer markup, and service price.

## How It Works

1. The `DesignerManufacturerPaymentService` runs as a background service in the application.
2. It checks for orders that have been in `Completed` status for the specified number of days (default: 30 days).
3. For each eligible order:
   - It verifies that a successful payment exists for the order.
   - It processes each order detail to pay the designer and manufacturer.
   - The designer receives their markup amount for each item sold, adjusted by their commission rate.
   - The manufacturer receives the service price amount for each item sold, adjusted by their commission rate.
   - Transactions are recorded in the database.
   - User balances are updated accordingly.

## Commission Rates

Both designers and manufacturers have commission rates defined in their respective entities:

1. **Designer Commission Rate**: This is a percentage value (0-100) that determines how much of the designer markup the designer actually receives.
   - Example: If a designer's markup is $100 and their commission rate is 80%, they receive $80.

2. **Manufacturer Commission Rate**: This is a percentage value (0-100) that determines how much of the service price the manufacturer actually receives.
   - Example: If a service price is $200 and the manufacturer's commission rate is 70%, they receive $140.

The commission rates allow the platform to take a percentage of each transaction while providing transparency to all parties involved.

## Configuration

The payment processing system is configurable through the `appsettings.json` file:

```json
"PaymentProcessing": {
  "PaymentProcessingCheckIntervalMinutes": 60,  // How often to check for orders ready for payout
  "DaysAfterOrderToPayOut": 30                  // Number of days to wait after order completion before payout
}
```

## Entities Involved

- **Order**: Contains the overall order information
- **OrderDetail**: Contains the detailed breakdown of each item ordered
- **Transaction**: Records the payment to designers and manufacturers
- **User**: Contains the balance that will be updated when payment is processed
- **CustomShoeDesign**: Links to the designer (User) who created the design
- **Manufacturer**: Links to the user who provides manufacturing services and contains the commission rate
- **Designer**: Contains the designer's commission rate

## Transaction Processing

Each transaction created includes:
- The recipient user ID
- The order detail ID
- The payment ID
- The amount of money transferred (adjusted by the applicable commission rate)
- Timestamps

## Fail-safe Mechanisms

The system includes several fail-safe mechanisms:
- It checks for existing transactions to avoid duplicate payments
- It logs detailed information about each payment processed
- It handles errors gracefully and continues processing other payments if one fails
- It uses database transactions to ensure data consistency
- It provides fallback values if commission rates are not found

## Monitoring

The system logs various events:
- When the service starts and stops
- When payment processing runs
- The number of eligible orders found
- Successful payments to designers and manufacturers
- Commission rates applied to each payment
- Errors that occur during processing 