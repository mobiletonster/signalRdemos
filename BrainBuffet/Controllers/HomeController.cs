using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrainBuffet.Controllers
{
    public class HomeController:Controller
    {
        [HttpGet("/api/ping")]

        public IActionResult Ping()
        {
            return Ok("Hello " + DateTime.Now);
        }
    }
}
