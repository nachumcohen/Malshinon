using Malshinons.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinons.DAL
{
    
    internal static class DalIntelReports
    {
        private static string connStr = DbConfig.ConnectionString;

        public static int AvgText(int id)
        {
            int avgText = 0;
            string query = "SELECT AVG(LENGTH(Text)) AS AvgLength FROM intelreports WHERE Reporter_Id = @id";

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    try
                    {
                        conn.Open();
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read() && !reader.IsDBNull(0))
                            {
                                avgText = Convert.ToInt32(reader.GetDouble(0));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error in AvgText: {ex.Message}");
                    }
                }
            }

            return avgText;
        }

        public static int CountReporterId(int id)
        {
            int count = 0;
            string query = "SELECT COUNT(*) FROM intelreports WHERE Reporter_Id = @id";


            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    conn.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())

                        try
                        {
                            if (reader.Read())
                            {
                                count = reader.GetInt32(0);
                            }


                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"error func CountTargetId{ex.Message}");
                        }
                }
            }

            return count;

        }

        public static int CountTargetId(int id)
        {
            int count = 0;
            string query = "SELECT COUNT(*) FROM intelreports WHERE Target_Id = @id";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    conn.Open();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            count = reader.GetInt32(0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CountTargetId: {ex.Message}");
            }

            return count;
        }

        public static void AddIntelRepots(IntelReports intelReports)
        {
                string query = @"INSERT INTO intelreports (Reporter_Id , Target_Id ,Text)
                                    VALUES (@ReporterId  , @TargetId , @Text)";

                using (MySqlConnection conn = new MySqlConnection(connStr))
                {

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        try
                        {
                            conn.Open();
                            cmd.Parameters.AddWithValue("@ReporterId", intelReports.ReporterId);
                            cmd.Parameters.AddWithValue("@TargetId", intelReports.TargetId);
                            cmd.Parameters.AddWithValue("@Text", intelReports.Text);
                            cmd.ExecuteNonQuery();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"error AddIntelRepots: {ex.Message}");
                        }
                    }
                }

        }

        public static Alert GetPotentialAlert(int Id, int minute)
        {
            Alert alert = null;
            string query = @"SELECT TARGET_ID, COUNT(TARGET_ID) Total , MAX(Timestamp) Max , MIN(Timestamp) Min
                      FROM intelreports WHERE (Timestamp BETWEEN NOW() - INTERVAL @minute MINUTE and NOW()) AND (TARGET_ID = @id ) 
                      GROUP BY Target_Id HAVING Total > 2;";

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", Id);
                    cmd.Parameters.AddWithValue("@minute", minute);
                    conn.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            alert = new Alert();
                            alert.TargetId = reader.GetInt32("TARGET_ID");
                            alert.Time1 = reader.GetDateTime("Min");
                            alert.Time2 = reader.GetDateTime("Max");
                        }
                    }

                }
            }

            return alert;
        }



    }
}
