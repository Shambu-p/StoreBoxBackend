using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StoreBackend.Data;
using StoreBackend.Models;

namespace StoreBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoreController : ControllerBase {
        
        private readonly StoreBackendContext context;

        public StoreController(StoreBackendContext db_context){
            this.context = db_context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Store>>> getAll(){

            return Ok();

        }

    }
}