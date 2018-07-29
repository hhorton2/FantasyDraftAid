using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FantasyDraftAid.DataAccess.Models
{
    [Table("game")]
    public class Game
    {
        [Key] [Column("gsis_id")] public string Id { get; set; }
        [Column("week")] public int Week { get; set; }
        [Column("season_year")] public int SeasonYear { get; set; }
        [Column("season_type")] public string SeasonType { get; set; }
    }
}