namespace ORMBenchmarksTest.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class SportContext : DbContext
    {
        public SportContext()
            : base(Constants.ConnectionString)
        {
            Database.SetInitializer<SportContext>(new DropCreateDatabaseAlways<SportContext>());
            Database.Initialize(true);
        }

        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<Sport> Sports { get; set; }
        public virtual DbSet<Team> Teams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sport>()
                .HasMany(e => e.Teams)
                .WithRequired(e => e.Sport)
                .WillCascadeOnDelete(false);


            modelBuilder.Configurations.Add(new PlayerMap());

            modelBuilder.Configurations.Add(new TeamMap());
        }
    }
}
