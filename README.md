# BankTransferAPI
A bank transaction API

You are to build a bank transaction API.
Consider the following conditions when building the API:
1.
There are two types of customers – Business and Retail.
2.
If the customer is a business user and has opened more than one (1) account within the same year, he gets 7% discount on transactions above 150,000 after three transactions within the same month.
3.
If the customer is a retail user, he gets 2% discount on transactions above 50,000 and less than 100,000 after three transactions within the same month.
4.
If a user has been a customer for over 4 years, he gets a 10% discount for every first three transactions in a month.
Write a DOTNET 8 API which exposes the following endpoints:
▪
Do Transfer
▪
Get Transaction History by Account Number
Request Body:
Do Transfer Request: { "sourceAccount": "", "destinationAccount": "", "amount": 0}
