﻿using System.Collections.Generic;

namespace FlatRent.Models.Dtos
{
    public class UserAgreements
    {
        public IEnumerable<ShortAgreementDetails> OwnerAgreements { get; set; }
        public IEnumerable<ShortAgreementDetails> TenantAgreements { get; set; }
    }
}