﻿[← Back Home](../README.md)

# Common Configurations for CSharp Services

## Swagger

| Property       | Description                | Required | Default Value | Secret? |
|----------------|----------------------------|----------|---------------|---------|
| EnableSwagger  | Enable Swagger spec and UI | No       | false         | No      |

## Azure App Config Environment Variables

The following should be specified as environment variables when launching the service:

| Name                                     | Description                                   | Required             | Default Value  | Secret?  |
|------------------------------------------|-----------------------------------------------|----------------------|----------------|----------|
| ExternalConfigurationSource              | Specifies the configuration source to use     | Yes                  | None           | No       |
| ConnectionStrings__AzureAppConfiguration | Connection string for Azure App Configuration | Yes (if using Azure) | None           | Yes      |

## Kafka

| Name                                 | Description               | Required  | Secret?  |
|--------------------------------------|---------------------------|-----------|----------|
| KafkaConnection__BootstrapServers__0 | Kafka broker address      | Yes       | No       |
| KafkaConnection__GroupId             | Consumer group identifier | Yes       | No       |
| KafkaConnection__ClientId            | Unique client identifier  | Yes       | No       |

### SASL Plain Text Authentication

Include the following configuration properties to connecto to Kafka that requires SASL_PLAINTEXT authentication:

| Name                                 | Value      | Secret? |
|--------------------------------------|------------|---------|
| KafkaConnection__SaslProtocolEnabled | true       | No      |
| KafkaConnection__SaslUsername        | <username> | No      |
| KafkaConnection__SaslPassword        | <password> | Yes     |

### Kafka Consumer Settings

| Name                                    | Description                                                           | Secret? |
|-----------------------------------------|-----------------------------------------------------------------------|---------|
| ConsumerSettings__ConsumerRetryDuration | A list of strings representing retry duration intervals for consumers | No      |
| ConsumerSettings__DisableRetryConsumer  | A boolean flag to enable/disable the retry mechanism for consumers    | No      |
| ConsumerSettings__DisableConsumer       | A boolean flag to completely enable/disable the consumer              | No      |

## CORS

| Property              | Description                        | Required   | Default                                     | Secret? |
|-----------------------|------------------------------------|------------|---------------------------------------------|---------|
| CORS__AllowAllOrigins | Allow all origins                  | No         | false                                       | No      |
| CORS__AllowedOrigins  | Array of allowed origins           | No         | []                                          | No      |
| CORS__AllowAllMethods | Allow all HTTP methods             | No         | true                                        | No      |
| CORS__AllowedMethods  | Array of allowed HTTP methods      | No         | ["GET", "POST", "PUT", "DELETE", "OPTIONS"] | No      |
| CORS__AllowAllHeaders | Allow all headers                  | No         | true                                        | No      |
| CORS__MaxAge          | Preflight cache duration (seconds) | No         | 600                                         | No      |

## Token Service Settings

TODO: Describe what the purpose of the token endpoint is

| Property                                        | Type    | Description                                               | Default Value         | Secret? |
|-------------------------------------------------|---------|-----------------------------------------------------------|-----------------------|---------|
| LinkTokenService__EnableTokenGenerationEndpoint | bool    | Controls whether the token generation endpoint is enabled | false                 | No      |
| LinkTokenService__Authority                     | string  | The authority URL used for token validation/generation    | Required (no default) | No      |
| LinkTokenService__LinkAdminEmail                | string? | Email address for Link administration                     | null                  | No      |
| LinkTokenService__TokenLifespan                 | int     | The lifespan of generated tokens in minutes               | 10                    | No      |
| LinkTokenService__SigningKey                    | string? | The key used for signing tokens                           | null                  | Yes     |
| LinkTokenService__LogToken                      | bool    | Controls whether token logging is enabled                 | false                 | No      |

## Service Authentication

