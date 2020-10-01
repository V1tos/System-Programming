using CA_3___Dapper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA_3___Dapper.Helpers
{
    public static class Database
    {
        public static void Reset()
        {
            using (SportContext sc = new SportContext())
            {
                sc.Database.ExecuteSqlCommand("DELETE FROM Sport");
                sc.Database.ExecuteSqlCommand("DELETE FROM Team");
                sc.Database.ExecuteSqlCommand("DELETE FROM Player");
            }
        }
        public static void Load(List<Player> p, List<Team> t, List<Sport> s)
        {
            AddSports(s);
            AddTeams(t);
            AddPlayers(p);
        }

        public static void AddPlayers(List<Player> players)
        {
            using (SportContext sc = new SportContext())
            {
                sc.Players.AddRange(players);
                sc.SaveChanges();
            }
        }
        public static void AddTeams(List<Team> teams)
        {
            using (SportContext sc = new SportContext())
            {
                sc.Teams.AddRange(teams);
                sc.SaveChanges();
            }
        }
        public static void AddSports(List<Sport> teams)
        {
            using (SportContext sc = new SportContext())
            {
                sc.Sports.AddRange(teams);
                sc.SaveChanges();
            }
        }
    }

}
