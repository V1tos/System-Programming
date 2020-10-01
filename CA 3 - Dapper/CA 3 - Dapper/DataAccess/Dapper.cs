using CA_3___Dapper.Model;
using System;
using CA_3___Dapper;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using CA_3___Dapper.Helpers;

namespace CA_3___Dapper.DataAccess
{
    class DapperTest : ITest
    {
        public long GetPlayerInMS(int id)
        {
            Stopwatch stopwatch = new Stopwatch();
            using (IDbConnection db = new SqlConnection(Constants.ConnectionString))
            {
                db.Open();
                stopwatch.Start();
                var tmp = db.Query<Player>("Select * from Player where Id = @Id", new { Id = id });
                stopwatch.Stop();
            }
            return stopwatch.ElapsedMilliseconds;
        }
        public long GetPlayersTeamInMS(int teamId)
        {
            Stopwatch stopwatch = new Stopwatch();
            using (IDbConnection db = new SqlConnection(Constants.ConnectionString))
            {
                db.Open();
                stopwatch.Start();
                var tmp = db.Query<List<Player>>("Select * from Player where TeamId = @Id", new { Id = teamId });
                stopwatch.Stop();
            }
            return stopwatch.ElapsedMilliseconds;
        }
        public long GetTeamsInSportInMS(int sportId)
        {
            Stopwatch stopwatch = new Stopwatch();
            using (IDbConnection db = new SqlConnection(Constants.ConnectionString))
            {
                db.Open();
                stopwatch.Start();
                var tmp = db.Query<List<Team>>("Select * from Team where SportId = @Id", new { Id = sportId });
                stopwatch.Stop();
            }
            return stopwatch.ElapsedMilliseconds;
        }
    }


}
