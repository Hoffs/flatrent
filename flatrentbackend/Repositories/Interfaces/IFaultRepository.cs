﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlatRent.Entities;
using FlatRent.Models;
using FlatRent.Models.Requests;

namespace FlatRent.Repositories.Interfaces
{
    public interface IFaultRepository
    {
        Task<(IEnumerable<FormError>, Fault)> CreateFaultAsync(Guid agreementId, FaultForm form, Guid userId);
        Task<IEnumerable<FormError>> UpdateAsync(Fault entity);
        Task<IEnumerable<FormError>> DeleteAsync(Fault entity);
    }
}