using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MII_Media.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace MII_Media.Data
{
    public class MiiContext:IdentityDbContext<ApplicationUser>
    {
       

        public MiiContext(DbContextOptions<MiiContext> options)
           : base(options)
        {
        }

        
    }
}
