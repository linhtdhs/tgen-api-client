# Antigravity Agent Directive: Project "TGenApiClient"

## 1. Project Overview
**Goal:** Build a lightweight, portable, zero-installation desktop application for testing and verifying HTTP APIs. 
**Tech Stack:**
* Framework: .NET 10 (Configured for Self-Contained, Single-File Deployment)
* UI Library: Avalonia UI (MVVM pattern via `CommunityToolkit.Mvvm`)
* Language: C# 14
* Local Storage: SQLite (Strictly localized to `AppContext.BaseDirectory` to ensure zero host-machine footprint)
* Architecture: Pragmatic 2-Tier MVVM
**Rules**:
* Comment on each interface, function, variable and class
* Do not nest "if" more than 3 times
* Do not repeat your self
* Do not hard code any value or use magic string/number. Create constant for them
* Use .ConfigureAwait(false) on outbound I/O function
* Use async/await on all I/O function
* Use C# 14 features
* Use Avalonia UI for the UI
* Use MVVM pattern for the UI
* Use CommunityToolkit.Mvvm for the UI
* Use SQLite for the database
* Use Entity Framework Core for the database
* Use .NET 10 for the application
* Use Self-Contained, Single-File Deployment for the application

## 2. Architectural Rules (Strict Adherence Required)
You must implement a **2-Tier MVVM Architecture** utilizing "Right-Sized" **SOLID** principles. Do not over-engineer with excessive domain layers. The solution must be divided into exactly two projects:

* **`TGenApiClient.Core`**: The engine of the application. 
    * Contains the SQLite `DbContext`, `HttpClient` logic, core models (e.g., `HttpRequestModel`, `HttpResponseModel`), and service interfaces (e.g., `IRequestExecutionService`, `IHistoryRepository`). 
    * Data storage must strictly use the application's current runtime directory (`AppContext.BaseDirectory`) to maintain portability. 
    * **No UI dependencies allowed.**
* **`TGenApiClient.UI`**: The Avalonia UI presentation layer. 
    * Contains Views (XAML), ViewModels (`ObservableObject`), and the Dependency Injection container setup (`Microsoft.Extensions.DependencyInjection`). 
    * References `TGenApiClient.Core`.

**Right-Sized SOLID Mandates:**
* **SRP:** Keep ViewModels strictly focused on UI state and data binding. Push all HTTP execution and database saving logic into dedicated service classes in the `Core` project.
* **DIP:** ViewModels must not instantiate services directly. Rely exclusively on interfaces (e.g., `IRequestExecutionService`) injected via constructor injection.

## 3. Core Features to Implement
1.  **Request Builder:** UI for HTTP Method (dropdown), URL input, Headers (key-value grid), and Body (JSON text area).
2.  **Response Viewer:** UI to display HTTP Status Code, Response Time, Headers, and formatted JSON payload.
3.  **Portable History:** A sidebar to save API requests and view past execution history, strictly saved to a local portable `.db` file next to the `.exe`.

## 4. Antigravity Execution Plan
Execute this in the following phases. Generate a Task List artifact before proceeding to the next phase.

* **Phase 1: Solution Scaffolding.** Generate the `TGenApiClient.sln` and the two architectural projects (`Core` and `UI`). Set up project references.
* **Phase 2: Core Logic.** Define the models and interfaces. Implement `HttpClient` execution logic and configure Entity Framework Core SQLite to use a relative path for the database. Write basic unit tests for the execution service.
* **Phase 3: UI & ViewModels.** Set up the DI container in `App.axaml.cs`. Build the MVVM bindings and the main Avalonia Grid layout (Sidebar for history, Top bar for URL/Method, Main area for Request/Response data).
* **Phase 4: Deployment Configuration.** Modify the `TGenApiClient.UI.csproj` to enable `<PublishSingleFile>true</PublishSingleFile>`, `<SelfContained>true</SelfContained>`, and `<IncludeNativeLibrariesForSelfExtract>true</IncludeNativeLibrariesForSelfExtract>` so the app can be dropped onto a USB drive and run anywhere.

**Verification:**
After Phase 4, build the application as a single file and verify that a simple `GET` request executes successfully and saves to the local directory without creating folders in the host OS user directories.