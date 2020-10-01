using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA_3___Dapper.Model
{
    [Table("Sport")]
    public class Sport
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Team> Teams { get; set; } = new HashSet<Team>();
    }

}
