﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServerWithAspNetIdentity.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string phoneNumber, string templateName, IDictionary<string, string> parameters);
    }
}
