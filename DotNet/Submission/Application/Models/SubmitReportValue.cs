﻿using Hl7.Fhir.Model;

namespace LantanaGroup.Link.Submission.Application.Models
{
    public class SubmitReportValue
    {
        public List<string>? PatientIds { get; set; }
        public Organization? Organization { get; set; }
        public List<MeasureReport> Aggregates { get; set; } = [];
        public List<string> MeasureIds { get; set; } = [];
    }
}
