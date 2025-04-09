using System.Threading;
using System.Threading.Tasks;
using UserManagement.Application.Common.Interfaces.Repositories;

namespace UserManagement.Application.Common.Interfaces
{
    public interface IUnitOfWork
    {        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}