#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Kitbags/.", "Kitbags/"]
COPY ["TLJ.PortsAndAdapters.Api/TLJ.PortsAndAdapters.Api.csproj", "TLJ.PortsAndAdapters.Api/"]
COPY ["TLJ.PortsAndAdapters.Application/TLJ.PortsAndAdapters.Application.csproj", "TLJ.PortsAndAdapters.Application/"]
COPY ["TLJ.PortsAndAdapters.Core/TLJ.PortsAndAdapters.Core.csproj", "TLJ.PortsAndAdapters.Core/"]
COPY ["TLJ.PortsAndAdapters.Infrastructure/TLJ.PortsAndAdapters.Infrastructure.csproj", "TLJ.PortsAndAdapters.Infrastructure/"]

RUN dotnet restore "TLJ.PortsAndAdapters.Api/TLJ.PortsAndAdapters.Api.csproj"
COPY . .
WORKDIR "/src/TLJ.PortsAndAdapters.Api"
RUN dotnet build "TLJ.PortsAndAdapters.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TLJ.PortsAndAdapters.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TLJ.PortsAndAdapters.Api.dll"]