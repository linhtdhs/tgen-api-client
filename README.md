# TGenApiClient

> **A Vibe Coded Project** 🚀✨

## Overview
**TGenApiClient** is an on-the-go desktop application designed for testing and verifying HTTP APIs without requiring installation. Built using **.NET 10** and the **Avalonia UI** framework, it serves as a lightweight, portable alternative to tools like Postman or ThunderClient.

The project strictly adheres to a **Pragmatic 2-Tier MVVM Architecture** to ensure clean separation of concerns and long-term maintainability.

## Folder Structure
The solution is cleanly divided into two primary projects:

* **`TGenApiClient.Core`** (The Engine):
  * **`Models/`**: Core data structures representing requests, responses, history items, and environments.
  * **`Services/`**: The backend logic including `HttpService` for network execution, `EnvironmentService` for interpolation, and `HistoryService` for persistence.
  * **`Constants/`**: Application-wide centralized configuration tokens.
  * *Note: This layer is strictly independent and carries absolutely zero UI dependencies.*

* **`TGenApiClient.UI`** (The Interface):
  * Application views (`MainWindow.axaml`, `EnvironmentWindow.axaml`) built natively in Avalonia framework.
  * **`Utils/`**: Custom syntax highlighters and text formatting logic for the embedded `AvaloniaEdit` interfaces.
  * Pre-configured MVVM binding structure utilizing `CommunityToolkit.Mvvm` and Dependency Injection targeting the `Core` library tools.

## Key Features
* **Modern, Dark Theme UI:** A responsive 3-pane layout allowing simultaneous views of your request history, active request parameters, and response streams.
* **Environment Variables:** Easily define multiple operational environments. Dynamically inject variables into your URLs, headers, and payloads automatically using standard `{{variable_name}}` syntax.
* **Rapid Request Configuration:**
  - Interactive Method and Indentation (2-space, 4-space, tabs) dropdown selectors.
  - Native syntax highlighting for Environment Variables inside URLs and Text Bodies.
  - Real-time error underlining for malformed JSON, XML, or Header definitions.
* **Automatic History Logging:** 
  - Effortlessly tracks and saves your requests and successful responses locally (Currently via robust persistence config mapping).
  - Instantly load and replay previous payloads and configurations just by clicking an item in the sidebar.
* **Rich Response Viewing:**
  - Immediate colored status indicators (200 OK, 404 Not Found, etc.).
  - Performance diagnostics (Latency tracked in milliseconds, total payload sizes).
  - Fast, unblocking read-only instances for huge payloads.
* **Zero-Install Deployment:** Operates entirely self-contained without touching the host OS paths or registry. 

## Building and Running from Source

Verify you have the latest [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0) installed.

To compile and launch the application directly from source:
```bash
dotnet build
dotnet run --project TGenApiClient.UI
```

## Creating a Portable Standalone Executable

To publish the application as a highly portable, single-file executable that does not require the user to pre-install the .NET runtime:

```bash
dotnet publish TGenApiClient.UI -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true
```

The resulting standalone, drop-and-run executable will be located inside the `TGenApiClient.UI\bin\Release\net10.0\win-x64\publish\` directory.
