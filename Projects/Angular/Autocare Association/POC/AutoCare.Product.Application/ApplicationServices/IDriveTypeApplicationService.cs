﻿using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Vcdb.Model;
using System.Threading.Tasks;

namespace AutoCare.Product.Application.ApplicationServices
{
    public interface IDriveTypeApplicationService : IVcdbApplicationService<DriveType>
    {
        new Task<DriveTypeChangeRequestStagingModel> GetChangeRequestStaging<TId>(TId changeRequestId);
    }
}