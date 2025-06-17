﻿[← Back Home](../README.md)

# Configuring Java Services

Spring Boot uses two main YAML configuration files:

* `bootstrap.yml`
    * Loaded before application.yml during the startup process.
    * Used for early-stage configuration that is required before the main application context is loaded.
* `application.yml`
    * Loaded after bootstrap.yml, within the main application context.

Some properties may want to be put in environment-specific configuration files with the naming convention `bootstrap-ENV.yml` and `application-ENV.yml`. For example, there may be `bootstrap-dev.yml` and `bootstrap-prod.yml`. At deployment-time, you can specify an environment variable `SPRING_PROFILES_ACTIVE` to indicate whether to load `dev` or `prod` on top of the default configurations.

To convert a property name in the canonical-form to an environment variable name you can follow these rules:

* Replace dots (.) with underscores (_).
* Remove any dashes (-).
* Convert to uppercase.

For example, the configuration property spring.main.log-startup-info would be an environment variable named SPRING_MAIN_LOGSTARTUPINFO.

Environment variables can also be used when binding to object lists. To bind to a List, the element number should be surrounded with underscores in the variable name.

For example, the configuration property my.service[0].other would use an environment variable named MY_SERVICE_0_OTHER.

Examples of environment variable naming conventions:

| YAML / JSON Key                                                       | Converted Environment Variable                                     |
|-----------------------------------------------------------------------|--------------------------------------------------------------------|
| server.port                                                           | SERVER_PORT                                                        |
| spring.datasource.url                                                 | SPRING_DATASOURCE_URL                                              |
| spring.cloud.azure.appconfiguration.stores[0].selects[0].label-filter | SPRING_CLOUD_AZURE_APPCONFIGURATION_STORES_0_SELECTS_0_LABELFILTER |
| my.custom.settings[2].api-key                                         | MY_CUSTOM_SETTINGS_2_APIKEY                                        |
| config.services[1].endpoints.internal-url                             | CONFIG_SERVICES_1_ENDPOINTS_INTERNALURL                            |

Note: Official guidance is to *remove* dashes (-) entirely from the environment variable. For example: `label-filter` becomes `LABELFILTER`. However, both `LABEL-FILTER` and `LABELFILTER` work interchangeably. 

# Common Configurations for Java Services

Any of the properties for serivce configuration can be provided either via environment variables, through a custom `application.yml` file, or via properties set in java using `-D<propertyName>=<value>` passed as an argument to the JVM during startup.

## Azure App Config and Key Vault

Default `key-filter` and `label-filter` properties are specified for each service, so that at deployment time only the connection to the Azure App Config or Key Vault services needs to be configured/specified in environment variables.

| Property Name                                                         | Description                                                                                  | Type/Value    |
|-----------------------------------------------------------------------|----------------------------------------------------------------------------------------------|---------------|
| spring.cloud.azure.appconfiguration.enabled                           | Enable Azure App Configuration                                                               | true or false |
| spring.cloud.azure.appconfiguration.stores[0].connection-string       | Connection string to Azure App Config instance (if not using managed identity).              | \<string>     |
| spring.cloud.azure.appconfiguration.stores[0].endpoint                | Endpoint to use for App Config when managed identity should be specified via AZURE_CLIENT_ID | \<string>     |
| spring.cloud.azure.appconfiguration.stores[0].selects[0].label-filter | Label to use for configuration                                                               | ",Validation" |
| spring.cloud.azure.appconfiguration.stores[0].selects[0].key-filter   | Key to use for configuration                                                                 | "/"           |

### Authentication

Java Azure libraries have difficult using different authentication mechanisms between App Config (AAC) and Key Vault (AKV). If you specify AZURE_CLIENT_ID, it will attempt to use managed identity for _both_ AAC and AKV.

If using managed identity authentication for one, it is suggested to use managed identity for both; _not_ a connectionString with a token/secret embedded in it for AAC and MI for AKV.

Specifying all three `AZURE_CLIENT_ID`, `AZURE_CLIENT_SECRET` and `AZURE_TENANT_ID` is only necessary when using a service principal for authentication. Only `AZURE_CLIENT_ID` is necessary to authenticate using managed identity.

If using a service principal for authentication, the `AZURE_TENANT_ID` is _not_ the same as the subscription ID.

