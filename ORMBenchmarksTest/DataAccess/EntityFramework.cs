using ORMBenchmarksTest.DTOs;
using ORMBenchmarksTest.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ORMBenchmarksTest.DataAccess
{
    public class EntityFramework : ITestSignature
    {
        public long GetPlayerByID(int id)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            using (SportContext context = new SportContext())
            {
                var player = context.Players.Where(x => x.Id == id).First();
            }
            watch.Stop();
            return watch.ElapsedMilliseconds;
        }

        public long GetPlayersForTeam(int teamId)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            using (SportContext context = new SportContext())
            {
                var players = context.Players.Where(x => x.TeamId == teamId).ToList();
            }
            watch.Stop();
            return watch.ElapsedMilliseconds;
        }

        public long GetTeamsForSport(int sportId)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            using (SportContext context = new SportContext())
            {
                //var tmpFromSportPlayers = context.Sports
                //var tmpPlayers = context.Teams.Where(t => t.SportId == sportId).SelectMany(t => t.Players).ToList();
                var players = context.Teams.Include(x=>x.Players).Where(x => x.SportId == sportId).ToList();
            }
            watch.Stop();
            return watch.ElapsedMilliseconds;
        }

        public void CreateDatabase()
        {
            try { SportContext context = new SportContext(); }
            catch (Exception ex) { }
        }

        public long Generate(int NumSports, int NumTeams, int NumPlayers)
        {

            List<SportDTO> sports = TestData.Generator.GenerateSports(NumSports);
            List<TeamDTO> teams = new List<TeamDTO>();
            List<PlayerDTO> players = new List<PlayerDTO>();

            foreach (var sport in sports)
            {
                var newTeams = TestData.Generator.GenerateTeams(sport.Id, NumTeams);
                teams.AddRange(newTeams);
                foreach (var team in newTeams)
                {
                    var newPlayers = TestData.Generator.GeneratePlayers(team.Id, NumPlayers);
                    players.AddRange(newPlayers);
                }
            }

            Stopwatch watch = new Stopwatch();
            watch.Start();

            ORMBenchmarksTest.TestData.Database.Reset();
            ORMBenchmarksTest.TestData.Database.Load(sports, teams, players);

            watch.Stop();
            return watch.ElapsedMilliseconds;
        }
    }
}
