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
  
   
## Overall Test Strategy

The overall strategy for testing the https://api.exchangerate.host/convert?from=USD&to=eur public API will be to ensure that it is functioning correctly and is     able to accurately convert US dollars to euros. This will involve testing the API under various scenarios to verify that it meets the specified requirements, is reliable, performs optimally, and is secure.

## Testing Approach
      
The following approach will be taken to test the https://api.exchangerate.host/convert?from=USD&to=eur public API:

  #### Functional Testing: This will be the first stage of testing and will involve verifying that the API functions as expected. The following tests will be conducted:
        Verify that the API returns the correct conversion value for a given amount of US dollars to euros.
        Verify that the API returns an error message when the from or to currency codes are invalid or not provided.
        Verify that the API returns an error message when the amount to be converted is not provided or is invalid.
        Verify that the API returns the correct conversion value for the maximum allowed amount of US dollars.
        Verify that the API returns an error message when the amount to be converted is greater than the maximum allowed amount.

  #### Performance Testing: This stage of testing will involve verifying that the API performs optimally under various load conditions. The following tests will be conducted:
        Verify that the API can handle a high volume of requests without degrading performance.
        Verify that the API response time remains within acceptable limits under various load conditions.

#### Security Testing: This stage of testing will involve verifying that the API is secure and does not expose any vulnerabilities. The following tests will be conducted:
        Verify that the API is accessible only over HTTPS and not over HTTP.
        Verify that the API requires authentication and authorization before it can be accessed.
        Verify that the API does not expose any sensitive data or information.
        Verify that the API is protected against common attacks such as SQL injection, cross-site scripting, and cross-site request forgery.

#### Integration Testing: This stage of testing will involve verifying that the API can be integrated with other systems without any issues. The following tests will be conducted:
        Verify that the API can be integrated with various programming languages such as Java, Python, and .NET.
        Verify that the API can be integrated with different operating systems such as Windows, Linux, and macOS.
        Verify that the API can be integrated with different web servers such as Apache and IIS.

#### Usability Testing: This stage of testing will involve verifying that the API is user-friendly and easy to use. The following tests will be conducted:
        Verify that the API documentation is clear, concise, and easy to understand.
        Verify that the API error messages are helpful and informative.
        Verify that the API response format is easy to parse and use in different applications.

#### Regression Testing: This stage of testing will involve verifying that the API still works correctly after changes are made to it. The following tests will be conducted:
        Verify that the API still functions as expected after updates are made to the code or infrastructure.
        Verify that the API still meets the specified requirements after changes are made to it.
        Verify that the API still performs optimally after changes are made to it.
