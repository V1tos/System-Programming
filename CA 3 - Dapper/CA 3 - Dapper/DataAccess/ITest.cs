using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA_3___Dapper.DataAccess
{
    public interface ITest
    {
        public long GetPlayerInMS(int id);

        public long GetPlayersTeamInMS(int teamId);

        public long GetTeamsInSportInMS(int sportId);
    }
}
