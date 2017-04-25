﻿using System;
using System.Collections.Generic;
using DddCore.BLL.Domain.Entities.GuidEntities;
using DddCore.Contracts.BLL.Domain.Entities.Audit.At;

namespace AuthGuard.BLL.Domain.Entities
{
    public class App : GuidAggregateRootEntityBase, ICreatedAt
    {
        public string UserId { get; set; }

        public string DisplayName { get; set; }
        public string Key { get; set; }
        public string WebsiteUrl { get; set; }
        public bool IsLocalAccountEnabled { get; set; }
        public bool IsRememberLogInEnabled { get; set; }
        public bool IsSecurityQuestionsEnabled { get; set; }
        public bool IsActive { get; set; }
        public int UsersCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public LocalAccountSettings EmailSettings { get; set; }
        public LocalAccountSettings PhoneSettings { get; set; }
        public ICollection<AppExternalProvider> ExternalProviders { get; set; }
    }

    public class AppExternalProvider : GuidEntityBase
    {
        public Guid AppId { get; set; }
        public Guid ExternalProviderId { get; set; }
        public ExternalProvider ExternalProvider { get; set; }
    }
}