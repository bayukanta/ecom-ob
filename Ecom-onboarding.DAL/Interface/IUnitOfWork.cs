using Microsoft.EntityFrameworkCore.Storage;
using Ecom_Onboarding.DAL.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ecom_Onboarding.DAL.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<Game> GameRepository { get; }
        IBaseRepository<Publisher> PublisherRepository { get; }
        void Save();
        Task SaveAsync(CancellationToken cancellationToken = default(CancellationToken));
        IDbContextTransaction StartNewTransaction();
        Task<IDbContextTransaction> StartNewTransactionAsync();
        Task<int> ExecuteSqlCommandAsync(string sql, object[] parameters, CancellationToken cancellationToken = default(CancellationToken));
    }
}
