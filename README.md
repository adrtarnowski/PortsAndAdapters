## What can you find here? :rocket:
The goal of this project is implement a reusable .NET project template with a full set of functionality. You can treat it as a reference project to avoid doing repetitive work or for training purposes. Template is based on ports and adapters pattern.

## Table of Contents
1. [Introduction](#Introduction)
2. [Quick start](#Quick-start)
3. [List of Kitbags](#List-of-Kitbags)
 
## Introduction:
The solution is divided into five parts:

- **API** contains common set of packages with startup class that runs web service instance.

- **Application** defines public contract for external applications. This is a gateway to an application core.

- **Core** provides the business logic that is written in a plain language. It delivers domain specific logic and encapsulates the logic.

- **Infrastructure** delivers the integration part like database, queue and other external providers.

- **Kitbag** provides plugins as a shared library.

## Quick start:

1. Clone the repository and run the following commands:
     1. Install new template: *dotnet new --install {REPO PATH}* (root folder to this repo, more about _dotnet new_ command [here](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-new)
     1. Create new solution: *dotnet new tlj.paa -n {YOUR PROJECT NAME} -o {YOUR PROJECT NAME}* (tlj.paa is a template short name defined in .template.config/template.json file)
  1. Database configuration:
     1. Provide database connection string in *appsettings.json* file in *{YOUR PROJECT NAME}.API* project
     1. Provide the same database connection string in *{YOUR PROJECT NAME}.DatabaseMigration* project
     1. Run *{YOUR PROJECT NAME}.DatabaseMigration*. Project applies necessary migrations to your database.
  1. Run *{YOUR PROJECT NAME}.Api* project.
  1. Open swagger page: *http://localhost:{YOUR PORT}/index.html*


## List of Kitbags:

- **Decorator Strategy** :currency_exchange: By default solution is configured to use decorator strategy. [README](https://github.com/adrtarnowski/PortsAndAdapters/tree/main/TLJ.PortsAndAdapters.Infrastructure)


- **CQRS** : Implemented


**ASYNC**

- **Message Bus** : Implemented


- **Idempotency** : TBD


- **Outbox Pattern** : :inbox_tray: _Kitbag.Builder.Outbox_ and _Kitbag.Builder.Outbox.EntityFramework_ (used EF as a event log) are libraries that provide Outbox Pattern functionality [README](https://github.com/adrtarnowski/PortsAndAdapters/tree/main/Kitbags/Kitbag.Builder.Outbox.EntityFramework)


**STORAGE**:

- **Unit Of Work** : Unit of Work with AuditLog is added


- **Database Migrations** :card_index: Kitbag provides a standardise approach to handling database changes. [README](https://github.com/adrtarnowski/PortsAndAdapters/tree/main/Kitbags/Kitbag.Builder.Persistence.DatabaseMigration.DbUp)


- **Audit Trail** :bookmark_tabs: Kitbag provides audit trail mechanism that can track any database changes without making any unnecessary modification in existing code [README](https://github.com/adrtarnowski/PortsAndAdapters/tree/main/Kitbags/Kitbag.Builder.Persistence.EntityFramework.Audit)


**Observability**:

- **Logging** : :chart_with_upwards_trend: Kitbag provides integration with AppInsights [README](https://github.com/adrtarnowski/PortsAndAdapters/tree/main/Kitbags/Kitbag.Builder.Logging.OpenTelemetry)


- **Health Check** : :syringe: Kitbag provides health check mechanism that can check if the service is up and running with it's dependencies [README](https://github.com/adrtarnowski/PortsAndAdapters/tree/main/Kitbags/Kitbag.Builder.ServiceHealthCheck)






