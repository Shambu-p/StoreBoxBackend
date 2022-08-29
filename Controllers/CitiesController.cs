using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using StoreBackend.Data;
using StoreBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace StoreBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitiesController : ControllerBase
    {
        private readonly asp_dbContext _context;

        public CitiesController(asp_dbContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<City>>> all() {

            return await _context.Cities.ToListAsync();

        }
    }
}