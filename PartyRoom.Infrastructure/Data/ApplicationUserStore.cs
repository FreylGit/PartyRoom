using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using PartyRoom.Domain.Entities;

namespace PartyRoom.Infrastructure.Data
{
    public class ApplicationUserStore : UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>
    {
        public ApplicationUserStore(ApplicationDbContext context, IdentityErrorDescriber describer = null)
            : base(context, describer)
        {
        }
    }
}
