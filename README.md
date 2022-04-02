# Crypto Coin Checker

It is a demonstrative project which tries to build an application related to crypto currencies, using the architecture *server - client*.

The main objective to achieve is being able to display, on the client side, the list of supported **currencies with real-time price update**.

In order to build the *backend* side of the application, **.NET Core** framework will be used, which already provides with different features to achieve the purpose of this project.

For the client side, it has not been decided yet how will be implemented, but it might be mobile related. *(since I'd like to refresh my knowledge about it)* 🙂

---
## Backend side
The backend side will be responsible to inform the client about the supported currencies and keep updating the price value according to real market prices.

### Backend API
An API is provided to the client in order to ask for the available currencies, which for the purpose of this demo, there will be hard-coded in a list.

Even though, it is expected to have one controller with few routes in this project, [Swagger](https://swagger.io/) will be used for tooling and building the backend API through the [nuget](https://www.nuget.org/packages/swashbuckle.aspnetcore/) *Swashbuckle.AspNetCore* for .NET Core. 
Providing the OpenAPI specification standard, which is always a good practice, and Swagger UI to explore and perform tests on controllers and routes.

### SignalR
To achieve the main objective, real-time prices need to be presented on client regularly so at this point there are two ways to proceed:

1. Make the Client be sending requests in intervals to ask for updated prices
2. Make the Backend inform about the updated prices whenever they are ready

Option 1 is not feasible since it would end up with a possible saturation of the bandwidth, management of requests order, many unattended requests to the server in case the component to get updated prices might not be operative, and several more.

Here is when SignalR, using the nuget *Microsoft.AspNetCore.SignalR.Core*, comes in to help with option 2, allowing informing the clients, previously subscribed, with any update in the price for specific currency when is available.

Regardless the *IMarketCheckerService* interface implementation, to inform the Client about updates, it should be done from the Hub implementation calling the method `Clients.All.SendAsync("ReceiveMessage", message);`
where *"ReceiveMessage"* is the listener method which should be implemented on the Client side to process the message sent.

### CoinGecko API
[Gecko API](https://www.coingecko.com/en/api) has been selected to get real-time prices for currencies from a crypto data aggregator.
The current free plan fits and covers the goals for this demo project.

- [x] 100% Free
- [x] No keys required
- [x] Publicly available
- [x] Rate limit of 50 calls/minute

Further reading of the [official documentation](https://www.coingecko.com/en/api/documentation) is required to know the definition of the available methods.

#### CoinGecko API Implementation
For the purpose of this demo project, the main path to request will be *'coins/markets'*, which together with the absolute end point base path will be both defined in the *appsettings.json*.

To request the main path is required to define several parameters which are the following:
```
new Dictionary<string, object>
{
    { "vs_currency", "eur" },
    { "ids", string.Join(",", coinIdList) },
    { "order", "market_cap_desc" },
    { "per_page", "100" },
    { "page", "1" },
    { "sparkline", false }
}
```
Being the most important:
1. `vs_currency` required, the target currency of market data (usd, eur, jpy, etc.)
2. `ids`, a comma separated string with the names of currencies, provided by `coinIdList` being the list of available Coins defined in the backend.

As a result, there is a list of `CoinMarketsDefinition` which is being mapped to be queued and sent to Client.

The whole process of getting updated prices from the API is set under an infinite loop, with a configurable Delay in order to not overload the network and avoid reaching the maximum call/minutes for the API free plan.

### Logging
It is always required some kind of logging, even if this is a demo project, to track and debug possible issues and bugs since this application will likely run on the background or as a service.

Even though there are different logging libraries for .NET, [Serilog](https://github.com/serilog/serilog) which provides diagnostic logging to files, the console, and many other outputs will be configured and implemented.
#### Configuration
*Please refer to the [Getting Started](https://github.com/serilog/serilog/wiki/Getting-Started) page to know more about initial configuration and extra features.*

This application has been configured using the [Two-stage initialization](https://github.com/serilog/serilog-aspnetcore#two-stage-initialization) in order to identify possible issues during the bootup process and afterwards reading the configuration settings from the `appsettings.json` in order to write to the Console and File the logging entries.

### Unit Testing
Adding Unit Testing to each project being developed it is always part of best practices. Allowing testing isolated methods and components integration.
There are different testing frameworks for .NET out there, but for this project, [NUnit](https://nunit.org/) framework will be used.
#### NSubstitute
For Mocking libraries there are several options as well, but in order to try out something different, [NSubstitute](https://github.com/nsubstitute/NSubstitute) library will be used in order to create a substitute instance for specified type.

### Launching project
For the purpose of this demo project, it has been set to be executed like console application, however it can be executed from IIS Express as well.
The assigned ports for both options are defined in `appsettings.json`and `launchSettings.json` respectively. 

*- Benjamin Soro*