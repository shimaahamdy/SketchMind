using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace SketchMind.Infrastructure.Identity
{
    // extend identityUser class for later use
    public class ApplicationUser: IdentityUser<int>
    {
    }
}
