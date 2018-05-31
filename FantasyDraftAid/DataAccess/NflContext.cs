using Microsoft.EntityFrameworkCore;

namespace FantasyDraftAid.DataAccess
{
    public class NflContext : DbContext
    {
        public NflContext(DbContextOptions<NflContext> options) : base(options)
        {
        }
    }
}