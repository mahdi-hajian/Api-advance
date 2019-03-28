using Common.Utilities;
using Data.Contracts;
using Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Exceptions;
using System;
using System.Collections.Generic;
using Common;

namespace Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository, IScopedDependency
    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public Task<User> GetByUserAndPass(string userName, string password, CancellationToken cancellationToken)
        {
            var passwordHash = SecurityHelper.GetSha256Hash(password);
            return Table.Where(c => c.UserName == userName && c.PasswordHash == passwordHash).SingleOrDefaultAsync(cancellationToken);
        }

        //public async Task AddAsync(User user, string password, CancellationToken cancellationToken)
        //{

        //    var exist = await TableNoTracking.AnyAsync(c => c.UserName == user.UserName);
        //    if (exist)
        //        throw new BadRequestException("نام کاربری تکراری است");

        //    var passwordHash = SecurityHelper.GetSha256Hash(password);
        //    user.PasswordHash = passwordHash;
        //    await base.AddAsync(user, cancellationToken);
        //}

        public Task UpdateSecuirtyStampAsync(User user, CancellationToken cancellationToken)
        {
            user.SecurityStamp = Guid.NewGuid().ToString();
            return UpdateAsync(user, cancellationToken);
        }

        public Task UpdateLastLoginDateAsync(User user, CancellationToken cancellationToken)
        {
            user.LastLoginDate = DateTimeOffset.Now;
            return UpdateAsync(user, cancellationToken);
        }

    }
}
