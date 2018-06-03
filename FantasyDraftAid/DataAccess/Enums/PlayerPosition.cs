using System.ComponentModel;

namespace FantasyDraftAid.DataAccess.Enums
{
    public enum PlayerPosition
    {
        [Description("Center")] C,
        [Description("Corner Back")] CB,
        [Description("Defensive Back")] DB,
        [Description("Defensive End")] DE,
        [Description("Defensive Lineman")] DL,
        [Description("Defensive Tackle")] DT,
        [Description("Full Back")] FB,
        [Description("Free Safety")] FS,
        [Description("Guard")] G,
        [Description("Inside Linebacker")] ILB,
        [Description("Kicker")] K,
        [Description("Linebacker")] LB,
        [Description("Long Snapper")] LS,
        [Description("Middle Linebacker")] MLB,
        [Description("Nose Tackle")] NT,
        [Description("Offensive Guard")] OG,
        [Description("Offensive Lineman")] OL,
        [Description("Outside Linebacker")] OLB,
        [Description("Offensive Tackle")] OT,
        [Description("Punter")] P,
        [Description("Quarterback")] QB,
        [Description("Running Back")] RB,
        [Description("Safety")] SAF,
        [Description("Strong Safety")] SS,
        [Description("Tackle")] T,
        [Description("Tight End")] TE,
        [Description("Wide Receiver")] WR,
        [Description("Unknown")] UNK
    }
}