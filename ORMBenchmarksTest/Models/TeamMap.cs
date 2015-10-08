using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMBenchmarksTest.Models
{
    public class TeamMap : EntityTypeConfiguration<Team>
    {
        public TeamMap()
        {
            this
                .HasMany(e => e.Players)
                .WithRequired(e => e.Team)
                .WillCascadeOnDelete(false);

            //this.ToTable("Team");
            //this.Property(t => t.Id).HasColumnName("Id");
            //this.Property(t => t.Name).HasColumnName("Name");
            //this.Property(t => t.FoundingDate).HasColumnName("FoundingDate");
            //this.Property(t => t.SportId).HasColumnName("SportId");
        }
    }
}
