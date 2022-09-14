using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StoreBackend.Models;
using StoreBackend.Data;
using StoreBackend.Services;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;


namespace StoreBackend.Controllers {

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase {

        public readonly StoreBackendContext context;
        public readonly AuthSettings jwtSettings;

        public static readonly string claim_id = "Id";

        public AuthController (StoreBackendContext store_context, IOptions<AuthSettings> option) {
            this.context = store_context;
            this.jwtSettings = option.Value;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserAuthentication>> Authenticate(string email, string password) {

            UserService service = new UserService(context);
            User found_user = await service.getUserByEmail(email);
            
            if(found_user == null) {
                return NotFound("user not found");
            }

            if(!BCrypt.Net.BCrypt.Verify(password, found_user.Password)) {
                return NotFound("incorrect password!");
            }

            var token_descriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(new Claim[]{
                        // new Claim(ClaimTypes.Name, found_user.Email)
                        new Claim(claim_id, found_user.Id.ToString()),
                        new Claim("Name", found_user.Name),
                        new Claim("Email", found_user.Email),
                        new Claim("Role", found_user.Role.ToString())
                    }),
                Expires = DateTime.Now.AddSeconds(3600),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.jwtSettings.securityKey)),
                    SecurityAlgorithms.HmacSha256
                )
            };

            var token_handler = new JwtSecurityTokenHandler();
            var token = token_handler.WriteToken(token_handler.CreateToken(token_descriptor));

            UserAuthentication user_return = new UserAuthentication();
            user_return.Id = found_user.Id;
            user_return.Name = found_user.Name;
            user_return.Email = found_user.Email;
            user_return.Role = found_user.Role;
            user_return.Token = token;

            return user_return;

        }

        [HttpGet("authorization"), Authorize]
        public ActionResult<UserAuthentication> authorization(){

            UserAuthentication user_return = new UserAuthentication();
            user_return.Id = uint.Parse(User?.FindFirstValue("Id"));
            user_return.Name = User?.FindFirstValue("Name");
            user_return.Email = User?.FindFirstValue("Email");

            // string role = User?.FindFirstValue("Role");
            user_return.Role = Convert.ToByte(User?.FindFirstValue("Role"));
            // user_return.Token = token;

            return user_return;

        }
    }

}