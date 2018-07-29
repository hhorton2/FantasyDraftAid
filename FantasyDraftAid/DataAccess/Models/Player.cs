using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FantasyDraftAid.DataAccess.Enums;

namespace FantasyDraftAid.DataAccess.Models
{
    [Table("player")]
    public class Player
    {
        [Key] [Column("player_id")] public string Id { get; set; }
        [Column("full_name")] public string FullName { get; set; }
        [Column("first_name")] public string FirstName { get; set; }
        [Column("last_name")] public string LastName { get; set; }
        [Column("team")] public string TeamId { get; set; }
        [ForeignKey(nameof(TeamId))] public Team Team { get; set; }
        [Column("years_pro")] public int? YearsPro { get; set; }
        [Column("position")] public PlayerPosition Position { get; set; }
        [Column("profile_url")] public string ProfileUrl { get; set; }
        [Column("status")] public PlayerStatus PlayerStatus { get; set; }
    }
}