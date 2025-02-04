﻿using Confluent.Kafka;
using LantanaGroup.Link.LinkAdmin.BFF.Application.Models.Integration;
using LantanaGroup.Link.LinkAdmin.BFF.Infrastructure;
using LantanaGroup.Link.LinkAdmin.BFF.Infrastructure.Logging;
using LantanaGroup.Link.Shared.Application.Models;
using OpenTelemetry.Trace;
using System.Diagnostics;
using System.Text.Json;

namespace LantanaGroup.Link.LinkAdmin.BFF.Application.Commands.Integration
{
    public class CreateReportScheduled : ICreateReportScheduled
    {
        private readonly ILogger<CreateReportScheduled> _logger;
        private readonly IProducer<string, object> _producer;
        private const double DEFAULT_DELAY_MINUTES = 5;
        private const double MAX_DELAY_MINUTES = 60 * 24;

        public CreateReportScheduled(ILogger<CreateReportScheduled> logger, IProducer<string, object> producer)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _producer = producer ?? throw new ArgumentNullException(nameof(producer));
        }

        public async Task<string> Execute(ReportScheduled model, string? userId = null)
        {
            using Activity? activity = ServiceActivitySource.Instance.StartActivity("Producing Report Scheduled Event");
            string correlationId = Guid.NewGuid().ToString();

            try
            {
                var headers = new Headers
                {
                    { "X-Correlation-Id", System.Text.Encoding.ASCII.GetBytes(correlationId) }
                };

                string Key = string.IsNullOrEmpty(model.FacilityId) ? throw new ArgumentException("FacilityId cannot be null or empty", nameof(model.FacilityId)) : model.FacilityId;

                DateTime EndDate = DateTime.UtcNow;

                if (double.TryParse(model.Delay, out double delay))
                {
                    if (delay < 0)
                    {
                        throw new ArgumentException("Delay cannot be negative", nameof(model.Delay));
                    }
                    if (delay > MAX_DELAY_MINUTES)
                    {
                        throw new ArgumentException($"Delay cannot exceed {MAX_DELAY_MINUTES} minutes", nameof(model.Delay));
                    }
                    EndDate = DateTime.UtcNow.AddMinutes(delay);
                }
                else
                {
                    _logger.LogWarning("Invalid delay value '{Delay}'. Using default delay of {DefaultDelay} minutes", model.Delay, DEFAULT_DELAY_MINUTES);
                    EndDate = DateTime.UtcNow.AddMinutes(DEFAULT_DELAY_MINUTES); // default to 5 minutes
                }
               
                 DateTime normalizedEndDate = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, EndDate.Hour, EndDate.Minute, 0, DateTimeKind.Utc);
                 if (model.ReportTypes == null || !model.ReportTypes.Any())
                 {
                    throw new ArgumentException("At least one report type must be specified", nameof(model.ReportTypes));
                 }
                
                 if (!Enum.IsDefined(typeof(Frequency), model.Frequency))
                 {
                    throw new ArgumentException("Invalid frequency value", nameof(model.Frequency));
                 }
                
                if (model.StartDate >= normalizedEndDate)
                {
                    throw new ArgumentException("Start date must be earlier than end date", nameof(model.StartDate));
                }
                
                var message = new Message<string, object>
                {
                    Key = model.FacilityId,
                    Headers = headers,
                    Value = new ReportScheduledMessage()
                    {
                        ReportTypes = model.ReportTypes,
                        Frequency = model.Frequency.ToString(),
                        StartDate = model.StartDate,
                        EndDate = normalizedEndDate,

                    },
                };

                await _producer.ProduceAsync(nameof(KafkaTopic.ReportScheduled), message);
                _logger.LogKafkaProducerReportScheduled(correlationId);

                return correlationId;

            }
            catch (Exception ex)
            {
                Activity.Current?.SetStatus(ActivityStatusCode.Error);
                Activity.Current?.RecordException(ex);
                _logger.LogKafkaProducerException(correlationId, ex.Message);
                throw;
            }

        }
    }
}
