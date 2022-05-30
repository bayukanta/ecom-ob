using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Ecom_Onboarding.DAL.Models;
using Ecom_Onboarding.DAL.Interface;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ecom_Onboarding.DAL.Repository
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private readonly OnBoardingSkdDbContext dbContext;

        public IBaseRepository<Game> GameRepository { get; }
        public IBaseRepository<Publisher> PublisherRepository { get; }


        public UnitOfWork(OnBoardingSkdDbContext context)
        {
            dbContext = context;

            GameRepository = new GameRepository(context);
            PublisherRepository = new PublisherRepository(context);
        }

        public void Save()
        {
            dbContext.SaveChanges();
        }

        public Task SaveAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return dbContext.SaveChangesAsync(cancellationToken);
        }

        public IDbContextTransaction StartNewTransaction()
        {
            return dbContext.Database.BeginTransaction();
        }

        public Task<IDbContextTransaction> StartNewTransactionAsync()
        {
            return dbContext.Database.BeginTransactionAsync();
        }

        public Task<int> ExecuteSqlCommandAsync(string sql, object[] parameters, CancellationToken cancellationToken = default(CancellationToken))
        {
            return dbContext.Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    dbContext?.Dispose();
                }
                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
