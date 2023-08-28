# CocktailDBAutomatedTests
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)


# CocktailDB API Automated Testing

This repository contains automated tests for the CocktailDB API. The tests cover both functional and non-functional aspects of the API to ensure its reliability, functionality, performance, and security.

## Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Node.js](https://nodejs.org/en/download/) (for Artillery)

## Getting Started

1. Clone this repository:

```bash
git clone https://github.com/your-username/CocktailDBAutomatedTests.git

1. Navigate to the repository directory:
   
   cd CocktailDBAutomatedTests

2. Install the required .NET and Node.js dependencies:
   
   dotnet restore
   
   npm install -g artillery

**Running Functional Tests**

1. Navigate to the API tests directory:
   
   cd Tests/ApiTests

1. Run the NUnit tests:
   
   dotnet test

**Running Non-Functional Tests**

**Performance Test (Artillery)**

1.Navigate to the performance tests directory:

cd Tests/PerformanceTests/Artillery/CocktailDBTests

1. Run the Artillery load test:

   ./run_artillery_test.sh

**Security Scanning Test (OWASP ZAP)**

1. Ensure OWASP ZAP is installed and running.

2. Navigate to the security test directory:

   cd Tests/SecurityTests

1. Run the OWASP ZAP security scan:

   - Open a terminal or command prompt.
   
   - Navigate to the root directory of your project (where your .csproj file is located).
   
     - cd CocktailDBTests
   
     - bash run_owasp_zap_scan.sh   # Run the OWASP ZAP security scan script

     - dotnet build  # Build the project (if not built already)
    
     - dotnet test --filter FullyQualifiedName~CocktailDBTests.Tests.CocktailApiSecurityServiceTest

**Additional Information**
  
  - Functional tests use NUnit and Moq for testing API endpoints and functionalities.

  - Performance tests use the Artillery framework to simulate load and assess performance.

  - Security scanning tests use the OWASP ZAP API to identify security vulnerabilities.


  For more details about the tests and their configurations, refer to the individual test files and the Documentation.md file.

  
Please replace `"your-username"` with your actual GitHub username and customize the content further based on your specific project structure and requirements. This `README.md` template provides an overview of how to set up and run your tests using both .NET and Artillery for performance testing.














