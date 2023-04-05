FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /app

COPY EndpointTests/EndpointTests.csproj .
COPY EndpointTests ./EndpointTests

RUN dotnet restore /app/EndpointTests/EndpointTests.csproj

COPY . .
RUN dotnet build -c Release /app/EndpointTests/EndpointTests.csproj

FROM build AS testrunner

WORKDIR /app
COPY --from=build /app .

RUN dotnet test --logger:trx;LogFileName=test_results.trx

#FROM testrunner AS livingdocgenerator

#RUN dotnet tool install --global SpecFlow.Plus.LivingDoc.CLI

#ENV PATH="${PATH}:/root/.dotnet/tools"
#RUN livingdoc feature-folder ./TestResults --output ./LivingDoc.html --title "ExchangeRate API Test Report" --project /app/EndpointTests/EndpointTests.csproj

ENTRYPOINT ["dotnet", "test", "/app/EndpointTests/EndpointTests.csproj", "--logger:trx;LogFileName=test_results.trx"]
