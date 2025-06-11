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
    internal static class DalAlerts
    {
        private static string connStr = DbConfig.ConnectionString;

        public static void AddAlert(Alert alert)
        {
            string query = @"INSERT INTO alerts (Target_Id ,StartTime , EndTime , Reason )
                                VALUES  (@Target_Id , @StartTime , @EndTime , @Reason);";

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        cmd.Parameters.AddWithValue("@Target_Id" , alert.TargetId);
                        cmd.Parameters.AddWithValue("@StartTime", alert.Time1);
                        cmd.Parameters.AddWithValue("@EndTime", alert.Time2);
                        cmd.Parameters.AddWithValue("@Reason", alert.Reason);

                        cmd.ExecuteNonQuery();
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine($"error addAlert : {ex.Message}");
                    }
                    
                }
            }
        }

        public static Alert GetAlert(int id)
        {
            Alert alert = null;

            string query = @"SELECT * FROM alerts WHERE Target_Id = @id";
            try
            {

                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        conn.Open();
                        cmd.Parameters.AddWithValue("@id", id);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                        
                            if (reader.Read())
                            {
                                alert = new Alert();
                                alert.TargetId = reader.GetInt32("Target_Id");
                                alert.Time1 = reader.GetDateTime("StartTime");
                                alert.Time2 = reader.GetDateTime("EndTime");
                                alert.Reason = reader.GetString("Reason");
                            }
                            else
                            {
                                Console.WriteLine("id nit find in alert");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error in GetAlert : {ex.Message}");
            }

            return alert;
        }
    }
}
