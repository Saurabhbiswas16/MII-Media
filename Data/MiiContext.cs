using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MII_Media.Models;
using System;
using System.Collections.Generic;
using System.Data;
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

        public DbSet<Friend> Friends { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<MII_Media.ViewModels.PostEditViewModel> PostEditViewModel { get; set; }

    }
}
