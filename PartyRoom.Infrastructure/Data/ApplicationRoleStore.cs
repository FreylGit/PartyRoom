using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using PartyRoom.Domain.Entities;

namespace PartyRoom.Infrastructure.Data
{
    public class ApplicationRoleStore : RoleStore<ApplicationRole, ApplicationDbContext, Guid>
    {
        public ApplicationRoleStore(ApplicationDbContext context, IdentityErrorDescriber describer = null)
            : base(context, describer)
        {
        }
    }
}
