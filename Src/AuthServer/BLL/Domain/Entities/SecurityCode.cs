﻿using System;
using System.Collections.Generic;
using System.Linq;
using DddCore.BLL.Domain.Entities.GuidEntities;

namespace AuthGuard.BLL.Domain.Entities
{
    public class SecurityCode : GuidAggregateRootBase
    {
        const int Min = 1000;
        const int Max = 9999;
        static readonly TimeSpan DefaultExpireTime = TimeSpan.FromHours(1);

        public int Code { get; set; }
        public DateTime ExpiredAt { get; set; }

        public SecurityCodeAction SecurityCodeAction { get; set; }

        public ICollection<SecurityCodeParameter> Parameters { get; set; }

        public static SecurityCode Generate(SecurityCodeAction action, SecurityCodeParameterName paramName, string paramValue)
        {
            return Generate(action, paramName, paramValue, DefaultExpireTime);
        }

        public static SecurityCode Generate(SecurityCodeAction action, SecurityCodeParameterName paramName, string paramValue, TimeSpan expirationTime)
        {
            return new SecurityCode
            {
                Code = GenerateRandomNumber(),
                SecurityCodeAction = action,
                ExpiredAt = DateTime.Now.Add(expirationTime),
                Parameters = new List<SecurityCodeParameter>
                {
                    new SecurityCodeParameter
                    {
                         Name = paramName,
                         Value = paramValue
                    }
                }
            };
        }

        public void AddParameter(SecurityCodeParameterName paramName, string value)
        {
            Parameters.Add(new SecurityCodeParameter
            {
                Name = paramName,
                Value = value
            });
        }

        public string GetParameterValue(SecurityCodeParameterName paramName)
        {
            return Parameters.Single(x => x.Name == paramName).Value;
        }

        private static int GenerateRandomNumber()
        {
            var rdm = new Random();
            return rdm.Next(Min, Max);
        }

        public static SecurityCode GenerateAddLocalProvider(string userName, Guid userId)
        {
            var code = Generate(SecurityCodeAction.AddLocalProvider, SecurityCodeParameterName.UserName, userName);
            code.AddParameter(SecurityCodeParameterName.UserId, userId.ToString());
            return code;
        }
    }
}