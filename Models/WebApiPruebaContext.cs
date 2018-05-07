using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using WebAPIPrueba.Models;

namespace WebAPIPrueba.Models
{
    public class WebApiPruebaContext : DbContext
    {
        public WebApiPruebaContext() : base("DefaultConnection")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }


        public DbSet<Department> Departments { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Boss> Bosses { get; set; }

        public DbSet<Link> Links { get; set; }

        public DbSet<Coordinator> Coordinators { get; set; }

        public DbSet<Leader> Leaders { get; set; }

        public DbSet<VotingPlace> VotingPlaces { get; set; }

        public DbSet<Commune> Communes { get; set; }

        public DbSet<Association> Associations { get; set; }

        public DbSet<Voter> Voters { get; set; }

        public DbSet<UserId> UserIds { get; set; }

        public DbSet<Refer> Refers { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<HojaVida> HojaVidas { get; set; }

        public DbSet<Filters> Filters { get; set; }

        public DbSet<Dates> Dates { get; set; }

        public DbSet<DatesFiles> DatesFiles { get; set; }

        public DbSet<TimesDates> TimesDates { get; set; }

        public DbSet<DateHours> DateHours { get; set; }

        public DbSet<WorkPlace> WorkPlaces { get; set; }

        public System.Data.Entity.DbSet<WebAPIPrueba.Models.Sms> Sms { get; set; }
    }
}