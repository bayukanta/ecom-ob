using Microsoft.EntityFrameworkCore;
using Ecom_Onboarding.Models;
using Ecom_Onboarding.Repositories;

namespace Ecom_Onboarding.Repository
{
    public class PublisherRepository : BaseRepository<Publisher>
    {
        public PublisherRepository(DbContext dbContext) : base(dbContext) { }
    }
}
