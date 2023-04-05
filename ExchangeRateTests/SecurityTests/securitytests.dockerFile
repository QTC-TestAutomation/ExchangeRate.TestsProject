FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

COPY SecurityTests/SecurityTests.csproj .
COPY SecurityTests ./SecurityTests

RUN dotnet restore /app/SecurityTests/SecurityTests.csproj

COPY . .
RUN dotnet build -c Release /app/SecurityTests/SecurityTests.csproj


FROM build AS testrunner

WORKDIR /app
COPY --from=build /app/SecurityTests/bin/Release/net6.0 ./SecurityTests

ENTRYPOINT ["dotnet", "/app/SecurityTests/SecurityTests.dll"]
CMD ["USD"]
