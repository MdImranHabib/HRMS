using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HRMS.Models;

namespace HRMS.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<HRMS.Models.Flat> Flats { get; set; }
        public DbSet<HRMS.Models.Resident> Residents { get; set; }            
        public DbSet<HRMS.Models.ResidentFlat> ResidentFlats { get; set; }
    }
}
