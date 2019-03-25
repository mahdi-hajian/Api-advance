using System.Threading;
using System.Threading.Tasks;
using Entities;

namespace Data.Contracts
{
    public interface IUserRepository: IRepository<User>
    {
        Task<User> GetByUserAndPass(string userName, string password, CancellationToken cancellationToken);
    }
}