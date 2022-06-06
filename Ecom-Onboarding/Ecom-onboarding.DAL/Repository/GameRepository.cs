using Microsoft.EntityFrameworkCore;
using Ecom_Onboarding.DAL.Models;
using Ecom_Onboarding.DAL.Repository;

namespace Ecom_Onboarding.DAL.Repository
{
    public class GameRepository : BaseRepository<Game>
    {
        public GameRepository(DbContext dbContext) : base(dbContext) { }
    }
}
