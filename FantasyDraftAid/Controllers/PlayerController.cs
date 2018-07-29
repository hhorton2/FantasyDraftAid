using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FantasyDraftAid.DataAccess;
using FantasyDraftAid.DataAccess.Enums;
using FantasyDraftAid.Services;
using FantasyDraftAid.Views;
using LazyCache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FantasyDraftAid.Controllers
{
    [Route("/players")]
    public class PlayerController : Controller
    {
        private readonly NflContext _context;
        private readonly PlayerPointsService _playerPointsService;
        private readonly IAppCache _cache;

        public PlayerController(NflContext context, PlayerPointsService playerPointsService, IAppCache cache)
        {
            _context = context;
            _playerPointsService = playerPointsService;
            _cache = cache;
        }

        // GET
        public async Task<IActionResult> Index([FromQuery] PlayerPosition position = PlayerPosition.QB,
            [FromQuery] int year = 2017)
        {
            var returnPlayers =
                await _cache.GetOrAddAsync($"{position}-{year}", async () => await GetPlayers(position, year),
                    DateTimeOffset.Now.AddHours(1));
            return Ok(returnPlayers);
        }

        private async Task<IEnumerable<FantasyPlayerView>> GetPlayers(PlayerPosition position, int year)
        {
            var players = await _context.Players.Include(p => p.Team)
                .Where(p => p.Position == position)
                .ToListAsync();
            var returnPlayers = _playerPointsService.GetFantasyPlayers(players, year);
            return returnPlayers;
        }
    }
}