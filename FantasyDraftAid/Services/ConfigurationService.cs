using System.IO;
using FantasyDraftAid.Configuration;
using FantasyDraftAid.Views;
using Microsoft.Extensions.Configuration;

namespace FantasyDraftAid.Services
{
    public class ConfigurationService
    {
        private static readonly IConfigurationRoot Config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("leaguePoints.json")
            .Build();

        public FantasyConfiguration GetConfiguration()
        {
            return new FantasyConfiguration
            {
                FieldGoalLessThanFifty = Config.GetValue<int>("FieldGoalLessThanFifty"),
                FieldGoalLessThanForty = Config.GetValue<int>("FieldGoalLessThanForty"),
                FieldGoalLessThanThirty = Config.GetValue<int>("FieldGoalLessThanThirty"),
                FieldGoalLessThanTwenty = Config.GetValue<int>("FieldGoalLessThanTwenty"),
                FieldGoalMax = Config.GetValue<int>("FieldGoalMax"),
                Pat = Config.GetValue<int>("Pat"),
                Fumble = Config.GetValue<int>("Fumble"),
                FumbleLost = Config.GetValue<int>("FumbleLost"),
                FumbleReturn = Config.GetValue<int>("FumbleReturn"),
                Interception = Config.GetValue<int>("Interception"),
                PassingTouchdown = Config.GetValue<int>("PassingTouchdown"),
                PassingYardsPerPoint = Config.GetValue<int>("PassingYardsPerPoint"),
                PickSix = Config.GetValue<int>("PickSix"),
                ReceivingTouchdown = Config.GetValue<int>("ReceivingTouchdown"),
                ReceivingYardsPerPoint = Config.GetValue<int>("ReceivingYardsPerPoint"),
                Reception = Config.GetValue<int>("Reception"),
                ReturnTouchdown = Config.GetValue<int>("ReturnTouchdown"),
                RushingTouchdown = Config.GetValue<int>("RushingTouchdown"),
                RushingYardsPerPoint = Config.GetValue<int>("RushingYardsPerPoint"),
                TwoPointConversion = Config.GetValue<int>("TwoPointConversion"),
                DefBlockKick = Config.GetValue<int>("DefBlockKick"),
                DefFumbleRecovery = Config.GetValue<int>("DefFumbleRecovery"),
                DefInterception = Config.GetValue<int>("DefInterception"),
                DefMaxPoints = Config.GetValue<int>("DefMaxPoints"),
                DefNoPoints = Config.GetValue<int>("DefNoPoints"),
                DefPatReturn = Config.GetValue<int>("DefPatReturn"),
                DefReturnTouchdowns = Config.GetValue<int>("DefReturnTouchdowns"),
                DefSack = Config.GetValue<int>("DefSack"),
                DefSafety = Config.GetValue<int>("DefSafety"),
                DefSixPoints = Config.GetValue<int>("DefSixPoints"),
                DefThirteenPoints = Config.GetValue<int>("DefThirteenPoints"),
                DefThirtyFourPoints = Config.GetValue<int>("DefThirtyFourPoints"),
                DefTouchdown = Config.GetValue<int>("DefTouchdown"),
                DefTwentyPoints = Config.GetValue<int>("DefTwentyPoints"),
                DefTwentySevenPoints = Config.GetValue<int>("DefTwentySevenPoints")
            };
        }
    }
}