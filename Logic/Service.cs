using Malshinons.DAL;
using Malshinons.Models;
using Malshinons.Modles;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinons.Logic
{
    internal static class Service
    {
        public static void ServiceIntelSubmission()
        {
            string FirstNameReporter;
            string LastNameReporter;
            string FirstNameTarget;
            string LastNameTarget;

            Console.WriteLine("enter report");
            string Report = Console.ReadLine();

            string[] Repo = Report.Split(' ');
            List<string> FullNameTarget = Logi.FindFullName(Repo);
            List<string> FullNameReporter = ServicePerson();

            if (FullNameReporter.Count == 2 && FullNameTarget.Count == 2)
            {
                FirstNameReporter = FullNameReporter[0];
                LastNameReporter = FullNameReporter[1];

                FirstNameTarget = FullNameTarget[0];
                LastNameTarget = FullNameTarget[1];

                if (!DalPerson.IsPerson(FirstNameTarget, LastNameTarget))
                {
                    Person person = Logi.CreatedPerson(FirstNameTarget, LastNameTarget, "target");
                    DalPerson.AddPerson(person);
                }
                try
                {
                    int idReporter = DalPerson.returnID(FirstNameReporter);
                    int idTarget = DalPerson.returnID(FirstNameTarget);

                    if (idReporter > 0 && idTarget > 0 && Report.Length > 30)
                    {
                        IntelReports intelReports = Logi.CreatedIntelRepots(idReporter, idTarget, Report);
                        DalIntelReports.AddIntelRepots(intelReports);
                    }
                    else
                    {
                        Console.WriteLine("full name no find in report");
                        return;
                    }
                    

                    string Type = Logi.ReturnType(idTarget);
                    DalPerson.TypeChange(idTarget, Type);


                    DalPerson.ToIncreaseNumMentions(idReporter);
                    DalPerson.ToIncreaseNumReports(idReporter);

                    Logi.PotentialThreatAlert(idTarget);

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

        public static List<string> ServicePerson()
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

                FullName = Logi.FindFullName(inputList);
                if (FullName.Count == 2)
                {
                    Console.WriteLine("The full name is correct");
                }
                else
                {
                    Console.WriteLine("enter only letter and at least tow letter");
                }
            }
            if (!DalPerson.IsPerson(FirstName, LastName))
            {

                DalPerson.AddPerson(Logi.CreatedPerson(FirstName, LastName, "reporter"));
            }
            else
            {
                int id = DalPerson.returnID(FirstName);
                string Type = Logi.ReturnType(id);
                DalPerson.TypeChange(id, Type);
                Logi.PotentialThreatAlert(id);
                ServiceAlert(id, 15);
                
            }

            return FullName;
        }

        public static void ServiceAlert(int id, int mineute)
        {
            Alert alert = DalIntelReports.GetPotentialAlert(id, mineute);
            if (alert == null)
            {
                Console.WriteLine("no alert");
            }
            else
            {
                if (alert.Reason == null)
                {
                    alert.Reason = "Rapid reports detected";
                }
                try
                {
                    DalAlerts.AddAlert(alert);
                    Console.WriteLine($"{id} add Alert");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"error checkAlert : {ex.Message}");
                }
            }
        }
    }
}
