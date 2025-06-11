using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Malshinons.Modles;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.X509;
using static System.Net.Mime.MediaTypeNames;
using Malshinons.Models;



namespace Malshinons.DAL
{
    internal class DalPerson1
    {
        private string connStr = "server=localhost;user=root;password= ;database=malshinon;";
        
        public bool IsPerson(string FirstName , string LastName)
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

        public int returnID(string FirstName)
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

        public void AddPerson(Person person)
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
       //logic
        public Person CreatedPerson(string FirstName, string LastName , string type)
        {
            string secretCode = new string(LastName.ToLower().Reverse().ToArray());

            Person person = new Person(FirstName, LastName, secretCode ,type);

            return person;
        }


        //logic
        public List<string> checkIsPerson()
        {
            string FirstName = "";
            string LastName = "";
            List<string> FullName = new List<string>();
            while (FullName.Count < 1)
            {
                Console.WriteLine("enter firstname");
                FirstName = Console.ReadLine();

                Console.WriteLine("enter lastname");
                LastName = Console.ReadLine();
                
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

                AddPerson(CreatedPerson(FirstName , LastName, "reporter"));
            }
            else
            {
                int id = returnID(FirstName);
                string Type = ReturnType(id);
                TypeChange(id, Type);
            }

            return FullName;
        }
        //logic
        public void IntelSubmissionFlow()
        {
            string FirstNameReporter;
            string LastNameReporter;
            string FirstNameTarget;
            string LastNameTarget;

            Console.WriteLine("enter report");
            string Report = Console.ReadLine();

            string[] Repo = Report.Split(' ');
            List<string> FullNameTarget = FindFullName(Repo);
            List<string> FullNameReporter = checkIsPerson();
            if (FullNameReporter.Count == 2 && FullNameTarget.Count == 2)
            {
                FirstNameReporter = FullNameReporter[0];
                LastNameReporter = FullNameReporter[1];

                FirstNameTarget = FullNameTarget[0];
                LastNameTarget = FullNameTarget[1];

                if (!IsPerson(FirstNameTarget, LastNameTarget))
                {
                    AddPerson(CreatedPerson(FirstNameTarget, LastNameTarget , "target"));
                }
                try
                {
                    AddIntelRepots(Report, FirstNameReporter, FirstNameTarget);
                    int id = returnID(FirstNameTarget);
                    string Type = ReturnType(id);
                    TypeChange(id, Type);
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

        public IntelReports CreatedIntelRepots(int reporterId, int targetId, string text)
        {
            IntelReports intelReports = new IntelReports(reporterId, targetId, text);
            return intelReports;
        }


        public void AddIntelRepots(string text , string FirsNameReporter , string FirsNameTarget)
        {
            int Id1 = 0;
            int Id2 = 0;
            
            
             Id1 = returnID(FirsNameReporter);
             Id2 = returnID(FirsNameTarget);
              
            
            if (Id1 > 0 && Id2 > 0 && text.Length > 30)
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
                            cmd.Parameters.AddWithValue("@ReporterId", Id1);
                            cmd.Parameters.AddWithValue("@TargetId", Id2);
                            cmd.Parameters.AddWithValue("@Text", text);
                            cmd.ExecuteNonQuery();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"error AddIntelRepots: {ex.Message}");
                        }
                    }
                }
                try
                {
                    ToIncreaseNumMentions(Id1);
                    ToIncreaseNumReports(Id2);
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"ToIncrease{ex.Message}");
                }

            }
            else
            {
                Console.WriteLine("enter text 30 chars or id < 1");
            }
            
        }
        //logic
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

        public void ToIncreaseNumReports(int Id)
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

        public void ToIncreaseNumMentions(int Id)
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

        public int AvgText(int id)
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




        public int CountReporterId(int id)
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

        public int CountTargetId(int id)
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


        public void TypeChange(int id , string type)
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
        //logic
        public string ReturnType(int id)
        {
            string Type = "";
            try
            {
                int avgText = AvgText(id);
                int countReporterId = CountReporterId(id);
                int countTargetId = CountTargetId(id);


                if (avgText >= 100 && countReporterId >= 10)
                {
                    Type = "potential_agent";
                }
                else if (countReporterId > 0 && countTargetId > 0)
                {
                    Type = "both";
                }
                else if (countReporterId > 0)
                {
                    Type = "reporter";
                }
                else if (countTargetId > 0)
                {
                    Type = "target";
                }


            }
            catch (Exception e)
            {
                Console.WriteLine($"error {e.Message}");
            }
            return Type;
        }

    
 //logic                   
        public void potentialThreatAlert(int id)
        {
            try
            {
                if (CountTargetId(id) >= 20)
                {
                    Console.WriteLine($"{id} potential threat alert");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error potentialThreatAlert{ex.Message}");
            }
        }
    }
}
