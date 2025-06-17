﻿[← Back Home](../README.md)

## Census Overview

The Census service is primarily responsible for maintaining a tenants admit and discharge patient information needed to determine when a patient is ready for reporting. To accomplish this, the Census service has functionality in place to request an updated FHIR List of recently admitted patients. The frequency that the request is made is based on a Tenant configuration made in the Census service.

- **Technology**: .NET Core
- **Image Name**: link-census
- **Port**: 8080
- **Database**: MSSQL (previously Mongo)
- **Scale**: 0-3

See [Census Functionality](../functionality/census_management.md) for more information on the role of the Census service in the Link Cloud ecosystem.

## Common Configurations

* [Swagger](../config/csharp.md#swagger)
* [Azure App Configuration](../config/csharp.md#azure-app-config-environment-variables)
* [Kafka Configuration](../config/csharp.md#kafka)
* [Kafka Consumer Retry Configuration](../config/csharp.md#kafka-consumer-settings)
* [Service Registry Configuration](../config/csharp.md#service-registry)
* [CORS Configuration](../config/csharp.md#cors)
* [Token Service Configuration](../config/csharp.md#token-service-settings)
* [Service Authentication](../config/csharp.md#service-authentication)
* [SQL Server Database Configuration](../config/csharp.md#sql-server-database)

## Kafka Events/Topics

### Consumed Events

- **Event**: `PatientIDsAcquired`

### Produced Events

- **Event**: `PatientCensusScheduled`

## API Operations

The **Census** service provides REST endpoints for managing and querying census data. These endpoints allow you to retrieve, update, and manage historical and current census records for each facility.

### Census Data Management

- **GET /api/census/{facilityId}/current**: Retrieves the current census for a specific facility.
- **GET /api/census/{facilityId}/history**: Retrieves the census history for a specific facility.
- **GET /api/census/{facilityId}/history/admitted**: Retrieves a list of admitted patients for a facility within a specified date range. If no dates are provided, all active patients are returned.
- **GET /api/census/{facilityId}/all**: Retrieves all census data for a specific facility.

### Census Configuration Management

- **POST /api/census/config**: Creates a new CensusConfig for a facility.
- **GET /api/census/config/{facilityId}**: Retrieves the CensusConfig for a specific facility.
- **PUT /api/census/config/{facilityId}**: Updates the CensusConfig for a specific facility.
- **DELETE /api/census/config/{facilityId}**: Deletes the CensusConfig for a specific facility.

These endpoints enable tenants to manage census data and configurations effectively, ensuring accurate and consistent census management across facilities.