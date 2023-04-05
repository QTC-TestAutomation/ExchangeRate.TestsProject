Feature: Exchange rate conversion
In order to convert between currencies
As a user of the exchange rate API
I want to be able to request exchange rate conversions

Scenario: Convert <fromCurrency> to <toCurrency>
Given I want to convert <fromCurrency> to <toCurrency>
When I call the public exchange rate conversion API with <fromCurrency> and <toCurrency> currencies
Then the API should return a success response with <exchangeRate> exchange rate

Examples: 
| fromCurrency | toCurrency | exchangeRate |
| USD          | EUR        | valid        |
| EUR          | USD        | valid        |
| USD          | USD        | 1            |
| EUR          | EUR        | 1            |

Scenario: Convert with invalid currency code
Given I want to convert to an invalid currency code
When I call the public exchange rate conversion API with USD and an invalid currency code
Then the API should return a success response with <exchangeRate> exchange rate
Examples: 
| exchangeRate |
| null		   |

Scenario: Convert with missing source currency code
Given I want to convert with a missing source currency code
When I call the public exchange rate conversion API with a missing source currency code and EUR
Then the API should return a success response with <exchangeRate> exchange rate
Examples: 
| exchangeRate |
| valid		   |

Scenario: Convert with missing target currency code
Given I want to convert with a missing target currency code
When I call the public exchange rate conversion API with USD and a missing target currency code
Then the API should return a success response with <exchangeRate> exchange rate
Examples: 
| exchangeRate |
| valid		   |

Scenario: Convert with invalid amount
Given I want to convert USD with an invalid amount
When I call the public exchange rate conversion API with an invalid amount and EUR
Then the API should return a success response with <exchangeRate> exchange rate
Examples: 
| exchangeRate |
| null		   |

Scenario: Convert with negative amount
Given I want to convert USD with a negative amount
When I call the public exchange rate conversion API with a negative amount and EUR
Then the API should return a success response with <exchangeRate> exchange rate
Examples: 
| exchangeRate |
| valid		   |