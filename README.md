
# Quality Assurance Plan: CryptoAPI

## Objective

The purpose of this section is to outline how the CryptoAPI project has been tested to ensure reliability, maintainability, and correctness. Testing is applied at multiple levels of the application using unit tests, integration tests and CI/CD automation.

---

## Testing Strategies

### Unit Testing

- **Purpose**: Ensure individual service methods behave correctly under different scenarios, including edge cases and exception conditions.
- **Tools**: MSTest, Moq
- **Scope**: Business logic in service classes such as `PortfolioService`, focusing on authentication, portfolio management, and validation.
- **Example**: Verifying that `AddPortfolioItemAsync` throws when passed null data, and that repository calls occur as expected when valid data is used.

---

### Integration Testing

- **Purpose**: Validate how repositories interact with the in-memory EF Core database.
- **Tools**: MSTest, EF Core In-Memory Database
- **Scope**: Repository behavior (`PortfolioRepository`, `UserRepository`) including entity relationships, data persistence, and query accuracy.
- **Example**: Testing that `UserRepository.GetByUsernameAsync` returns the correct user when seeded, and that portfolio items are correctly added or removed.

---

### End-to-End Testing

- **Purpose**: Simulate real user flows like login or registration via the frontend.
- **Tools**: Cypress 
- **Scope**: API-level interaction from a user's perspective, ensuring that routes and auth mechanisms work in tandem.
- **Example**: Testing that a new user can register and then successfully authenticate.

---

## Tools and Technologies

- MSTest – For both unit and integration testing.
- Moq – For mocking dependencies in service unit tests.
- EF Core In-Memory Database – Used to simulate real database interactions.
- AutoMapper – Used for model transformation and mapping validation.
- Codacy (via GitHub Actions) – For static analysis and code coverage reporting.
- Docker – For building and deploying the backend image.

---

## Test Environment

All tests are executed locally and automatically via GitHub Actions in CI workflows. Integration tests use a clean in-memory database seeded per test run. Cypress-based E2E tests for the frontend.

---

## Test Execution and Reporting

- **Frequency**:
  - Unit and integration tests are run automatically on push or PR to the `main` branch.
  - Code coverage is calculated and sent to Codacy.
- **Tools**:
  - MSTest for test execution
  - Codacy for code coverage reports
- **Metrics**:
  - Code coverage percentage
  - Number of tests passed/failed
  - Test execution time

---

## CI/CD Pipeline Integration

### CI: Build, Test, and Coverage Reporting

```yaml
name: CI Build, Test, and Codacy Coverage

on:
  push:
    branches: [ "main" ]
  pull_request:
    branches: [ "main" ]

jobs:
  build-test-coverage:
    name: Build, Test, and Codacy Coverage
    runs-on: ubuntu-latest

    steps:
      - name: Checkout Code
        uses: actions/checkout@v4
        with:
          fetch-depth: 0

      - name: Setup .NET 8
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '8.0.x'

      - name: Restore Dependencies
        run: dotnet restore

      - name: Build Solution
        run: dotnet build --configuration Release --no-restore

      - name: Run Tests with Coverage
        run: |
          dotnet test CryptoAPI.TestProject/CryptoAPI.TestProject.csproj --configuration Release \
            --collect:"XPlat Code Coverage"

      - name: Locate Coverage File
        id: locate_coverage
        run: |
          echo "COVERAGE_FILE=$(find . -name 'coverage.cobertura.xml' | head -n 1)" >> $GITHUB_ENV

      - name: Show Coverage File Path
        run: |
          echo "Found coverage file at: ${{ env.COVERAGE_FILE }}"

      - name: Download Codacy Coverage Reporter
        run: |
          curl -Ls https://github.com/codacy/codacy-coverage-reporter/releases/latest/download/codacy-coverage-reporter-linux \
            -o codacy-coverage-reporter && chmod +x codacy-coverage-reporter

      - name: Upload Coverage to Codacy
        env:
          CODACY_PROJECT_TOKEN: ${{ secrets.CODACY_PROJECT_TOKEN }}
        run: ./codacy-coverage-reporter report -l CSharp -r "${{ env.COVERAGE_FILE }}"
```

---

### CD: Docker Image Build and Push

```yaml
name: Docker Build and Push

on:
  push:
    branches: [ "main" ]  
  pull_request:
    branches: [ "main" ] 

jobs:
  build-and-push:
    runs-on: ubuntu-latest 

    steps:
    - name: Checkout Code
      uses: actions/checkout@v4

    - name: Log in to Docker Hub
      uses: docker/login-action@v2.1.0 
      with:
        username: ${{ secrets.DOCKER_USERNAME }}  
        password: ${{ secrets.DOCKER_PASSWORD }}  

    - name: Build Docker Image
      run: docker build -t ${{ secrets.DOCKER_USERNAME }}/crypto-backend-image .

    - name: Push Docker Image
      run: docker push ${{ secrets.DOCKER_USERNAME }}/crypto-backend-image
```

---

## Conclusion

This QA plan illustrates the comprehensive testing approach used in the CryptoAPI project. With unit tests ensuring logic correctness, integration tests validating persistence behavior, and CI/CD workflows providing automation and code quality feedback, CryptoAPI is structured for stability and long-term maintainability.
