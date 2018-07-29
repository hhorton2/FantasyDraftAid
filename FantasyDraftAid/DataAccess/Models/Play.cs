using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FantasyDraftAid.DataAccess.Models
{
    [Table("play_player")]
    public class Play
    {
        [Key] [Column("gsis_id", Order = 1)] public string GameId { get; set; }
        [Key] [Column("drive_id", Order = 2)] public string DriveId { get; set; }
        [Key] [Column("play_id", Order = 3)] public string PlayId { get; set; }
        [Key] [Column("player_id", Order = 4)] public string PlayerId { get; set; }
        [Column("fumbles_tot")] public int Fumbles { get; set; }
        [Column("fumbles_lost")] public int FumblesLost { get; set; }
        [Column("kicking_fgm")] public int FieldGoals { get; set; }
        [Column("kicking_fgm_yds")] public int FieldGoalYards { get; set; }
        [Column("kicking_xpmade")] public int Pat { get; set; }
        [Column("passing_int")] public int PassingInterception { get; set; }
        [Column("passing_twoptm")] public int PassingTwoPointConversion { get; set; }
        [Column("passing_yds")] public int PassingYards { get; set; }
        [Column("passing_tds")] public int PassingTouchdowns { get; set; }
        [Column("receiving_rec")] public int Receptions { get; set; }
        [Column("receiving_tds")] public int ReceivingTouchdowns { get; set; }
        [Column("receiving_twoptm")] public int ReceivingTwoPointConversion { get; set; }
        [Column("receiving_yds")] public int ReceivingYards { get; set; }
        [Column("rushing_yds")] public int RushingYards { get; set; }
        [Column("rushing_tds")] public int RushingTouchdowns { get; set; }
        [Column("rushing_twoptm")] public int RushingTwoPointConversion { get; set; }
        [ForeignKey(nameof(PlayerId))] public Player Player { get; set; }
        [ForeignKey(nameof(GameId))] public Game Game { get; set; }
    }
}