| Property                                           | Description                                  | Required | Default Value | Secret? |
|----------------------------------------------------|----------------------------------------------|----------|---------------|---------|
| Authentication__EnableAnonymousAccess              | Enable anonymous access to the service       | No       | false         | No      |
| Authentication__Schemas__LinkBearer__Authority     | Authority URL for Link Bearer authentication | Yes      | None          | No      |
| Authentication__Schemas__LinkBearer__ValidateToken | Validate the token on each request           | No       | true          | No      |
| DataProtection__Enabled                            | Enable data protection for sensitive data    | No       | false         | No      |

## Redis

| Name                     | Description              | Required                 | Secret?  |
|--------------------------|--------------------------|--------------------------|----------|
| ConnectionStrings__Redis | Redis connection string  | Yes (if caching enabled) | Yes      |
| Cache__Enabled           | Toggle for Redis caching | No                       | No       |

## Service Registry

The Service Registry section contains URLs and configurations for all microservices in the Link Cloud ecosystem. Configuration key: `ServiceRegistry`

Not all services use every URL. The configuration is provided for completeness and future-proofing. If using Azure App Configuration, all service URLs should be specified, and each service will use only the URLs it needs.

### Service URLs

| Property                                   | Description                               |
|--------------------------------------------|-------------------------------------------|
| ServiceRegistry__AccountServiceUrl         | Base URL for the Account service          |
| ServiceRegistry__AuditServiceUrl           | Base URL for the Audit service            |
| ServiceRegistry__CensusServiceUrl          | Base URL for the Census service           |
| ServiceRegistry__DataAcquisitionServiceUrl | Base URL for the Data Acquisition service |
| ServiceRegistry__MeasureServiceUrl         | Base URL for the Measure service          |
| ServiceRegistry__NormalizationServiceUrl   | Base URL for the Normalization service    |
| ServiceRegistry__NotificationServiceUrl    | Base URL for the Notification service     |
| ServiceRegistry__QueryDispatchServiceUrl   | Base URL for the Query Dispatch service   |
| ServiceRegistry__ReportServiceUrl          | Base URL for the Report service           |
| ServiceRegistry__SubmissionServiceUrl      | Base URL for the Submission service       |

### Tenant Service Configuration

| Property                                 | Description                                 | Required   | Default     |
|------------------------------------------|---------------------------------------------|------------|-------------|
| TenantService__TenantServiceUrl          | Base URL for the Tenant service             | Yes        | None        |
| TenantService__CheckIfTenantExists       | Whether to validate tenant existence        | No         | true        |
| TenantService__GetTenantRelativeEndpoint | Relative endpoint path for tenant retrieval | No         | "facility/" |

### SQL Server Database

| Name                                  | Description                               | Required | Default Value | Secret? |
|---------------------------------------|-------------------------------------------|----------|---------------|---------|
| DatabaseProvider                      | Database provider to use                  | No       | SqlServer     | No      |
| ConnectionStrings__DatabaseConnection | MSSQL connection string                   | Yes      | None          | Yes     |
| AutoMigrate                           | Automatically migrate the database schema | No       | false         | No      |

### Mongo Database

| Name                      | Description               | Required | Default Value | Secret? |
|---------------------------|---------------------------|----------|---------------|---------|
| MongoDB__ConnectionString | MongoDB connection string | Yes      | None          | Yes     |
| MongoDB__DatabaseName     | MongoDB database name     | Yes      | None          | No      |

### Example Configuration
```json
{
  "ServiceRegistry": {
    "AccountServiceUrl": "https://localhost:7001",
    "AuditServiceUrl": "https://localhost:7334",
    "CensusServiceUrl": "https://localhost:7234",
    "DataAcquisitionServiceUrl": "https://localhost:7194",
    "MeasureServiceUrl": "https://localhost:7135",
    "NormalizationServiceUrl": "https://localhost:7038",
    "NotificationServiceUrl": "https://localhost:7434",
    "QueryDispatchServiceUrl": "https://localhost:7534",
    "ReportServiceUrl": "https://localhost:7110",
    "SubmissionServiceUrl": "https://localhost:7046",
    "TenantService": {
      "TenantServiceUrl": "https://localhost:7332",
      "CheckIfTenantExists": true,
      "GetTenantRelativeEndpoint": "facility/"
    }
  }
}
```