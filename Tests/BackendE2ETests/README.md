# End-to-End (E2E) Test Project

## Overview

This project is designed to execute end-to-end (E2E) tests for validating the functionality and behavior of the system.
It ensures that all the services work together seamlessly within the system by routing requests through the **Admin BFF
** service.

## Key Features

- **Testing Framework:** This project uses **XUnit** as the testing framework.
- **Admin BFF Service:** All tests communicate with the **Admin BFF service**, which acts as a proxy. The tests never
  communicate directly with individual microservices but rely on the Admin BFF for all interactions.
- **Docker Compatibility:** Tests can run in complete isolation of an external Docker Compose infrastructure to ensure
  repeatability and deterministic results.
- **Self-Contained Test Data:** All test data required during execution is embedded within the project. No external
  internet dependency is needed to fetch the test data.
- **Environment Cleanup:** Any data created during the tests is thoroughly cleaned up after execution, ensuring the
  environment is restored to its initial state.
- **Environment Variables Support:** Tests can be configured via environment variables when necessary, allowing for
  customization of the test environment and behavior without modifying the codebase.

## Prerequisites

- .NET 8.0 SDK must be installed.
- Docker (optional, only required if you wish to run the tests in complete isolation).

## Running the Tests

To execute the tests locally, follow these steps:

1. Build the project:
   ```bash
   dotnet build
   ```

2. Run the tests:
   ```bash
   dotnet test
   ```

If you want to run the tests using the Docker Compose infrastructure, ensure the services are up and running:

   ```bash
   docker-compose up
   ```

### Configuring

The `TestConfig` class supports configurable properties that are sourced from environment variables. Environment 
variables can be specified on the host machine (i.e. Windows > Start > "Edit environment variables for your account")
or they can be specified in a `.runsettings` file in the root of the repository.

| Environment Variable                                | Description                                                                                                                                | Default Value                |
|-----------------------------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------|------------------------------|
| EXTERNAL_FHIR_SERVER_BASE_URL                       | Base URL for FHIR server from where the tests are being executed                                                                           | http://localhost:6157/fhir   |
| INTERNAL_FHIR_SERVER_BASE_URL                       | Base URL for FHIR server from where link services are deployed/running (i.e. within the docker network)                                    | http://fhir-server:8080/fhir |
| ADMIN_BFF_BASE_URL                                  | Base URL for Admin BFF service API                                                                                                         | http://localhost:8063/api    |
| ADHOC_REPORTING_SMOKE_<br/>TEST_MEASURE_BUNDLE_PATH | Path to the measure bundle file used in smoke tests                                                                                        | resource://...ACH...json     |
| ADMINBFF_OAUTH_SHOULD_AUTHENTICATE                  | Flag to enable OAuth authentication for Admin BFF                                                                                          | false                        |
| ADMINBFF_OAUTH_TOKEN_ENDPOINT                       | OAuth token endpoint URL for Admin BFF authentication                                                                                      |                              |
| ADMINBFF_OAUTH_CLIENT_ID                            | OAuth client ID for Admin BFF authentication                                                                                               |                              |
| ADMINBFF_OAUTH_USERNAME                             | Username for Admin BFF OAuth authentication                                                                                                |                              |
| ADMINBFF_OAUTH_PASSWORD                             | Password for Admin BFF OAuth authentication                                                                                                |                              |
| ADMINBFF_OAUTH_SCOPE                                | OAuth scope for Admin BFF authentication                                                                                                   |                              |
| FHIRSERVER_OAUTH_SHOULD_AUTHENTICATE                | Flag to enable OAuth authentication for FHIR server                                                                                        | false                        |
| FHIRSERVER_OAUTH_TOKEN_ENDPOINT                     | OAuth token endpoint URL for FHIR server authentication                                                                                    |                              |
| FHIRSERVER_OAUTH_CLIENT_ID                          | OAuth client ID for FHIR server authentication                                                                                             |                              |
| FHIRSERVER_OAUTH_USERNAME                           | Username for FHIR server OAuth authentication                                                                                              |                              |
| FHIRSERVER_OAUTH_PASSWORD                           | Password for FHIR server OAuth authentication                                                                                              |                              |
| FHIRSERVER_OAUTH_SCOPE                              | OAuth scope for FHIR server authentication                                                                                                 |                              |
| FHIRSERVER_BASICAUTH_SHOULD_AUTHENTICATE            | Whether to pass basic credentials as authentication to the FHIR server. If both OAUTH and BASICAUTH are specified, OAUTH takes precedence. | false                        | 
| FHIRSERVER_BASICAUTH_USERNAME                       | Username for basic authentication with the FHIR server.                                                                                    |                              |
| FHIRSERVER_BASICAUTH_PASSWORD                       | Password for basic authentication with the FHIR server.                                                                                    |                              |

The default values/settings are configured to support running the BackendE2ETests within the docker environment as specified by the `/docker-compose.yml` file.

> Note: If using the Rider IDE for development/testing, you need to configure the test settings in Rider to use `.runsettings` in the `Build, Execution, Deployment > Unit Testing > Test Runner > Test Settings` section.

## Test Data

- Test data resides within the **E2ETests** project.
- The tests are pre-configured to load and utilize this data during execution.

## Cleanup

Post execution, all test-created data in the environment is cleaned up. This ensures that the tests leave no residual
data that might interfere with subsequent test runs.

## End-to-End Tests

### Smoke Test

This smoke test ensures that the core functionality of the system's adhoc and historical reporting capabilities are
working as expected. It validates the interaction between services via the **Admin BFF** service. The test performs the
following steps:

1. **Load Data on a FHIR Server**: Populate the FHIR server with the necessary test data, ensuring the data is structured correctly for testing purposes.

2. **Load a Measure into the MeasureEval and Validation Services**:
    - Store a predefined measure into the **MeasureEval** service, which is responsible for evaluating measures.
    - The measure's validationa rtifacts are also loaded into the **Validation** service to verify its compliance with the expected standards.

3. **Configure a Tenant for the FHIR Server**: Set up a dedicated tenant configuration associated with the FHIR server. This is a minimal configuration that indicates how to query and normalize.

4. **Generate an Adhoc Report**:
    - Trigger the generation of an adhoc report using the data and measures loaded into the system. This tests the report generation workflow from input processing to report creation.
    - Polls the Admin BFF service on an interval to check when the report is submitted, and proceeds when it has been submitted

5. **Download the Report Data**: Retrieve the generated report data.

6. **Validate the Report Data**: Verify the downloaded report data against expected results to ensure accuracy and completeness.