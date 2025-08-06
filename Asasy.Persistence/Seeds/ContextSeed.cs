using AAITHelper.Enums;
using Asasy.Domain.Entities.UserTables;
using Asasy.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;

namespace Asasy.Persistence.Seeds
{
    public static class ContextSeed
    {


        public static async Task Seed(UserManager<ApplicationDbUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext applicationDbContext)
        {
            //لو هوزد حاجه فى السيد تبقا بالشكل ده
            await CreateBasicSetting(applicationDbContext);
            await CreateRoles(roleManager);
            await CreateBasicUsers(userManager);
        }

        private static async Task CreateRoles(RoleManager<IdentityRole> roleManager)
        {
            foreach (IdentityRole role in DefaultRoles.IdentityRoleList())
            {
                await roleManager.CreateAsync(role);
            }
        }

        private static async Task CreateBasicUsers(UserManager<ApplicationDbUser> userManager)
        {

            foreach (ApplicationDbUser user in DefaultUser.IdentityBasicUserList())
            {
                var userFound = await userManager.FindByEmailAsync(user.Email);
                if (userFound == null)
                {
                    await userManager.CreateAsync(user, "123456");
                    if (user.TypeUser == UserType.Admin.ToNumber())
                        await userManager.AddToRoleAsync(user, Roles.Admin.ToString());
                    else
                        await userManager.AddToRoleAsync(user, Roles.Mobile.ToString());
                }
            }


        }

        private static async Task CreateBasicSetting(ApplicationDbContext applicationDbContext)
        {
            if (!await applicationDbContext.Settings.AnyAsync())
            {
                try
                {
                    using var connection = applicationDbContext.Database.GetDbConnection();
                    await connection.OpenAsync();
                    
                    using var command = connection.CreateCommand();
                    command.CommandText = "SET IDENTITY_INSERT Settings ON;";
                    await command.ExecuteNonQueryAsync();
                    
                    await applicationDbContext.Settings.AddAsync(DefaultSettings.BasicSettingAsync());
                    await applicationDbContext.SaveChangesAsync();
                    
                    command.CommandText = "SET IDENTITY_INSERT Settings OFF;";
                    await command.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {
                    // Log error but don't crash
                    throw;
                }
            }
        }

    }
}
