using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using KickScooterSharing.Models;

namespace KickScooterSharing.Data
{
    public static class IdentityExtensions
    {
        public static Status Status(this IIdentity identity)
        {
            return null;
        }
    }
}
