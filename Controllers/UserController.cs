using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StoreBackend.Data;
using StoreBackend.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace StoreBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        
        private readonly StoreBackendContext stContext;

        public UserController(StoreBackendContext context) {
            stContext = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> all() {
            return await stContext.Users.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> single(uint id) {

            var user = await stContext.Users.FindAsync(id);

            if(user == null) {
                return NotFound();
            }
            
            return user;

        }

        [HttpPost]
        public async Task<ActionResult<User>> add( string name, string email, string password, byte role) {

            User new_user = new User();
            new_user.Name = name;
            new_user.Email = email;
            new_user.Role = role;
            new_user.Password = BCrypt.Net.BCrypt.HashPassword(password);
            //BCrypt.Net.BCrypt.Verify("Pa$$w0rd", passwordHash);

            stContext.Users.Add(new_user);
            var result = await stContext.SaveChangesAsync();

            // return CreatedAtAction("Add User", new {Id = new_user.Id }, new_user);
            return new_user;


        }

        [HttpPut]
        public async Task<ActionResult<User>> change(uint id, string name, string email, string password, byte role) {

            User user = stContext.Users.Where(u => u.Id == id).FirstOrDefault<User>();

            User new_user = new User();
            new_user.Id = id;
            new_user.Name = name;
            new_user.Email = email;
            new_user.Role = role;
            new_user.Password = BCrypt.Net.BCrypt.HashPassword(password);

            stContext.Entry(new_user).State = EntityState.Modified;

            await stContext.SaveChangesAsync();

            return new_user;

        }

    }
}