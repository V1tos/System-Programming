using CA_3___Dapper.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA_3___Dapper.DataAccess
{
    public class TestADO : ITest
    {
        public long GetPlayerInMS(int id)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            using (SqlConnection connection = new SqlConnection(Constants.ConnectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand($"Select * From Player where Id = {id}", connection);
                    Read(command);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }

        private static void Read(SqlCommand command)
        {
            List<object> objs = new List<object>();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        objs.Add(reader);
                    }
                }
            }
        }

        public long GetPlayersTeamInMS(int teamId)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            using (SqlConnection connection = new SqlConnection(Constants.ConnectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand($"Select * From Player where TeamId = {teamId}", connection);
                    Read(command);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }

        public long GetTeamsInSportInMS(int sportId)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            using (SqlConnection connection = new SqlConnection(Constants.ConnectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand($"Select * From Team where SportId = {sportId}", connection);
                    Read(command);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }
    }

}
