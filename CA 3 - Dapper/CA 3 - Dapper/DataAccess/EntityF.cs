using CA_3___Dapper.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA_3___Dapper.DataAccess
{
    class EntityF : ITest
    {
        SportContext SportContext = new SportContext();
        public long GetPlayerInMS(int id)
        {
            var watch = Stopwatch.StartNew();
            SportContext.Players.Find(id);
            watch.Stop();
            return watch.ElapsedMilliseconds;
        }
        public long GetPlayersTeamInMS(int teamId)
        {
            var watch = Stopwatch.StartNew();
            SportContext.Players.FirstOrDefault(x => x.TeamId == teamId);
            watch.Stop();
            return watch.ElapsedMilliseconds;
        }
        public long GetTeamsInSportInMS(int sportId)
        {
            var watch = Stopwatch.StartNew();
            SportContext.Teams.FirstOrDefault(x => x.SportId == sportId);
            watch.Stop();
            return watch.ElapsedMilliseconds;
        }
    }

}
