﻿using Newtonsoft.Json;
using QueryDispatch.Application.Models;
using System.Runtime.Serialization;

namespace LantanaGroup.Link.QueryDispatch.Application.Models
{
    [DataContract]
    public class ReportScheduledValue
    {
        [DataMember]
        public List<string> ReportTypes { get; set; }
        [DataMember]
        public Frequency Frequency { get; set; }
        [DataMember]
        public DateTimeOffset StartDate { get; set; }
        [DataMember]
        public DateTimeOffset EndDate { get; set; }

        public bool IsValid()
        {
            if (ReportTypes == null || ReportTypes.Count <= 0 || StartDate == default || EndDate == default)
            {
                return false;
            }

            return true;
        }
    }
}
