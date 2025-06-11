import { PaginationMetadata } from "src/app/models/pagination-metadata.model";

export interface IReportListSummary
{
    id: string;
    facilityId: string;
    reportStartDate: Date;
    reportEndDate: Date;
    submitted: boolean
    submitDate: Date;
    reportTypes: string[];
    frequency: string;
    censusCount: number;
    initialPopulationCount: number;
    reportMetrics: IScheduledReportMetrics;
}

export interface ICensusCount
{
    admittedPatients: number;
    dischargedPatients: number;
}

export interface IScheduledReportMetrics
{
  measureIpCounts: Record<string, number>;
  reportStatusCounts: Record<string, number>;
  validationStatusCounts: Record<string, number>;
}

export class IPagedReportListSummary {
  records: IReportListSummary[] = [];
  metadata: PaginationMetadata = new PaginationMetadata;
}

export interface IMeasureReportSummary
{
  id: string;
  patientId: string;
  reportType: string;
  status: string;
  validationStatus: string;
  resourceCount: number;
  resourceCountSummary: Record<string, number>;
}

export class IPagedMeasureReportSummary
{
  records: IMeasureReportSummary[] = [];
  metadata: PaginationMetadata = new PaginationMetadata;
}

export interface IResourceSummary
{
  facilityId: string;
  measureReportId: string;
  patientId: string;
  resourceId: string;
  fhirId: string;
  resourceType: string;
  resourceCategory: string;
  reference: string;
}

export class IPagedResourceSummary
{
  records: IResourceSummary[] = [];
  metadata: PaginationMetadata = new PaginationMetadata;
}