## Telemetry

| Property Name              | Description                                                   | Type/Value               |
|----------------------------|---------------------------------------------------------------|--------------------------|
| telemetry.exporterEndpoint | Endpoint that can be connected to by scrapers for metric data | "http://localhost:55690" |
| loki.enabled               | Enable Loki for logging                                       | true or false            |
| loki.url                   | URL for Loki                                                  | "http://localhost:3100"  |
| loki.app                   | Application name for Loki                                     | "link-dev"               |

## Swagger

| Property Name                | Description                              | Type/Value                                                                   |
|------------------------------|------------------------------------------|------------------------------------------------------------------------------|
| springdoc                    | Configuration for Swagger and Swagger UI | See [Springdoc documentation](https://springdoc.org/#properties) for details |
| springdoc.api-docs.enabled   | Enable Swagger specification generation  | true or false (default)                                                      |
| springdoc.swagger-ui.enabled | Enable Swagger UI                        | true or false (default)                                                      |

## Databases

### Mongo DB

| Property Name                | Description                          | Type/Value    | Secret? |
|------------------------------|--------------------------------------|---------------|---------|
| spring.data.mongodb.host     | Host address for the Mongo database  | "localhost"   | No      |
| spring.data.mongodb.port     | Port for the Mongo database          | 27017         | No      |
| spring.data.mongodb.database | Database name for the Mongo database | "measureeval" | No      |
| spring.data.mongodb.username | Username for the Mongo database      | \<string>     | No      |
| spring.data.mongodb.password | Password for the Mongo database      | \<string>     | Yes     |

### SQL Server

| Property Name                  | Description                          | Type/Value                                         | Secret? |
|--------------------------------|--------------------------------------|----------------------------------------------------|---------|
| spring.datasource.url          | URL for the SQL Server database      | \<string> prefixed with "jdbc:sqlserver://"        | No      |
| spring.datasource.username     | Username for the SQL Server database | \<string>                                          | No      |
| spring.datasource.password     | Password for the SQL Server database | \<string>                                          | Yes     |
| spring.jpa.hibernate.ddl-auto  | DDL auto setting for JPA/Hibernate   | "none" (default) or "update"                       | No      |
| spring.jpa.properties.show_sql | Show SQL statements in logs          | true (default) or false                            | No      |
| spring.jpa.properties.dialect  | SQL dialect for the database         | "org.hibernate.dialect.SQLServerDialect" (default) | No      |

### Auto Update/Migrate DBs

| Property Name                 | Description                                                       | Type/Value                                          | Secret? |
|-------------------------------|-------------------------------------------------------------------|-----------------------------------------------------|---------|
| spring.jpa.hibernate.ddl-auto | Indicates whether how to update the schema in hibernate databases | create \| create-drop \| update \| validate \| none | No      |

## Kafka

| Property Name                       | Description                                                       | Type/Value       | Secret? |
|-------------------------------------|-------------------------------------------------------------------|------------------|---------|
| spring.kafka.bootstrap-servers      | Kafka bootstrap servers                                           | "localhost:9092" | No      |
| spring.kafka.consumer.group-id      | Kafka consumer group ID                                           | "measureeval"    | No      |
| spring.kafka.producer.client-id     | Kafka producer client ID                                          | "measureeval"    | No      |
| spring.kafka.retry.maxAttempts      | Maximum number of times consumption of an event should be retried | 3                | No      |
| spring.kafka.retry.retry-backoff-ms | Time in milliseconds to wait before retrying a failed event       | 3000             | No      |

## Service Authentication

| Property Name                   | Description                                                                                                                | Type/Value              | Secret? |
|---------------------------------|----------------------------------------------------------------------------------------------------------------------------|-------------------------|---------|
| secret-management.key-vault-uri | URI for the Azure Key Vault                                                                                                | \<string>               | Yes     |
| authentication.adminEmail       | Email address representing the Link administrator account                                                                  | \<string>               | No      |
| authentication.anonymous        | Whether the service should allow anonmyous users access to the services. This should onyl be enabled for DEV environments. | true or false (default) | No      |
| authentication.authority        | Authority for the service to authenticate against.                                                                         | "http://localhost:7004" | No      |
| authentication.signingKey       | Signing key for generating/verifying JWTs                                                                                  | \<string>               | Yes     |
