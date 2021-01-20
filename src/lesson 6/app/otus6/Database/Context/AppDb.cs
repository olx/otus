using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using otus6.Models;

namespace otus6.Database
{
    public class AppDb: DbContext
    {
        public DbSet<User> Users { get; set; }

        public AppDb(DbContextOptions<AppDb> options)
            : base(options)
        {
            //Database.EnsureCreated();
        }
    }
}
