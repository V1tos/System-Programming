using CA_3___Dapper.DataAccess;
using CA_3___Dapper.Helpers;
using CA_3___Dapper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CA_3___Dapper
{
    class Program
    {
        static int sportCount = 0;
        static int teamCount = 0;
        static int playerCount = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("Enter sport count: ");
            sportCount = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter team count: ");
            teamCount = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter player count: ");
            playerCount = int.Parse(Console.ReadLine());

            List<Sport> sports = Generator.GenerateSport(sportCount);
            List<Team> teams = new List<Team>();
            List<Player> players = new List<Player>();

            foreach (var sport in sports)
            {
                var tmpTeams = Generator.GenerateTeams(sport.Id, teamCount);
                teams.AddRange(tmpTeams);
                foreach (var team in tmpTeams)
                {
                    var tmpPlayers = Generator.GeneratePlayers(team.Id, playerCount);
                    players.AddRange(tmpPlayers);
                    foreach (var player in tmpPlayers)
                    {
                        team.Players.Add(player);
                    }
                    sport.Teams.Add(team);
                }
            }
            Database.Load(players, teams, sports);

            List<TestResult> results = new List<TestResult> { StartProcces(Framework.Dapper, new DapperTest()),
                                                              StartProcces(Framework.AdoNet, new TestADO()),
                                                              StartProcces(Framework.EntityFramework, new EntityF())};

            ShowResult(results);
        }

        private static TestResult StartProcces(Framework framework, ITest test)
        {
            TestResult testResult = new TestResult { Framework = framework };

            for (int i = 0; i < playerCount ; i++)
            {
                testResult.PlayerMs += test.GetPlayerInMS(i);
            }
            testResult.PlayerMs /= playerCount;

            for (int i = 0; i < teamCount; i++)
            {
                testResult.TeamMs += test.GetPlayersTeamInMS(i);
            }
            testResult.TeamMs /= teamCount;

            for (int i = 0; i < sportCount; i++)
            {
                testResult.SportMs += test.GetTeamsInSportInMS(i);
            }
            testResult.SportMs /= sportCount;

            return testResult;
        }

        public static void ShowResult(ICollection<TestResult> results)
        {
            foreach(var item in results)
            {
                Console.WriteLine("_____________{0}______________\n",
                    Enum.GetName(typeof(Framework),
                    item.Framework));
                Console.WriteLine("{0,-15}, {1,15}{2,15}",
                                  "PlayerById", "PlayersInTeam", "TeamsInSport");
                Console.WriteLine("{0,-15}, {1,15}{2,15}",
                                    item.PlayerMs, item.SportMs, item.TeamMs);
            }
        }
    }
}
