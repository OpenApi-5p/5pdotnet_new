<p align="center">
  <img src="https://images.5paisa.com/login/5paisalogonew.svg" width="60%" alt="5Paisa Logo">
</p>

<h1 align="center">5Paisa Connect .NET Library</h1>

<p align="center">
    <em>Your gateway to seamless integration with 5Paisa APIs using .NET</em>
</p>

<p align="center">
    <img src="https://img.shields.io/github/license/prashant-Sharma-mnit/5pdotnet_new?style=flat&logo=opensourceinitiative&logoColor=white&color=blue" alt="license">
    <img src="https://img.shields.io/github/last-commit/prashant-Sharma-mnit/5pdotnet_new?style=flat&logo=git&logoColor=white&color=blue" alt="last-commit">
    <img src="https://img.shields.io/github/languages/top/prashant-Sharma-mnit/5pdotnet_new?style=flat&color=blue" alt="repo-top-language">
    <img src="https://img.shields.io/github/languages/count/prashant-Sharma-mnit/5pdotnet_new?style=flat&color=blue" alt="repo-language-count">
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

The API can work with either **ScripCode** or **ScripData**. For details on **ScripData** format, refer to the [documentation](https://xstream.5paisa.com/dev-docs/order-management-system/place-order).

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
