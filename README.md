<p align="center">
  <img src="https://images.5paisa.com/login/5paisalogonew.svg" width="60%" alt="5Paisa Logo">
</p>

<h1 align="center">5Paisa Connect .NET Library</h1>

<p align="center">
    <em>Your gateway to seamless integration with 5Paisa APIs using .NET</em>
</p>

<p align="center">
    <img src="https://img.shields.io/github/last-commit/prashant-Sharma-mnit/5pdotnet_new?style=flat&logo=git&logoColor=white&color=blue" alt="last-commit">
</p>

<p align="center">
	<em>Built with modern tools:</em>
</p>

<p align="center">
	<img src="https://img.shields.io/badge/.NET-512BD4.svg?style=flat&logo=.NET&logoColor=white" alt=".NET">
	<img src="https://img.shields.io/badge/C%23-239120.svg?style=flat&logo=C-Sharp&logoColor=white" alt="CSharp">
	<img src="https://img.shields.io/badge/REST%20API-008080.svg?style=flat&logo=API&logoColor=white" alt="REST API">
	<img src="https://img.shields.io/badge/MS%20SQL-CC2927.svg?style=flat&logo=Microsoft-SQL-Server&logoColor=white" alt="MS SQL">
	<img src="https://img.shields.io/badge/Visual%20Studio-5C2D91.svg?style=flat&logo=Visual-Studio&logoColor=white" alt="Visual Studio">
	<br>
	<img src="https://img.shields.io/badge/GitHub-Actions-2088FF.svg?style=flat&logo=GitHub-Actions&logoColor=white" alt="GitHub Actions">
	<img src="https://img.shields.io/badge/Unit%20Testing-00ADD8.svg?style=flat&logo=Testing&logoColor=white" alt="Unit Testing">
	<img src="https://img.shields.io/badge/JSON-000000.svg?style=flat&logo=JSON&logoColor=white" alt="JSON">
</p>

---

### Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Installation](#installation)
- [API Key Configuration](#api-key-configuration)
- [Usage](#usage)
- [API Reference](#api-reference)
- [Contributing](#contributing)
- [License](#license)
- [Acknowledgments](#acknowledgments)

---

## Overview

The **5Paisa Connect .NET Library** offers a convenient way for developers to integrate with 5Paisa trading APIs. This library is designed for seamless stock order placement, retrieval of market data, managing portfolios, and more, empowering developers to build robust trading applications in .NET.

---

## Features

- **Stock Order Placement**: Easy and efficient way to place, modify, and cancel orders.
- **Market Data**: Retrieve detailed information about market depth for various scrips.
- **Algo Orders Support**: Seamless integration with algo trading, allowing algorithmic order placement.
- **Real-Time Data**: Access real-time stock market data for effective decision-making.
- **WebSocket Support**: Stream live market data with minimal latency.
- **Scrip Master**: Fetch scrip details such as codes and instrument information.

---

## Installation

### Prerequisites

Before using the **5Paisa Connect .NET Library**, ensure that you have the following installed on your machine:

- **.NET SDK**: .NET Core 3.1 or higher / .NET 6.0 (recommended). You can download it from [here](https://dotnet.microsoft.com/download).
- **Visual Studio**: Version 2019 or later (recommended). You can download it from [here](https://visualstudio.microsoft.com/).
   - Make sure to install the ".NET Desktop Development" workload.
- **Git**: For version control and to clone/download the library from GitHub. You can download it from [here](https://git-scm.com/).

### Installation via GitHub

```bash
git clone https://github.com/OpenApi-5p/5pdotnet_new.git
cd 5pdotnet_new
```
Add the DLL to your project references:
   - In Visual Studio, right-click on your project in Solution Explorer.
   - Select "Add" > "Reference..."
   - Browse to the location of `5PaisaConnect.dll` and add it.
---
## API Key Configuration

To use the 5Paisa APIs, you need to configure API keys. Follow these steps to set up your API keys:

1. Sign up or log in to your 5Paisa account [here](https://xstream.5paisa.com/dashboard).
2. Navigate to the developer section in your account settings and generate the following keys:
   - **API Key**
   - **Encryption Key**
   - **Client Code**
3. Use the keys in your .NET project:

```csharp
string APIKey = "your_api_key";
string EncryptionKey = "your_encryption_key";
string EncryptUserId = "your_encrypted_user_id";
string RequestToken = "your_RequestToken";
string ClientCode = "your_ClientCode";

OutputBaseClass obj = new OutputBaseClass();
connect = new _5PaisaAPI(APIKey, EncryptionKey, EncryptUserId);
Token agr = new Token();
```
### Generate Access Token

To generate an access token, use the `GetOuthLogin` method with your request token. Here's how you can do it:

```csharp
// Generate access token using Request Token
var authResponse = connect.GetOuthLogin(RequestToken);
var accessToken = authResponse.TokenResponse;
```

### Get TOTP Login

To perform **TOTP** (Time-based One-Time Password) login, use the following method:

```csharp
// Perform TOTP login using the provided ClientCode, TOTP, and Pin
var response = connect.TOTPLogin(ClientCode, TOTP, Pin);

if (response.status == "0")
{
    string requestTokenOTP = response.TokenResponse.RequestToken;
    response = connect.GetOuthLogin(requestTokenOTP);
    var accessToken = response.TokenResponse;
}
```
---
## Usage

### Place Order

The **Place Order** API enables users to execute trades through their demat accounts. This API is essential for placing various types of orders securely.

#### Authentication

To use this API, you must provide an **Access Token** (Bearer Token) for authentication.

#### Supported Order Types

The API supports the following order types:
- **Limit Order**
- **Market Order**
- **Stop Loss Order**
- **Stop Loss – Market Order**
- **After Market Order**
- **IOC Order**

Orders can be placed as either **intraday** or **delivery**.

#### Input Parameters

The API can work with either **ScripCode** or **ScripData**. For more details on this api refer to the [documentation](https://xstream.5paisa.com/dev-docs/order-management-system/place-order).

#### Code Example

Here’s how to perform an order placement:

```csharp
// Perform Order placement
OrderInfo order = new OrderInfo();
order.Exchange = '';
order.ExchangeType = '';
order.ScripCode = 0;
order.ScripData = "";
order.Price = 236;
order.OrderType = "";
order.Qty = 1;
order.DisQty = 0;
order.StopLossPrice = 0;
order.IsIntraday = false;
order.iOrderValidity = 0;
order.AppSource = 10345;
order.RemoteOrderID = "";
obj = connect.placeOrder(order);
OrderResponse resOrderbook = obj.PlaceOrderResponse;   
```
---
### Modify Order

The **Modify Order** API allows users to modify any order that has not yet been successfully executed. You can update fields such as **price**, **quantity**, **stop loss price**, and even change the order type from **limit** to **market** or vice versa.

#### Authentication

To use this API, you must provide an **Access Token** (Bearer Token) for authentication, either through **OAuth** or **TOTP**.

#### Important Notes

- You only need to pass the fields that require modification along with the **ExchangeOrderID**.
- Passing all other fields is optional.
- The **ExchangeOrderID** is required to identify the order and can be obtained from the order book.

#### Input Parameters

The API requires the **ExchangeOrderID** for order identification. For more information, refer to the [documentation](https://xstream.5paisa.com/dev-docs/order-management-system/modify-order).

#### Code Example

Here’s how to modify an order:

```csharp
// Modify Order
OrderInfo Modifyorder = new OrderInfo();
Modifyorder.Qty = 2;
// Modifyorder.StopLossPrice = "";
// Modifyorder.Price = "";
Modifyorder.ExchOrderID = "";
obj = connect.ModifyOrder(Modifyorder);
OrderResponse Modifyres = obj.PlaceOrderResponse;   
```
---
### Cancel Order

The **Cancel Order** API allows users to cancel an order that has not yet been successfully executed.

#### Authentication

To use this API, you must provide an **Access Token** (Bearer Token) for authentication.

#### Important Notes

- The API can cancel an order by passing just the **ExchangeOrderID**.
- The **ExchangeOrderID** for any order can be fetched from the **Order Status**, **Order Book**, or **Order WebSocket**.

#### Input Parameters

You only need to provide the **ExchangeOrderID** to cancel an order. For more details, refer to the [documentation](https://xstream.5paisa.com/dev-docs/order-management-system/cancel-order).

#### Code Example

Here’s how to cancel an order:

```csharp
// Cancel Order
OrderInfo Cancelorder = new OrderInfo();
Cancelorder.ExchOrderID = ClientCode;
obj = connect.CancelOrder(order);
OrderResponse Cancelres = obj.PlaceOrderResponse;   
```
---
### Order Book

The **Order Book** API enables users to track orders placed throughout the day. This API provides details on various types of orders, including cash, derivatives, currency, and commodity orders.

#### Authentication

To use this API, you must provide an **Access Token** (Bearer Token) for authentication.

#### API Response

The API responds with a detailed list of orders, including various parameters such as:
- Average Price
- Trigger Rate
- Quantity Traded
- Pending Quantity

Additionally, it provides the status of the API execution along with:
- Remote Order ID
- Broker Order ID
- Exchange Order ID

For more details, refer to the [documentation](https://xstream.5paisa.com/dev-docs/order-tracking-system/order-book).

#### Code Example

Here’s how to fetch the order book:

```csharp
// Fetch Order Book
OrderInfo orderBook = new OrderInfo();
orderBook.ClientCode = ClientCode;
obj = connect.OrderBook(orderBook);
OrderBookResponse res = obj.OrderBook;   
```
---
### Trade Book

The **Trade Book** API allows users to track trades executed throughout the day. It fetches the trade book, which contains details for cash, derivatives, currency, and commodity trades across multiple exchanges (NSE, BSE, MCX).

#### API Response

The API provides a detailed list of trades, including:
- Order Rate
- Order Quantity
- Traded Quantity

Since one order can be executed in multiple trades, you can map them using `ExchangeOrderId` and `ExchangeTradeID`.

For more details, refer to the [documentation](https://xstream.5paisa.com/dev-docs/order-tracking-system/trade-book).

#### Code Example

Here’s how to fetch the trade book:

```csharp
// Fetch Trade Book
OrderInfo TradeBook = new OrderInfo();
TradeBook.ClientCode = ClientCode;
obj = connect.TradeBook(TradeBook);
TradeBookResponse resTradeBook = obj.TradeBook;   
```
---

### Trade History

The **Trade History** API helps clients track their trade history, providing a detailed list of trades with various parameters.

#### Authentication

To use this API, you must provide an **Access Token** (Bearer Token) for authentication.

#### API Response

The API returns a list of trade history records, allowing users to view details associated with each trade.

Since this API provides detailed trade history, you can map orders using the `ExchOrderID`.

#### Code Example

Here’s how to fetch trade history:

```csharp
// Fetch Trade History
OrderInfo TradeHistory = new OrderInfo()
{
    ExchOrderList = new List<ExchOrderIDList>()
};
var ExOrderId = new string[] { "", "", "" };

foreach (var item in ExOrderId)
{
    ExchOrderIDList ExchOrderId = new ExchOrderIDList();
    ExchOrderId.ExchOrderID = item;
    TradeHistory.ExchOrderList.Add(ExchOrderId);
}
TradeHistory.ClientCode = ClientCode;
obj = connect.TradeHistory(TradeHistory);
TradeHistoryResponse resTradeHistory = obj.TradeHistory;
```
---
### NetPosition

The **NetPosition** API provides information about open positions in derivatives contracts and intraday stock positions. Overnight derivative positions can be identified using the `BodQty` flag, while overnight stock positions will not appear as they are converted into holdings.

#### Authentication

To use this API, you must provide an **Access Token** (Bearer Token) for authentication.

#### API Response

The API returns details about open positions for the client. Overnight derivative positions can be distinguished using the `BodQty` flag. Note that overnight stock positions are not displayed as they are converted to holdings.

For more details on this API, refer to the [documentation](https://xstream.5paisa.com/dev-docs/portfolio-management-system/netwise-positions).

#### Code Example

Here’s how to fetch net positions:

```csharp
// Call NetPositionNetWise API
OrderInfo netPositionRequest = new OrderInfo();
netPositionRequest.ClientCode = ClientCode; // Make sure to set the appropriate ClientCode here

// Call the NetPositionNetWise method from your backend API
OutputBaseClass obj = connect.NetPositionNetWise(netPositionRequest);

// Retrieve the response from the API call
NetPositionNetWiseRes res = obj.NetPositionNetWise;
```
---

### Historical Data

The **Historical Data** API provides historical candle data for various scrip codes, facilitating strategy deployment based on past trading activity.

#### Authentication

Clients must log in to use this API. Upon successful login, a token is generated in the response, which needs to be validated using a JWT validation API.

#### Data Provided

The API returns the following data:
- **OHLC Data**: Open, high, low, and close rates.
- **Volume Data**: Information about the trading volume.
- **Timestamps**: Time information associated with the provided data.

#### Interval Size

- **Maximum Permissible Interval Size**: 6 months.
- **Day Wise Data**: No restrictions on the size of the interval; maximum data can be fetched when the interval is a day.

#### Supported Intervals

- 1 minute
- 5 minutes
- 10 minutes
- 15 minutes
- 30 minutes
- 60 minutes
- Day-based interval

**Note**: The API allows fetching data for any time duration within the specified interval limits.

For more details on this API, refer to the [documentation](https://xstream.5paisa.com/dev-docs/market-data-system/historical-candles).

#### Code Example

Here’s how to fetch historical data:

```csharp
string Exch = "";
string ExchType = "";
int Scripcode = 0;
string day = "";
DateTime FromDate = DateTime.Today;
DateTime EndDate = DateTime.Today;
obj = connect.historical(Exch, ExchType, Scripcode, day, FromDate, EndDate);
```
---
### Market Feed

The **Market Feed** API is used to fetch the market feed of a particular scrip or a set of scrips.

#### Response Details

The response of the API includes details such as:
- **LTP**: Last Traded Price
- **High**: Highest price of the requested scrip
- **Low**: Lowest price of the requested scrip
- **Previous Close**: Closing price of the previous trading session

The response also contains the status and messages based on the execution of the API.

For more details on this API, refer to the [documentation](https://xstream.5paisa.com/dev-docs/market-data-system/market-feed).

#### Code Example

Here’s how to fetch the market feed:

```csharp
OrderInfo MarketFeed = new OrderInfo()
{
    MarketFeedData = new List<MarketFeedDataListReq>()
};

string[][] arr = new string[2][];

// Initialize the elements.
arr[0] = new string[4] { "N", "C", "0", "RELIANCE_EQ" };
arr[1] = new string[4] { "N", "C", "0", "RELIANCE_EQ" };

for (int i = 0; i < arr.Length; i++)
{
    MarketFeedDataListReq a1 = new MarketFeedDataListReq();
    a1.Exch = arr[i][0];
    a1.ExchType = arr[i][1];
    a1.ScripCode = Convert.ToInt32(arr[i][2]);
    a1.ScripData = arr[i][3];
    MarketFeed.MarketFeedData.Add(a1);
}

obj = connect.MarketFeed(MarketFeed);
MarketFeedResponse resMarketFeed = obj.MarketFeed;
//End 
```
---

### Scrip Master

The **Scrip Master** API allows users to fetch the scrip details of all equity, derivatives, and commodities for NSE, BSE, and MCX. The data is provided in the form of a CSV dump, which can be imported into a database. The scrip master is regularly updated and can be accessed through the following segments:

- **all**: Scrips across all segments
- **bse_eq**: BSE Equity
- **nse_eq**: NSE Equity
- **nse_fo**: NSE Derivatives
- **bse_fo**: BSE Derivatives
- **ncd_fo**: NSE Currency
- **mcx_fo**: MCX Commodities

For more details on this API, refer to the [documentation](https://xstream.5paisa.com/dev-docs/docFundamentals/scrip-master).

#### Code Example

Here’s how to fetch data from the Scrip Master API:

```csharp
// Retrieving data from ScripMaster API
string segment = "nse_eq"; // Replace with the desired segment
string scripMasterResponse = string.Empty;

// Call the ScripMaster method 
try
{
    scripMasterResponse = connect.ScripMaster(segment);

    // Check if the response is not null or empty
    if (!string.IsNullOrEmpty(scripMasterResponse))
    {
        // Process the response here (You can do it as per your convenience)
        Console.WriteLine("Scrip Master Data:");
        Console.WriteLine(scripMasterResponse);
    }
    else
    {
        Console.WriteLine("No data received from ScripMaster API.");
    }
}
catch (Exception ex)
{
    // Handle any exceptions that may occur during the API call
    Console.WriteLine($"Error calling ScripMaster API: {ex.Message}");
}
```
---
### Web Socket

The **Web Socket**  facilitates the integration of live streaming functionality for market data and trade confirmations. It simplifies the trading experience for clients by providing real-time updates and confirmations. This can be easily integrated into your application and operates in an authenticated environment.

#### Key Features:
- **Live Streaming**: Fetch market data and trade confirmations in real-time.
- **Subscription Model**: Clients can subscribe to or unsubscribe from various types of live data streams.
- **Authentication**: Requires access tokens and client codes for establishing a connection.

To connect to the Web Socket, clients must:
1. Connect to the server.
2. Subscribe to the desired data types.

The connection request requires the access token and client code as query parameters

For more details on Web Socket, refer to the [documentation](https://xstream.5paisa.com/dev-docs/market-data-system/web-socket).

#### Code Example

Here’s how to retrieve data from the Web Socket:

```csharp
// Retrieving data from websocket
WebSocket _WS = new WebSocket();
var exitEvent = new ManualResetEvent(false);
string Acc = "{{AccessToken}}"; // Replace with actual access token

// Connect to the WebSocket for market feed
_WS.ConnectForFeed(Acc, ClientCode);

if (_WS.IsConnected())
{
    _WS.MessageReceived += WriteResult;
    
    WebsocketInfo MarketFeed = new WebsocketInfo()
    {
        MarketFeedData = new List<WebSocketMarketFeedDataListReq>()
    };
    
    string[][] arr = new string[3][];
    
    // Initialize the elements
    arr[0] = new string[3] { "N", "C", "11536" };
    arr[1] = new string[3] { "N", "D", "57919" };
    arr[2] = new string[3] { "B", "C", "500325" };
    
    for (int i = 0; i < arr.Length; i++)
    {
        WebSocketMarketFeedDataListReq a1 = new WebSocketMarketFeedDataListReq();
        a1.Exch = arr[i][0];
        a1.ExchType = arr[i][1];
        a1.ScripCode = Convert.ToInt32(arr[i][2]);
        
        MarketFeed.MarketFeedData.Add(a1);
    }
    
    MarketFeed.Method = "MarketFeedV3";
    MarketFeed.Operation = "Subscribe";
    MarketFeed.ClientCode = ClientCode;
    _WS.FetchFeed(MarketFeed);
    //_WS.Close();
}

exitEvent.WaitOne();

static void WriteResult(object sender, MessageEventArgs e)
{
    Console.WriteLine("Received: " + e.Message);
}
```
---
## API Reference

For a complete reference to all available APIs, please refer to the official 5Paisa API documentation:

- [5Paisa API Documentation](https://xstream.5paisa.com/dev-docs)
Explore detailed guides, request and response structures, and example use cases.
---
## Contributing

We welcome all types of contributions to make this project better. Whether you're reporting issues, suggesting new features, or improving documentation, your feedback helps us grow. Here's how you can contribute:

### 1. Reporting Issues
If you come across any bugs, errors, or inconsistencies in the library or documentation, you can help us by:
- **Opening an Issue**:
  - Go to the **Issues** tab of the repository.
  - Click on **New Issue** and provide detailed information, including:
    - A clear description of the problem.
    - Steps to reproduce the issue (if applicable).
    - Screenshots, logs, or error messages that illustrate the issue (if possible).
    - Your environment setup, such as the .NET version and any relevant configurations.

### 2. Suggesting New Features
If you have an idea for a new feature or enhancement, we encourage you to share it with us:
- **Open an Enhancement Request**:
  - Create a new issue with the **Enhancement** label and describe the feature in detail.
  - Explain the benefit of the feature and how it can improve the library's functionality.
  - Provide example use cases or relevant documentation links that could help clarify the feature.

### 3. Improving Documentation
We value contributions that improve the clarity and comprehensiveness of our documentation:
- **Propose Documentation Improvements**:
  - If you find sections of the documentation that are unclear, outdated, or lacking in detail, open an issue to suggest improvements.
  - You can also recommend additional sections or topics you feel should be covered in the documentation.
  - If you'd like to provide more structured feedback on the documentation (e.g., clarity, readability, and technical accuracy), feel free to include that in your issue description.

### 4. Feedback on User Experience
Your feedback on the overall user experience is important to us:
- **General Feedback**:
  - If you have suggestions for improving how users interact with the library (e.g., the ease of setup, API usability, or overall experience), please share them by opening a new issue or participating in existing discussions.
  - You can also leave comments on how the project can be more accessible to different skill levels.

### 5. Engaging in Discussions
We encourage users to actively participate in discussions within the repository:
- **Join Ongoing Discussions**:
  - Participate in open issues or discussions to share your insights and suggestions.
  - Help other users by providing your perspective or suggesting potential solutions to the problems they raise.

### 6. Community Etiquette
To foster a positive and collaborative environment, we ask all contributors to:
- Be respectful and constructive in your feedback and suggestions.
- Keep discussions focused on improving the project and addressing real issues.

By contributing in any of the above ways, you're helping us create a better experience for everyone!

---

