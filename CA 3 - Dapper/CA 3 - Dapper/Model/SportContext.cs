using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA_3___Dapper.Model
{
    public class SportContext : DbContext
    {
        public SportContext()
            : base("name=SportContext")
        {
            Database.SetInitializer<SportContext>(new DropCreateDatabaseAlways<SportContext>());
        }

        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<Sport> Sports { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
    }
}
