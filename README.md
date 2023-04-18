# ExchangeRateTests

# Pre-Requisite Software
  * Visual Studio 2022
  * Install .net 6 SDK using command - ```winget install Microsoft.DotNet.SDK.6```

## Run endpoint tests using Docker locally

    cd ExchangeRateTests
    docker build -t exchange-tests -f EndpointTests/tests.dockerFile .
    docker run exchange-tests

## Run nbomber load tests using Docker locally

    docker build -t exchange-load-tests -f LoadTests/loadtests.dockerFile .
    docker run exchange-load-tests
    
## Run security tests using Docker cd locally

    docker build -t exchange-load-tests -f SecurityTests/securitytests.dockerFile .
    docker run exchange-security-tests   

## Github Actions are setup for both endpoint tests and load tests that show the test run results.

   https://github.com/nguntupallis/ExchangeRate.TestsProject/actions
