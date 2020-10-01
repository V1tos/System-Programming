using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA_3___Dapper.Helpers
{
    public class TestResult
    {
        public double PlayerMs { get; set; }
        public double TeamMs { get; set; }
        public double SportMs { get; set; }
        public Framework Framework { get; set; }
    }
    public enum Framework
    {
        EntityFramework,
        AdoNet,
        Dapper
    };

}
