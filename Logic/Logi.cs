using Malshinons.DAL;
using Malshinons.Models;
using Malshinons.Modles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace Malshinons.Logic
{
    internal static class Class1
    {
        public static Person CreatedPerson(string FirstName, string LastName, string type)
        {
            string secretCode = new string(LastName.ToLower().Reverse().ToArray());

            Person person = new Person(FirstName, LastName, secretCode, type);

            return person;
        }

        public static IntelReports CreatedIntelRepots(int reporterId, int targetId, string text)
        {
            IntelReports intelReports = new IntelReports(reporterId, targetId, text);
            return intelReports;
        }

        public static string ReturnType(int id)
        {
            string Type = "";
            try
            {
                int avgText = DalIntelReports.AvgText(id);
                int countReporterId = DalIntelReports.CountReporterId(id);
                int countTargetId = DalIntelReports.CountTargetId(id);


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

        public static List<string> FindFullName(string[] strings)
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

        public static void PotentialThreatAlert(int id)
        {
            try
            {
                if (DalIntelReports.CountTargetId(id) >= 20)
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
