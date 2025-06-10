using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Malshinons.Modles;
using MySql.Data.MySqlClient;



namespace Malshinons.DAL
{
    internal class DalPerson
    {
        private string connStr = "server=localhost;user=root;password= ;database=malshinon;";
        
        public bool IsPerson(string FirstName , string LastName)
        {

            //MySqlCommand cmd = null;
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

        public int returnID(string FirstName)
        {
            int Id = 0;

            MySqlDataReader reader = null;
            string query = "SELECT ID FROM people WHERE First_Name = @FirstName";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    conn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
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

        public void AddPerson(Person person)
        {
            string query = @"INSERT INTO people (First_Name ,Last_Name , Secret_Code)
                                   VALUES (@FirstName , @LastName , @SecretCode);";

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
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

        public Person CreatedPerson(string FirstName, string LastName)
        {
            string secretCode = new string(LastName.ToLower().Reverse().ToArray());

            Person person = new Person(FirstName, LastName, secretCode);

            return person;
        }

        public void checkIsPerson()
        {
            string FirstName = "";
            string LastName = "";
            List<string> FullName = new List<string>();
            while (FullName.Count < 1)
            {
                Console.WriteLine("enter firstname");
                LastName = Console.ReadLine();

                Console.WriteLine("enter lastname");
                FirstName = Console.ReadLine();
                
                string[] inputList = new string[2] { FirstName, LastName };

                FullName = FindFullName(inputList);
                if (FullName.Count == 2)
                {
                    Console.WriteLine("The full name is correct");
                }
                else
                {
                    Console.WriteLine("enter only letter and at least tow letter");
                }
            }
            if(!IsPerson(FirstName, LastName))
            {
                
                AddPerson(CreatedPerson(FirstName , LastName));
            }
        }



        public void IntelSubmissionFlow()
        {
            string FirstName;
            string LastName;

            Console.WriteLine("enter report");
            string Report = Console.ReadLine();

            string[] Repo = Report.Split(' ');
            List<string> FullName = FindFullName(Repo);
            if (FullName.Count == 2)
            {
                FirstName = FullName[0];
                LastName = FullName[1];

                if (!IsPerson(FirstName , LastName))
                {
                    AddPerson(CreatedPerson(FirstName, LastName));
                }
                try
                {
                    AddIntelRepots(Report, FirstName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"erorr add report into func IntelSubmissionFlow {ex.Message}");
                }

            }
            else
            {
                Console.WriteLine("full name no find in report");
            }
        }

        public void AddIntelRepots(string text , string FirsName)
        {
            int Id = 0;
            
            try
            {
              Id = returnID(FirsName);

            }
            catch(Exception ex)
            {
                Console.WriteLine($"error AddIntelRepots > returnID {ex.Message}");
            }
            if (Id > 0 && text.Length > 30)
            {
                string query = @"INSERT INTO intelreports (Reporter_Id , Target_Id ,Text)
                                    VALUES (@ReporterId  , @TargetId , @Text)";

                using (MySqlConnection conn = new MySqlConnection(connStr))
                {

                    using (MySqlCommand cmd = new MySqlCommand(query,conn))
                    {
                        try
                        {
                            conn.Open();
                            cmd.Parameters.AddWithValue("@ReporterId", Id);
                            cmd.Parameters.AddWithValue("@TargetId", Id);
                            cmd.Parameters.AddWithValue("@Text", text);
                            cmd.ExecuteNonQuery();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"error AddIntelRepots: {ex.Message}");
                        }
                    }
                }

            }
            
        }


        public List<string> FindFullName(string[] strings)
        {
            int isFullName = 0;
            string FirstName = "";
            string LastName = "";
            List<string> FullName = new List<string>();
            
            foreach (string s in strings)
            {
                
                if (s.Length > 1 && char.IsUpper(s[0]) && s.All(char.IsLetter))
                {
                    
                    if (isFullName == 0)
                    {
                        FirstName = s;
                        isFullName++;
                    }
                    else if (isFullName == 1)
                    {
                        LastName = s;
                        FullName.Add(FirstName);
                        FullName.Add(LastName);

                        break;
                    }
                }
                else
                {
                    isFullName = 0;
                }
            }
            Console.WriteLine(FullName.Count);
            return FullName;
            
        }
    }
}
