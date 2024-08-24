using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HospitalManagementSystem.Models;

namespace HospitalManagementSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<HospitalManagementSystem.Models.Baranggay> Baranggay { get; set; } = default!;
        public DbSet<HospitalManagementSystem.Models.Service> Service { get; set; } = default!;
        public DbSet<HospitalManagementSystem.Models.Patient> Patient { get; set; } = default!;
        public DbSet<HospitalManagementSystem.Models.Report> Report { get; set; } = default!;
        public DbSet<HospitalManagementSystem.Models.Morbidity> Morbidity { get; set; } = default!;
    }
}
