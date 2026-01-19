# StockAPI
API endpoint that consumes yahoo finance stock information and outputs json that includes ticker and intraday data include highaverage, lowaverage, and volume

## Tech Stack
* .Net 8 using ASP.NET Core Web API
* Controller based system 
* Yahoo finance API endpoint

## Requirements
* Self-hosted solution
* Consumes Yahoo Finance API
* Accepts symbol as string parameter
* Queries last month intraday data. Parameters used were 15 minute intervals and 1 month for the finance API
* Unit Tested and developed using TDD
* Iterative Development

## Setup
1. Clone this repository
2. Install .NET SDK 8+
3. Use ```dotnet restore``` inside the repository
4. Inside the top layer of the .NET application: run ```dotnet dev-certs https --trust``` to trust the certificate
5. Then run ```dotnet run --launch-profile https``` to launch the application
6. For tests run ```dotnet test```
7. The endpoint is ```GET /api/yahoo/intraday/{symbol}```

## Project Structure
* YahooStockAPI.Api/Controllers: API endpoints + HTTP status handling
* YahooStockAPI.Api/Services: Yahoo query + parsing + calculation handling
* YahooStockAPI.Api/Helpers: date conversion, rounding, averaging helpers
* YahooStockAPI.Tests: xUnit tests

## Challenges
Some of the challenges during this project are listed below.
1. One challenge was handling the rate limit for the yahoo finance API. Without any user agent, the rate limit would be reached and return a status of 429. This problem was resolved by setting the user agent to the one in the example of the problem description: "User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36". After this, the queries were successful.
2. Another challenge was handling exceptions or figuring out where to handle them. There are three layers for the code which go Controller -> Service -> Helper. It was decided that the controller would handle the exceptions as part of the responses and the service and helper files would throw the exceptions with the proper message.
3. Another small problem was handling null datapoints. If the datapoint was null, then some days may be left empty or a null dereference might occur later. This was handled by checking more carefully for null values or seeing if the data exists for certain symbols before even doing the processing.
4. It was initially difficult to understand the stock api output to begin solving this problem. Some time was allocated to understanding the different parameters that could be put into the finance API, what sort of outputs they provided, and getting a better understanding of the structure to actually use it.

## Future Considerations
1. Additional tests to check for other symbols or edge cases
2. Improving error messages or adding more specific logs