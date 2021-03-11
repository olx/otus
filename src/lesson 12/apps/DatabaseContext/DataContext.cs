using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DatabaseContext
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) {

        }
    }
}
