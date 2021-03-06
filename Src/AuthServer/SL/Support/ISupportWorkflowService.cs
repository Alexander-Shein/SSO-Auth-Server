﻿using System.Threading.Tasks;
using DddCore.Contracts.BLL.Errors;
using DddCore.Contracts.SL.Services.Application;

namespace AuthGuard.SL.Support
{
    public interface ISupportWorkflowService : IWorkflowService
    {
        Task<(MessageVm Message, OperationResult OperationResult)> SendMessage(MessageIm im);
    }
}