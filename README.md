# Checkout API Excerise

Build an API that allows a merchant
- To process a payment through your payment gateway.
- To retrieve details of a previously made payment.

Build a simulator to mock the responses from the bank to test the API from your first deliverable.

### Design
I made a design decision that to use endpoint, consumers would have to pass the Merchant code in the request header to get data from the endpoint e.g amazon would
be 'AMAUK' to signify the merchant abbreviation and country code as this could be expanded in the future if merchants have a franchise in other countries 

### Improvements
 - More Logging
 - More Test around card details
 - More Validation around card details
 - Use async methods in api
 - Add Authentication
 - Api versioning