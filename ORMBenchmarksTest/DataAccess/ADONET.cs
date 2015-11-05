using ORMBenchmarksTest.DTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMBenchmarksTest.DataAccess
{
    public class ADONET : ITestSignature
    {
        public long GetPlayerByID(int id)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            using(SqlConnection conn = new SqlConnection(Constants.ConnectionString))
            {
                conn.Open();
                using(SqlDataAdapter adapter = new SqlDataAdapter("SELECT Id, FirstName, LastName, DateOfBirth, TeamId FROM Player WHERE Id = @ID", conn))
                {
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@ID", id));
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                }
            }
            watch.Stop();
            return watch.ElapsedMilliseconds;
        }

        public long GetPlayersForTeam(int teamId)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            using (SqlConnection conn = new SqlConnection(Constants.ConnectionString))
            {
                conn.Open();
                using(SqlDataAdapter adapter = new SqlDataAdapter("SELECT Id, FirstName, LastName, DateOfBirth, TeamId FROM Player WHERE TeamId = @ID", conn))
                {
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@ID", teamId));
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                }
            }
            watch.Stop();
            return watch.ElapsedMilliseconds;
        }

        public long GetTeamsForSport(int sportId)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            using (SqlConnection conn = new SqlConnection(Constants.ConnectionString))
            {
                conn.Open();
                using(SqlDataAdapter adapter = new SqlDataAdapter("SELECT p.Id, p.FirstName, p.LastName, p.DateOfBirth, p.TeamId, t.Id as TeamId, t.Name, t.SportId FROM Player p "
                                                                  + "INNER JOIN Team t ON p.TeamId = t.Id WHERE t.SportId = @ID", conn))
                {
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@ID", sportId));
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                }
            }
            watch.Stop();
            return watch.ElapsedMilliseconds;
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

            using (SqlConnection conn = new SqlConnection(Constants.ConnectionString))
            {
                conn.Open();

                using (SqlDataAdapter deleteAdapter
                    = new SqlDataAdapter())
                {
                    deleteAdapter.DeleteCommand = new SqlCommand("DELETE FROM Player", conn);
                    deleteAdapter.DeleteCommand.ExecuteNonQuery();
                    deleteAdapter.DeleteCommand = new SqlCommand("DELETE FROM Team", conn);
                    deleteAdapter.DeleteCommand.ExecuteNonQuery();
                    deleteAdapter.DeleteCommand = new SqlCommand("DELETE FROM Sport", conn);
                    deleteAdapter.DeleteCommand.ExecuteNonQuery();
                }

                SqlDataAdapter playerAdapter
                    = new SqlDataAdapter("INSERT INTO Player(Id, FirstName, LastName, DateOBirth, TeamId) VALUES (@ID, @FIRSTNAME, @LASTNAME, @DATEOFBIRTH, @TEAMID)", conn);

                foreach (var sport in sports)
                {
                    using (var command = new SqlCommand("INSERT INTO Sport(Id, Name) VALUES (@ID, @NAME)", conn))
                    {
                        command.Parameters.AddWithValue("@ID", sport.Id);
                        command.Parameters.AddWithValue("@NAME", sport.Name);
                        command.ExecuteNonQuery();
                    }
                }

                foreach (var team in teams)
                {
                    using (var command = new SqlCommand("INSERT INTO Team(Id, Name, FoundingDate, SportId) VALUES (@ID, @NAME, @FOUNDINGDATE, @SPORTID)", conn))
                    {

                        command.Parameters.AddWithValue("@ID", team.Id);
                        command.Parameters.AddWithValue("@NAME", team.Name);
                        command.Parameters.AddWithValue("@FOUNDINGDATE", team.FoundingDate);
                        command.Parameters.AddWithValue("@SPORTID", team.SportId);
                        command.ExecuteNonQuery();
                    }
                }

                foreach (var player in players)
                {
                    using (var command = new SqlCommand("INSERT INTO Player(Id, FirstName, LastName, DateOBirth, TeamId) VALUES (@ID, @FIRSTNAME, @LASTNAME, @DATEOFBIRTH, @TEAMID)", conn))
                    {

                        command.Parameters.AddWithValue("@ID", player.Id);
                        command.Parameters.AddWithValue("@FIRSTNAME", player.FirstName);
                        command.Parameters.AddWithValue("@LASTNAME", player.LastName);
                        command.Parameters.AddWithValue("@DATEOFBIRTH", player.DateOfBirth);
                        command.Parameters.AddWithValue("@TEAMID", player.TeamId);
                        command.ExecuteNonQuery();
                    }
                }

                conn.Close();

            }
            watch.Stop();
            return watch.ElapsedMilliseconds;
        }
    }
}
