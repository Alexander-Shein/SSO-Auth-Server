﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthGuard.BLL.Domain.Entities;
using AuthGuard.Data;
using AuthGuard.Services.Security;
using AuthGuard.Services.TwoFactor.Models.Input;
using AuthGuard.Services.TwoFactor.Models.View;
using DddCore.Contracts.BLL.Errors;
using Microsoft.AspNetCore.Identity;

namespace AuthGuard.Services.TwoFactor
{
    public class TwoFactorsService : ITwoFactorsService
    {
        readonly UserManager<ApplicationUser> userManager;
        readonly SignInManager<ApplicationUser> signInManager;
        readonly IEmailSender emailSender;
        readonly ISmsSender smsSender;
        readonly ISecurityCodesService securityCodesService;
        readonly ApplicationDbContext context;

        public TwoFactorsService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ISmsSender smsSender,
            ISecurityCodesService securityCodesService,
            ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailSender = emailSender;
            this.smsSender = smsSender;
            this.securityCodesService = securityCodesService;
            this.context = context;
        }

        public async Task<(IEnumerable<TwoFactorProviderVm> Providers, OperationResult OperationResult)> GetTwoFactorProvidersAsync()
        {
            var user = await signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                return (null, OperationResult.FailedResult(1, "User is not logged in for 2 factor validation."));
            }

            var userFactors = await userManager.GetValidTwoFactorProvidersAsync(user);
            var providers = userFactors.Select(x => new TwoFactorProviderVm
            {
                DisplayName = x,
                Key = x
            });

            return (providers, OperationResult.SucceedResult);
        }

        public async Task<OperationResult> SendCodeAsync(TwoFactorProviderIm im)
        {
            var provider = im.Key;

            var user = await signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return OperationResult.FailedResult(1, "User is not logged in for 2 factor validation.");
            }

            var securityCode = SecurityCode.Generate(SecurityCodeAction.TwoFactorVerification, SecurityCodeParameterName.UserId, user.Id);
            securityCode.AddParameter(SecurityCodeParameterName.LocalProvider, im.Key);

            securityCodesService.Insert(securityCode);

            var parameters = new Dictionary<string, string>
            {
                {"Code", securityCode.Code.ToString()}
            };

            if (String.Equals(provider, "Email", StringComparison.OrdinalIgnoreCase))
            {
                await emailSender.SendEmailAsync(await userManager.GetEmailAsync(user), Template.TwoFactorCode, parameters);
            }

            if (String.Equals(provider, "Phone", StringComparison.OrdinalIgnoreCase))
            {
                await smsSender.SendSmsAsync(await userManager.GetPhoneNumberAsync(user), Template.TwoFactorCode, parameters);
            }

            await context.SaveChangesAsync();

            return OperationResult.SucceedResult;
        }

        public async Task<OperationResult> VerifyCode(TwoFactorVerificationIm im)
        {
            var user = await signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return OperationResult.FailedResult(1, "User is not logged in for 2 factor validation.");
            }

            var securityCode = await securityCodesService.Get(im.Code);

            if (securityCode == null || user.Id != securityCode.GetParameterValue(SecurityCodeParameterName.UserId))
            {
                return OperationResult.FailedResult(2, "Invalid code.");
            }

            var provider = securityCode.GetParameterValue(SecurityCodeParameterName.LocalProvider);
            var code = await userManager.GenerateTwoFactorTokenAsync(user, provider);

            var signInResult = await signInManager.TwoFactorSignInAsync(provider, code, im.RememberLogIn, im.RememberBrowser);

            securityCodesService.Delete(securityCode);

            await context.SaveChangesAsync();

            if (signInResult.IsLockedOut)
            {
                return OperationResult.FailedResult(3, "Account is locked.");
            }

            return OperationResult.SucceedResult;
        }
    }
}