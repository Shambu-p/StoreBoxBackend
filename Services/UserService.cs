using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoreBackend.Data;
using StoreBackend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace StoreBackend.Services {

    public class UserService {
        
        private readonly StoreBackendContext context;

        public UserService(StoreBackendContext db_context) {
            context = db_context;
        }

        public async Task<ActionResult<IEnumerable<User>>> getAllUsers() {
            return await context.Users.ToListAsync();
        }

        public async Task<User> getUserById(uint id) {
            return await context.Users.FindAsync(id);
        }

        public async Task<User> getUserByEmail(string email) {
            
            User user = context.Users.Where(u => u.Email == email).FirstOrDefault<User>();
            return user;

        }

        public async Task<User> addUser(string name, string email, string password, byte role) {

            User new_user = new User();
            new_user.Name = name;
            new_user.Email = email;
            new_user.Role = role;
            new_user.Password = BCrypt.Net.BCrypt.HashPassword(password);
            //BCrypt.Net.BCrypt.Verify("Pa$$w0rd", passwordHash);

            context.Users.Add(new_user);
            var result = await context.SaveChangesAsync();

            return new_user;

        }

        public async Task<User> changeUser(User new_user) {
            
            User user = context.Users.Where(u => u.Id == new_user.Id).FirstOrDefault<User>();
            
            if(user != null) {
            
                if(new_user.Name != null){
                    user.Name = new_user.Name;
                }

                if(new_user.Password != null){
                    user.Password = BCrypt.Net.BCrypt.HashPassword(new_user.Password);
                }

                if(new_user.Email != null){
                    user.Email = new_user.Email;
                }

                if(new_user.Role != null){
                    user.Role = new_user.Role;
                }

                context.Entry(user).State = EntityState.Modified;

                await context.SaveChangesAsync();

            }

            return user;

        }

    }

}