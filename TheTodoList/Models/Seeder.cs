using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheTodoList.Entities;

namespace TheTodoList.Models
{
    public class Seeder
    {
        private readonly TodoContext ctx;
        private readonly UserManager<StoreUser> userManager;

        public Seeder(TodoContext ctx, UserManager<StoreUser> userManager)
        {
            this.ctx = ctx;
            this.userManager = userManager;
        }

        public async Task Seed()
        {
            ctx.Database.EnsureCreated();
            await CreateUser("giuseppemumolo@gmail.com", "giuseppe", "mumolo", "mgiuseppe2");
            await CreateUser("gianfrancesco@gmail.com", "gian", "francesco", "gianfrancesco");
        }

        private async Task CreateUser(string email, string firstName, string LastName, string userName)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new StoreUser
                {
                    FirstName = firstName,
                    LastName = LastName,
                    Email = email,
                    UserName = userName
                };

                var result = await userManager.CreateAsync(user, "P4ssw0rd!");

                if (result != IdentityResult.Success)
                    throw new InvalidOperationException("Failed to create default user");
            }
        }
    }
}
