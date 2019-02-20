using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Teach_MGT_Team.TeamAPI.MVC;

namespace Teach_MGT_Team.Models
{
    public class MVCDbContext : DbContext
    {
        private static bool _created = false;

        public MVCDbContext (DbContextOptions<MVCDbContext> options)
            : base(options)
        {
            /*
            Database.EnsureDeleted();
            _created = false; // */

            if (!_created)
            {
                _created = true;
                Database.EnsureCreated();
            }

        }

        // Error: No database provider has been configured for this DbContext. 
        // A provider can be configured by overriding the DbContext.OnConfiguring method 
        // or by using AddDbContext on the application service provider. If AddDbContext is used, 
        // then also ensure that your DbContext type accepts a DbContextOptions<TContext> object in 
        // its constructor and passes it to the base constructor for DbContext.      

        public MVCDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();

                optionsBuilder.UseSqlite("Data Source = MVCDb.db");

                // var connectionString = configuration.GetConnectionString("DbCoreConnectionString");
                // optionsBuilder.UseSqlServer(connectionString);
            }
        }
        

        public DbSet<Teach_MGT_Team.TeamAPI.MVC.Player> Player { get; set; }

        public DbSet<Teach_MGT_Team.TeamAPI.MVC.Team> Team { get; set; }
    }
}
