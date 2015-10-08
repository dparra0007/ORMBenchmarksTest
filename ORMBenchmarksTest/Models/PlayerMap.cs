using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMBenchmarksTest.Models
{
    public class PlayerMap : EntityTypeConfiguration<Player>
    {
        public PlayerMap()
        {
            //this.ToTable("Player");
            //this.Property(t => t.Id).HasColumnName("Id");
            //this.Property(t => t.FirstName).HasColumnName("FirstName");
            //this.Property(t => t.LastName).HasColumnName("LastName");
            //this.Property(t => t.DateOfBirth).HasColumnName("DateOfBirth");
            //this.Property(t => t.TeamId).HasColumnName("TeamId");
        }
    }
}
