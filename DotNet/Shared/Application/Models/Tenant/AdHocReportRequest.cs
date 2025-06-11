﻿namespace LantanaGroup.Link.Shared.Application.Models.Tenant
{
    public class AdHocReportRequest
    {
        public bool? BypassSubmission { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<string>? ReportTypes { get; set; }
        public List<string>? PatientIds { get; set; }
    }
}
