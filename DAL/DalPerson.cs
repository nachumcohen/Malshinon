using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Malshinons.Modles;
using MySql.Data.MySqlClient;

namespace Malshinons.DAL
{
    internal static class DalPerson2
    {
        private static string connStr = DbConfig.ConnectionString;
        public static bool IsPerson(string FirstName, string LastName)
        {

            MySqlDataReader reader = null;
            string query = "SELECT First_Name , Last_Name FROM people WHERE First_Name = @FirstName AND Last_Name = @LastName";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@FirstName", FirstName);
                        cmd.Parameters.AddWithValue("@LastName", LastName);
                        using (reader = cmd.ExecuteReader())
                        {
                            return reader.HasRows;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ex.Message{ex.Message}");
            }
            return false;

        }

        public static void AddPerson(Person person)
        {
            string query = @"INSERT INTO people (First_Name ,Last_Name , Secret_Code , Type)
                                   VALUES (@FirstName , @LastName , @SecretCode , @Type);";

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        cmd.Parameters.AddWithValue("@Type", person.Type);
                        cmd.Parameters.AddWithValue("@FirstName", person.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", person.LastName);
                        cmd.Parameters.AddWithValue("@SecretCode", person.SecretCode);
                        cmd.ExecuteNonQuery();

                        Console.WriteLine("add person successfully");
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine($"error AddPerson: {ex.Message}");
                    }
                }
            }
        }

        public static int returnID(string FirstName)
        {
            int Id = 0;

            MySqlDataReader reader = null;
            string query = "SELECT ID FROM people WHERE First_Name = @FirstName";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        conn.Open();
                        cmd.Parameters.AddWithValue("@FirstName", FirstName);
                        using (reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Id = reader.GetInt32("Id");
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"returnID{ex.Message}");
            }
            return Id;

        }

        public static void TypeChange(int id, string type)
        {
            if (type.Length == 0)
            {
                Console.WriteLine("TypeChange: not type");
                return;
            }
            string query = @"UPDATE people SET Type = @Type WHERE ID = @id";

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@Type", type);
                        cmd.ExecuteNonQuery();

                        Console.WriteLine("change Type good");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"error to TypeChange: {ex.Message}");
                    }
                }
            }

        }

        public static void ToIncreaseNumReports(int Id)
        {
            string query = "UPDATE people SET Num_Reports = Num_Reports +1 WHERE ID = @Id ";

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {

                    try
                    {
                        conn.Open();
                        cmd.Parameters.AddWithValue("@Id", Id);
                        cmd.ExecuteNonQuery();

                        Console.WriteLine("add NumReports");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"error ToIncreaseNumReports: {ex.Message}");
                    }
                }

            }

        }

        public static void ToIncreaseNumMentions(int Id)
        {
            string query = "UPDATE people SET Num_Mentions = Num_Mentions +1 WHERE ID = @Id";

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {

                    try
                    {
                        conn.Open();
                        cmd.Parameters.AddWithValue("@Id", Id);
                        cmd.ExecuteNonQuery();

                        Console.WriteLine("add NumMentions");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"error ToIncreaseNumMentions: {ex.Message}");
                    }
                }

            }
        }

    }
}
