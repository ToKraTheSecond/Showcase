# FsharpShowcase
F# code that I want to expose publicly

## Get Programming with F#

### Capstone 2 - Simple Bank Account System

* allow a customer to deposit and withdraw from an account and maintain a running total of the balance
* decline transcaction if the customer tries to withdraw more money than is in the account ( balance stay as is )
* system writes out all transactions to a data store (is pluggable - filesystem, console, .. )
* do not couple code to filesystem, console input
* app executable as a console application
* at start ask for the cusomer's name and opening balance -> creates account in memory
* app repeatedly asks whether the customer wants to deposit or withdraw money from the account
* app prints out updated balance to the user after every transaction.
* store balance only in memory
* do not worry about opening multiple accounts
* do not worry about overdraw warning - just keep the same balance

F# structures that will be use:
- records, tuples, functions, high-order functions
