using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlatRent.Entities;
using FlatRent.Models;

namespace FlatRent.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<FormError>> AddClientAsync(User client);
        Task<IEnumerable<FormError>> AddEmployeeAsync(User employee);
        Task<User> GetUserForCredentialsAsync(string email, string password);
    }
}