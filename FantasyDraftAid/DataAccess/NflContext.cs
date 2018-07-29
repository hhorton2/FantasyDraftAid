using FantasyDraftAid.DataAccess.Enums;
using FantasyDraftAid.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Npgsql.NameTranslation;

namespace FantasyDraftAid.DataAccess
{
    public class NflContext : DbContext
    {
        static NflContext()
        {
            NpgsqlConnection.GlobalTypeMapper.MapEnum<PlayerPosition>("player_pos", new NpgsqlNullNameTranslator());
            NpgsqlConnection.GlobalTypeMapper.MapEnum<PlayerStatus>("player_status", new NpgsqlNullNameTranslator());
        }

        public NflContext(DbContextOptions<NflContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Play>()
                .HasKey(c => new {c.GameId, c.DriveId, c.PlayId, c.PlayerId});
            builder.ForNpgsqlHasEnum("player_pos",
                new[]
                {
                    "C", "CB", "DB", "DE", "DL", "DT", "FB", "FS", "G", "ILB", "K", "LB", "LS", "MLB", "NT", "OG", "OL",
                    "OLB", "OT", "P", "QB", "RB", "SAF", "SS", "T", "TE", "WR", "UNK"
                });
        }

        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Play> Plays { get; set; }
        public DbSet<Game> Games { get; set; }
    }
}