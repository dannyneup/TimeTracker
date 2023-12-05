FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TimeTracker.Api/TimeTracker.Api.csproj", "TimeTracker.Api/"]
RUN dotnet restore "TimeTracker.Api/TimeTracker.Api.csproj"
COPY . .
WORKDIR "/src/TimeTracker.Api"
RUN dotnet build "TimeTracker.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TimeTracker.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TimeTracker.Api.dll"]
