﻿using FluentValidation;
using LantanaGroup.Link.LinkAdmin.BFF.Application.Models.Integration;

namespace LantanaGroup.Link.LinkAdmin.BFF.Application.Validators
{
    public class ReportScheduledValidator : AbstractValidator<ReportScheduled>
    {
        public ReportScheduledValidator()
        {
            RuleFor(x => x.FacilityId)
                .NotEmpty()
                    .WithMessage("FacilityId is required");

            RuleFor(x => x.ReportTypes)
                .NotEmpty()
                    .WithMessage("ReportType is required");

            RuleFor(x => x.StartDate)
                .NotEmpty()
                    .WithMessage("StartDate is required")
                .Must((x, y) => x.StartDate < DateTime.Now)
                    .WithMessage("StartDate must be in the past");
            RuleFor(x => x.Delay)
              .NotEmpty()
                  .WithMessage("Delay is required")
                .Must(delay => double.TryParse(delay, out _))
                 .WithMessage("Delay must be a valid number");

        }
    }
}
