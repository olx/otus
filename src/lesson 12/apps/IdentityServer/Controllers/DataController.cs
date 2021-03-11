using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks; 
using Microsoft.AspNetCore.Identity;
using DatabaseContext;

namespace IdentityServer.Controllers
{
    public class DataController : Controller
    {
        private readonly DataContext _context;
        private readonly UserManager<AppUser> _manager;
        public DataController(DataContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _manager = userManager;
        }
        public async Task SeedData()
        {
            await DataSeed.SeedDataAsync(_context, _manager);
        }
    }
}
