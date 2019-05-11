using System.Threading.Tasks;
using FlatRent.Entities;

namespace FlatRent.Services.Interfaces
{
    public interface IIncidentService
    {
        Task SendIncidentEmail(Incident incident);
    }
}