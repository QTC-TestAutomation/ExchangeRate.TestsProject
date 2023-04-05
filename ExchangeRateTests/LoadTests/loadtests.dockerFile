FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

COPY LoadTests/LoadTests.csproj .
COPY LoadTests ./LoadTests

RUN dotnet restore /app/LoadTests/LoadTests.csproj

COPY . .
RUN dotnet build -c Release /app/LoadTests/LoadTests.csproj


FROM build AS testrunner

WORKDIR /app
COPY --from=build /app/LoadTests/bin/Release/net6.0 ./LoadTests

ENTRYPOINT ["dotnet", "/app/LoadTests/LoadTests.dll"]
