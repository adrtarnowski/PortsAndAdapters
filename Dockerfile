ARG VERSION=6.0-alpine

FROM mcr.microsoft.com/dotnet/sdk:$VERSION AS build
WORKDIR /src
COPY ["TLJ.PortsAndAdapters.Api/TLJ.PortsAndAdapters.Api.csproj", "TLJ.PortsAndAdapters.Api/"]
COPY ["Kitbags/Kitbag.Builder.HttpClient/Kitbag.Builder.HttpClient.csproj", "Kitbags/Kitbag.Builder.HttpClient/"]
COPY ["Kitbags/Kitbag.Builder.Core/Kitbag.Builder.Core.csproj", "Kitbags/Kitbag.Builder.Core/"]
COPY ["Kitbags/Kitbag.Builder.Logging.AppInsights/Kitbag.Builder.Logging.AppInsights.csproj", "Kitbags/Kitbag.Builder.Logging.AppInsights/"]
COPY ["Kitbags/Kitbag.Builder.CQRS.Core/Kitbag.Builder.CQRS.Core.csproj", "Kitbags/Kitbag.Builder.CQRS.Core/"]
COPY ["Kitbags/Kitbag.Builder.RunningContext/Kitbag.Builder.RunningContext.csproj", "Kitbags/Kitbag.Builder.RunningContext/"]
COPY ["Kitbags/Kitbag.Builder.Swagger/Kitbag.Builder.Swagger.csproj", "Kitbags/Kitbag.Builder.Swagger/"]
COPY ["Kitbags/Kitbag.Builder.WebApi/Kitbag.Builder.WebApi.csproj", "Kitbags/Kitbag.Builder.WebApi/"]
COPY ["TLJ.PortsAndAdapters.Application/TLJ.PortsAndAdapters.Application.csproj", "TLJ.PortsAndAdapters.Application/"]
COPY ["TLJ.PortsAndAdapters.Core/TLJ.PortsAndAdapters.Core.csproj", "TLJ.PortsAndAdapters.Core/"]
COPY ["Kitbags/Kitbag.Builder.Persistence.Core/Kitbag.Builder.Persistence.Core.csproj", "Kitbags/Kitbag.Builder.Persistence.Core/"]
COPY ["Kitbags/Kitbag.Builder.CQRS.Dapper/Kitbag.Builder.CQRS.Dapper.csproj", "Kitbags/Kitbag.Builder.CQRS.Dapper/"]
COPY ["Kitbags/Kitbag.Builder.MessageBus/Kitbag.Builder.MessageBus.csproj", "Kitbags/Kitbag.Builder.MessageBus/"]
COPY ["TLJ.PortsAndAdapters.Infrastructure/TLJ.PortsAndAdapters.Infrastructure.csproj", "TLJ.PortsAndAdapters.Infrastructure/"]
COPY ["Kitbags/Kitbag.Builder.AzureAD/Kitbag.Builder.AzureAD.csproj", "Kitbags/Kitbag.Builder.AzureAD/"]
COPY ["Kitbags/Kitbag.Builder.CQRS.IntegrationEvents/Kitbag.Builder.CQRS.IntegrationEvents.csproj", "Kitbags/Kitbag.Builder.CQRS.IntegrationEvents/"]
COPY ["Kitbags/Kitbag.Builder.MessageBus.ServiceBus/Kitbag.Builder.MessageBus.ServiceBus.csproj", "Kitbags/Kitbag.Builder.MessageBus.ServiceBus/"]
COPY ["Kitbags/Kitbag.Builder.Persistence.EntityFramework.Audit/Kitbag.Builder.Persistence.EntityFramework.Audit.csproj", "Kitbags/Kitbag.Builder.Persistence.EntityFramework.Audit/"]
COPY ["Kitbags/Kitbag.Builder.Persistence.EntityFramework.UnitOfWork/Kitbag.Builder.Persistence.EntityFramework.UnitOfWork.csproj", "Kitbags/Kitbag.Builder.Persistence.EntityFramework.UnitOfWork/"]
COPY ["Kitbags/Kitbag.Builder.Persistence.EntityFramework/Kitbag.Builder.Persistence.EntityFramework.csproj", "Kitbags/Kitbag.Builder.Persistence.EntityFramework/"]
RUN dotnet restore "TLJ.PortsAndAdapters.Api/TLJ.PortsAndAdapters.Api.csproj"
COPY . .
WORKDIR "/src/TLJ.PortsAndAdapters.Api"
RUN dotnet build "TLJ.PortsAndAdapters.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TLJ.PortsAndAdapters.Api.csproj" \
    --runtime alpine-x64 \
    --configuration Release \
    --self-contained true \
    --output  /app/publish  \
    /p:PublishSingleFile=true \
    /p:PublishTrimmed=true

FROM mcr.microsoft.com/dotnet/sdk:$VERSION AS final
RUN adduser \
  --disabled-password \
  --home /app \
  --gecos '' app \
  && chown -R app /app
USER app
WORKDIR /app
COPY --from=publish /app/publish .
ENV \
  DOTNET_RUNNING_IN_CONTAINER=true \
  ASPNETCORE_URLS=http://+:3002
EXPOSE 3002
ENTRYPOINT ["./TLJ.PortsAndAdapters.Api", "--urls", "http://0.0.0.0:3002"]