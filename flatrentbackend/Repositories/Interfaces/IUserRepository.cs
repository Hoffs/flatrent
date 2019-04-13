﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlatRent.Entities;
using FlatRent.Models;

namespace FlatRent.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUser(Guid id);
        Task<IEnumerable<FormError>> AddClientAsync(User client);
        Task<User> GetUserForCredentialsAsync(string email, string password);
    }
}