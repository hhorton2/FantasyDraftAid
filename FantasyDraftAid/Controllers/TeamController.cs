using System.Linq;
using FantasyDraftAid.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace FantasyDraftAid.Controllers
{
    [Route("/teams")]
    public class TeamController : Controller
    {
        private readonly NflContext _context;

        public TeamController(NflContext context)
        {
            _context = context;
        }

        // GET
        public IActionResult Index()
        {
            return Ok(_context.Teams.ToList());
        }
    }
}