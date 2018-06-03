using System.Linq;
using FantasyDraftAid.DataAccess;
using FantasyDraftAid.DataAccess.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FantasyDraftAid.Controllers
{
    [Route("/players")]
    public class PlayerContoller : Controller
    {
        private readonly NflContext _context;

        public PlayerContoller(NflContext context)
        {
            _context = context;
        }

        // GET
        public IActionResult Index()
        {
            var quarterbacks = _context.Players.Include(p => p.Team).Where(p => p.Position == PlayerPosition.QB)
                .ToList();
            var enummed = quarterbacks.Select(qb => new
            {
                qb.FirstName,
                qb.LastName,
                qb.FullName,
                playerStatus = qb.PlayerStatus.GetDescription(),
                posititon = qb.Position.GetDescription(),
                team = qb.Team.Name
            }).ToList();
            return Ok(enummed);
        }
    }
}