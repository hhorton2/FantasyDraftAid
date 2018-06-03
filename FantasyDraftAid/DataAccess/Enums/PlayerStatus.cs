using System.ComponentModel;

namespace FantasyDraftAid.DataAccess.Enums
{
    public enum PlayerStatus
    {
        [Description("Active")] Active,
        [Description("InjuredReserve")] InjuredReserve,
        [Description("NonFootballInjury")] NonFootballInjury,
        [Description("Suspended")] Suspended,
        [Description("PUP")] PUP,
        [Description("UnsignedDraftPick")] UnsignedDraftPick,
        [Description("Exempt")] Exempt,
        [Description("Unknown")] Unknown
    }
}