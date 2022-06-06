using Microsoft.EntityFrameworkCore;
using Ecom_Onboarding.DAL.Models;
using Ecom_Onboarding.DAL.Repository;

namespace Ecom_Onboarding.DAL.Repository
{
    public class PublisherRepository : BaseRepository<Publisher>
    {
        public PublisherRepository(DbContext dbContext) : base(dbContext) { }
    }
}
