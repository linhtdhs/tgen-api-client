# tgenapiclient

## Overview
**tgenapiclient** is an on-the-go desktop application to test and verify HTTP APIs without any installation. The project is created using **.NET 10** and the **Avalonia UI** framework. It acts as a lightweight alternative to tools like Postman or ThunderClient.

## Features
* **Modern, Dark Theme UI:** A responsive split-pane layout to view request configuration on the left and response outcomes on the right.
* **Request Configuration:**
  - Method Selector (`GET`, `POST`, `PUT`, `DELETE`, `PATCH`).
  - URL Input Bar.
  - Request Headers Text Box.
  - Request Body Text Box.
* **Detailed Response View:**
  - Status indicators (e.g. HTTP Status Code colored green for success and red for errors).
  - Performance Metrics (Request Latency in ms, Payload Size in bytes).
  - Response Body Viewer.
  - Response Headers Viewer.
* **Portable Deployment:** The project has been configured to publish as a single executable, completely self-contained. 

## Building and Running from Source

You need [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0) installed.

To build and run:
```bash
dotnet build
dotnet run
```

## Creating a Standalone Executable

To publish the application as a single executable that does not require the .NET runtime to be installed on the target machine:

```bash
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

The resulting executable will be located in the `bin\Release\net10.0\win-x64\publish\` directory.
