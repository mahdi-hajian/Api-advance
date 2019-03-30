using System.Threading;
using System.Threading.Tasks;
using Entities;

namespace Data.Contracts
{
    public interface IUserRepository: IRepository<User>
    {
        Task UpdateSecuirtyStampAsync(User user, CancellationToken cancellationToken);
        Task UpdateLastLoginDateAsync(User user, CancellationToken cancellationToken);
    }
}