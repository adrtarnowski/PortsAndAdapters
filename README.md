# Ports And Adapters
Here you can find "Ports and Adapters" .NET project template. I divided application into four main parts:

### API
Api contains common set of packages with startup class that runs web service instance.
    
### Application
Application defines public contract for external applications. This is a gateway to an application core.

### Core
Core provides the business logic that is written in a plain language. It delivers domain specific logic and encapsulates the logic.

### Infrastructure
Infrastructure delivers the integration part like database, queue and other external providers.
