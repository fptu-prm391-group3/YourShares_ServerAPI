# YourShares_ServerAPI

> Server side api for `YourShares` application.

[![Build Status](https://dev.azure.com/caovanphu19980307/YourShares/_apis/build/status/fptu-se1269-group3.YourShares_ServerAPI?branchName=master)](https://dev.azure.com/caovanphu19980307/YourShares/_build/latest?definitionId=4&branchName=master)

## Table of Contents

* [Overall Description](#Overall-Description)
* [Key features](#Key-features)
* [Getting started](#Getting-started)
    * [Design and Structure](#Design-and-Structure)
    * [Installation](#Installation)
* [License](#License)

## Overall Description

`YourShares` is an application that allows startup company to watch their shares which they published and manage them through each of the round changes. Besides, it also allows shareholders to keep track of their shares such as bonus shares status, holding section together with the company.

Read more about this project at [here](https://sites.google.com/fpt.edu.vn/yourshares)

## Key features

* Manage company shares data, including their amount, holding percent, changes they've made.
* View and keep track of shareholder status.
* Automatic evaluate how the changes through each company's round affect to shareholders, such as the holding section and shares value
* Keep track and analyze shareholder's transaction.


## Getting started

### Design and Structure

* Domain Driven Design
* SQLServer RDBMS
* Restful Web Api with ASP.NET Core
* Mobile application with reactjs [here](https://github.com/phancongbinh1998/YourShares)
* Web administrator

### Installation

To clone and run this application, you will need [Git](https://git-scm.com) and [.NET Core SDK 2.2](https://dotnet.microsoft.com/download/dotnet-core/2.2) installed on your computer. From your command line:

```shell=
# Clone this repository
git clone https://github.com/minhtus/YourShares_ServerAPI.git

# Install dependencies and run application
dotnet run --project YourShares.RestApi
```

## License

This project is licensed  under MIT open source license.

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)
