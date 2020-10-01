using CA_3___Dapper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA_3___Dapper.Helpers
{
    public static class Generator
    {
        public static List<Player> GeneratePlayers(int teamId, int count)
        {
            List<Player> players = new List<Player>();

            Random rnd = new Random();
            var names = Data.GetNames();
            var lastNames = Data.GetLastNames();

            for (int i = 0; i < count; i++)
            {
                Player temp = new Player
                {
                    Name = names[rnd.Next(0, names.Count)],
                    LastName = lastNames[rnd.Next(0, lastNames.Count)],
                    TeamId = teamId,
                    Id = ((teamId - 1) * count) + i + 1
                };

                players.Add(temp);
            }
            return players;
        }

        public static List<Team> GenerateTeams(int sportId, int count)
        {
            List<Team> teams = new List<Team>();

            Random rnd = new Random();
            var names = Data.GetTeamNames();


            for (int i = 0; i < count; i++)
            {
                Team temp = new Team
                {
                    Name = names[rnd.Next(0, names.Count)],
                    SportId = sportId,
                    Id = ((sportId - 1) * count) + i + 1
                };

                teams.Add(temp);
            }
            return teams;
        }
        static int j = 1;
        public static List<Sport> GenerateSport(int count)
        {
            List<Sport> sports = new List<Sport>();

            Random rnd = new Random();
            var names = Data.GetSportNames();


            for (int i = 0; i < count; i++)
            {
                Sport temp = new Sport
                {
                    Name = names[rnd.Next(0, names.Count)],
                    Id = i + 1
                };

                sports.Add(temp);
            }
            return sports;
        }
    }

}
