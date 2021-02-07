## What can you find here? :rocket:
The goal of this project is implement a reusable .NET project template with a full set of functionality. You can treat it as a reference project to avoid doing repetitive work or for training purposes. Template is based on ports and adapters pattern.

## Give a Star :star:
 If you liked the project or any part of it, please give a star :)
 
## Introduction:
The solution is divided into five parts:

- **API** contains common set of packages with startup class that runs web service instance.

- **Application** defines public contract for external applications. This is a gateway to an application core.

- **Core** provides the business logic that is written in a plain language. It delivers domain specific logic and encapsulates the logic.

- **Infrastructure** delivers the integration part like database, queue and other external providers.

- **Kitbag** provides plugins as a shared library.

## Quick start:

1. Clone the repository and run the following commands:
     1. Install the new template: *dotnet new --install {REPO PATH}*
     1. Create new solution: *dotnet new TLJ.PortsAndAdapters -n {YOUR PROJECT NAME} -o {YOUR PROJECT NAME}*
  1. Database configuration:
     1. Provide database connection string in *appsettings.json* file in *{YOUR PROJECT NAME}.API* project
     1. Provide the same database connection string in *{YOUR PROJECT NAME}.DatabaseMigration* project
     1. Run *{YOUR PROJECT NAME}.DatabaseMigration*. Project applies necessary migrations to your database.
  1. Run *{YOUR PROJECT NAME}.Api* project.
  1. Open swagger page: *http://localhost:{YOUR PORT}/index.html*

## List of Kitbags:

- **Audit Trail** :bookmark_tabs: Kitbag provides audit trail mechanism that can track any database changes without making any unnecessary modification in existing code [README]()

