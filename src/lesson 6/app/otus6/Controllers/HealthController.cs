using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace otus.Controllers
{
    public class HealthController : Controller
    {
        [Route("/health")]
        public object Health()
        {
            return new { status = "OK" };        
        }

    }
}
