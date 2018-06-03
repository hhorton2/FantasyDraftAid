using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FantasyDraftAid.DataAccess.Models
{
    [Table("team")]
    public class Team
    {
        [Key] [Column("team_id")] public string Id { get; set; }
        [Column("city")] public string City { get; set; }
        [Column("name")] public string Name { get; set; }
    }
}