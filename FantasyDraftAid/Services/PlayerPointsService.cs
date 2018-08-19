using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FantasyDraftAid.Configuration;
using FantasyDraftAid.DataAccess;
using FantasyDraftAid.DataAccess.Enums;
using FantasyDraftAid.DataAccess.Models;
using FantasyDraftAid.Views;
using Microsoft.EntityFrameworkCore;

namespace FantasyDraftAid.Services
{
    public class PlayerPointsService
    {
        private readonly ConfigurationService _configurationService;
        private readonly NflContext _context;

        public PlayerPointsService(ConfigurationService configurationService, NflContext context)
        {
            _configurationService = configurationService;
            _context = context;
            _context.ChangeTracker.AutoDetectChangesEnabled = false;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public IEnumerable<FantasyPlayerView> GetFantasyPlayers(IEnumerable<Player> players, int year)
        {
            var config = _configurationService.GetConfiguration();
            var playersArray = players as Player[] ?? players.ToArray();
            var playerMap = new ConcurrentDictionary<string, Player>();
            Parallel.ForEach(playersArray, p => { playerMap[p.Id] = p; });
            var playerAggregatePrior = GetPlayerAggregates(playersArray, year);
            var playerAggregateTwo = GetPlayerAggregates(playersArray, year - 1);
            var playerTwoMap = new ConcurrentDictionary<string, FantasyPlayerAggregateView>();
            Parallel.ForEach(playerAggregateTwo, p => { playerTwoMap[p.Id] = p; });
            var playerAggregateThree = GetPlayerAggregates(playersArray, year - 2);
            var playerThreeMap = new ConcurrentDictionary<string, FantasyPlayerAggregateView>();
            Parallel.ForEach(playerAggregateThree, p => { playerThreeMap[p.Id] = p; });
            var returnPlayers = playerAggregatePrior.AsParallel()
                .Select(p => new FantasyPlayerView
                {
                    AverageGamesPlayedPerSeason = p.GamesPlayed,
                    FirstName = playerMap.GetValueOrDefault(p.Id).FirstName,
                    FullName = playerMap.GetValueOrDefault(p.Id).FullName,
                    LastName = playerMap.GetValueOrDefault(p.Id).LastName,
                    PlayerStatus = playerMap.GetValueOrDefault(p.Id).PlayerStatus.GetDescription(),
                    Position = playerMap.GetValueOrDefault(p.Id).Position.GetDescription(),
                    ProfileUrl = playerMap.GetValueOrDefault(p.Id).ProfileUrl,
                    TeamName = playerMap.GetValueOrDefault(p.Id).Team.Name,
                    YearsPro = playerMap.GetValueOrDefault(p.Id).YearsPro ?? 0,
                    PriorYearGamesPlayed = p.GamesPlayed,
                    PriorYearPoints = GetPoints(config, p),
                    PriorYearPointsPPG = decimal.ToInt32(p.GamesPlayed == 0 ? 1 : GetPoints(config, p) / p.GamesPlayed),
                    TwoYearGamesPlayed = playerTwoMap.GetOrAdd(p.Id, new FantasyPlayerAggregateView()).GamesPlayed,
                    TwoYearPoints = GetPoints(config, playerTwoMap.GetOrAdd(p.Id, new FantasyPlayerAggregateView())),
                    TwoYearPointsPPG =
                        decimal.ToInt32(
                            GetPoints(config, playerTwoMap.GetOrAdd(p.Id, new FantasyPlayerAggregateView())) /
                            GetGamesPlayedOrOne(playerTwoMap, p)),
                    ThreeYearGamesPlayed = playerThreeMap.GetOrAdd(p.Id, new FantasyPlayerAggregateView()).GamesPlayed,
                    ThreeYearPoints =
                        GetPoints(config, playerThreeMap.GetOrAdd(p.Id, new FantasyPlayerAggregateView())),
                    ThreeYearPointsPPG =
                        decimal.ToInt32(GetPoints(config,
                                            playerThreeMap.GetOrAdd(p.Id, new FantasyPlayerAggregateView())) /
                                        GetGamesPlayedOrOne(playerThreeMap, p)),
                    ThreeYearAverage = (GetPoints(config, p) +
                                        GetPoints(config,
                                            playerTwoMap.GetOrAdd(p.Id, new FantasyPlayerAggregateView())) +
                                        GetPoints(config,
                                            playerThreeMap.GetOrAdd(p.Id, new FantasyPlayerAggregateView())))
                                       / 3
                }).OrderByDescending(p => p.PriorYearPoints);
            return returnPlayers;
        }

        private static int GetGamesPlayedOrOne(IReadOnlyDictionary<string, FantasyPlayerAggregateView> playerMap,
            FantasyPlayerAggregateView player)
        {
            return playerMap.ContainsKey(player.Id) && playerMap[player.Id].GamesPlayed > 0
                ? playerMap[player.Id].GamesPlayed
                : 1;
        }

        private static int GetPoints(FantasyConfiguration config,
            FantasyPlayerAggregateView player)
        {
            return player.Id == null || string.IsNullOrEmpty(player.Id)
                ? 0
                : player.FieldGoalsFifty * config.FieldGoalLessThanFifty +
                  player.FieldGoalsForty * config.FieldGoalLessThanForty +
                  player.FieldGoalsThirty * config.FieldGoalLessThanThirty +
                  player.FieldGoalsTwenty * config.FieldGoalLessThanTwenty +
                  player.FieldGoalsMax * config.FieldGoalMax +
                  player.PassingInterceptions * config.Interception +
                  player.PassingTouchdowns * config.PassingTouchdown +
                  player.PassingTwoPointConversions * config.TwoPointConversion +
                  player.PassingYards / config.PassingYardsPerPoint +
                  player.ReceivingTouchdowns * config.ReceivingTouchdown +
                  player.ReceivingTwoPointConversions * config.TwoPointConversion +
                  player.ReceivingYards / config.ReceivingYardsPerPoint +
                  player.Receptions * config.Reception +
                  player.RushingTouchdowns * config.RushingTouchdown +
                  player.RushingTwoPointConversions * config.TwoPointConversion +
                  player.RushingYards / config.RushingYardsPerPoint +
                  player.Fumbles * config.Fumble +
                  player.FumblesLost * config.FumbleLost +
                  player.Pats * config.Pat;
        }

        private List<FantasyPlayerAggregateView> GetPlayerAggregates(IEnumerable<Player> players, int year)
        {
            var playerAggregate = players.SelectMany(player => GetFantasyPlayerRawViews(year, player))
                .GroupBy(p => p.Id)
                .Select(SelectAggregateView()).ToList();
            return playerAggregate;
        }

        private static Func<IGrouping<string, FantasyPlayerRawView>, FantasyPlayerAggregateView> SelectAggregateView()
        {
            return p => new FantasyPlayerAggregateView
            {
                FieldGoalsFifty = p.AsParallel().Sum(f => f.FieldGoalsFifty),
                FieldGoalsForty = p.AsParallel().Sum(f => f.FieldGoalsForty),
                FieldGoalsMax = p.AsParallel().Sum(f => f.FieldGoalsMax),
                PassingInterceptions = p.AsParallel().Sum(f => f.PassingInterceptions),
                Id = p.Key,
                GamesPlayed = p.AsParallel().Select(f => f.GameId).Distinct().Count(),
                FieldGoalsThirty = p.AsParallel().Sum(f => f.FieldGoalsThirty),
                Fumbles = p.AsParallel().Sum(f => f.Fumbles),
                RushingTouchdowns = p.AsParallel().Sum(f => f.RushingTouchdowns),
                PassingYards = p.AsParallel().Sum(f => f.PassingYards),
                ReceivingTwoPointConversions = p.AsParallel().Sum(f => f.ReceivingTwoPointConversions),
                ReceivingYards = p.AsParallel().Sum(f => f.ReceivingYards),
                FumblesLost = p.AsParallel().Sum(f => f.FumblesLost),
                RushingYards = p.AsParallel().Sum(f => f.RushingYards),
                FieldGoalsTwenty = p.AsParallel().Sum(f => f.FieldGoalsTwenty),
                ReceivingTouchdowns = p.AsParallel().Sum(f => f.ReceivingTouchdowns),
                PassingTwoPointConversions = p.AsParallel().Sum(f => f.PassingTwoPointConversions),
                Pats = p.AsParallel().Sum(f => f.Pats),
                PassingTouchdowns = p.AsParallel().Sum(f => f.PassingTouchdowns),
                Receptions = p.AsParallel().Sum(f => f.Receptions),
                RushingTwoPointConversions = p.AsParallel().Sum(f => f.RushingTwoPointConversions)
            };
        }

        private IEnumerable<FantasyPlayerRawView> GetFantasyPlayerRawViews(int year, Player player)
        {
            return _context.Plays
                .Include(p => p.Game)
                .Where(p => p.Game.SeasonType == "Regular" &&
                            p.Game.SeasonYear == year &&
                            player.Id == p.PlayerId)
                .Select(SelectPlayerRawView())
                .ToList();
        }

        private static Expression<Func<Play, FantasyPlayerRawView>> SelectPlayerRawView()
        {
            return p => new FantasyPlayerRawView
            {
                Id = p.PlayerId,
                FieldGoalsTwenty = p.FieldGoalYards > 0 && p.FieldGoalYards <= 19 ? 1 : 0,
                FieldGoalsThirty = p.FieldGoalYards >= 20 && p.FieldGoalYards <= 29 ? 1 : 0,
                FieldGoalsForty = p.FieldGoalYards >= 30 && p.FieldGoalYards <= 39 ? 1 : 0,
                FieldGoalsFifty = p.FieldGoalYards >= 40 && p.FieldGoalYards <= 49 ? 1 : 0,
                FieldGoalsMax = p.FieldGoalYards >= 50 ? 1 : 0,
                Fumbles = p.Fumbles,
                FumblesLost = p.FumblesLost,
                GameId = p.GameId,
                PassingInterceptions = p.PassingInterception,
                PassingTouchdowns = p.PassingTouchdowns,
                PassingTwoPointConversions = p.PassingTwoPointConversion,
                PassingYards = p.PassingYards,
                Pats = p.Pat,
                ReceivingTouchdowns = p.ReceivingTouchdowns,
                ReceivingTwoPointConversions = p.ReceivingTwoPointConversion,
                ReceivingYards = p.ReceivingYards,
                Receptions = p.Receptions,
                RushingTouchdowns = p.RushingTouchdowns,
                RushingTwoPointConversions = p.RushingTwoPointConversion,
                RushingYards = p.RushingYards
            };
        }
    }
}