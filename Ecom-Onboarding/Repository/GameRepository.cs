using Microsoft.EntityFrameworkCore;
using Ecom_Onboarding.Models;
using Ecom_Onboarding.Repositories;

namespace Ecom_Onboarding.Repository
{
    public class GameRepository : BaseRepository<Game>
    {
        public GameRepository(DbContext dbContext) : base(dbContext) { }
    }
}
