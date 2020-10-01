using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA_3___Dapper.Model
{

    [Table("Team")]

    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int SportId { get; set; }
        public virtual Sport Sport { get; set; }
        public virtual ICollection<Player> Players { get; set; } = new HashSet<Player>();
    }

}
