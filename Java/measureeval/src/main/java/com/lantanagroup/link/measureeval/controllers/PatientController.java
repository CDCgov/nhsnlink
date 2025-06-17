package com.lantanagroup.link.measureeval.controllers;

import com.lantanagroup.link.measureeval.repositories.PatientReportingEvaluationStatusTemplateRepository;
import com.lantanagroup.link.measureeval.services.PatientStatusBundler;
import org.hl7.fhir.r4.model.Bundle;
import org.springframework.http.HttpStatus;
import org.springframework.security.access.prepost.PreAuthorize;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.server.ResponseStatusException;

@RestController
@RequestMapping("/api/patient")
@PreAuthorize("hasRole('LinkUser')")
public class PatientController {
    private final PatientReportingEvaluationStatusTemplateRepository patientReportingEvaluationStatusTemplateRepository;
    private final PatientStatusBundler patientStatusBundler;

    public PatientController(PatientReportingEvaluationStatusTemplateRepository patientReportingEvaluationStatusTemplateRepository, PatientStatusBundler patientStatusBundler) {
        this.patientReportingEvaluationStatusTemplateRepository = patientReportingEvaluationStatusTemplateRepository;
        this.patientStatusBundler = patientStatusBundler;
    }

    @GetMapping("/{facilityId}/{reportId}/{patientId}")
    public Bundle getPatientData(@PathVariable String facilityId, @PathVariable String reportId, @PathVariable String patientId) {
        var patientReportStatus = patientReportingEvaluationStatusTemplateRepository.getFirstByFacilityIdAndPatientIdAndReports_ReportTrackingId(facilityId, patientId, reportId);

        if (patientReportStatus == null) {
            throw new ResponseStatusException(HttpStatus.NOT_FOUND, "facilityId, reportId, or patientId not found");
        }

        return patientStatusBundler.createBundle(patientReportStatus);
    }
}
