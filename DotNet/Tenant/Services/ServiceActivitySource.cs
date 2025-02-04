﻿using LantanaGroup.Link.Tenant.Config;
using LantanaGroup.Link.Tenant.Models;
using System.Diagnostics;
using LantanaGroup.Link.Shared.Application.Models;

namespace LantanaGroup.Link.Tenant.Services
{
    public class ServiceActivitySource
    {
        private static string _version = string.Empty;
        public static string ServiceName = TenantConstants.ServiceName;
        public static ActivitySource Instance { get; private set; } = new ActivitySource(ServiceName, _version);

        public static void Initialize(ServiceInformation serviceInfo)
        {
            _version = serviceInfo.Version;
            Instance = new ActivitySource(ServiceName, _version);
        }
    }
}
