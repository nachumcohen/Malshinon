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
    internal class Servis
    {
        public void IntelSubmissionFlow()
        {
            string FirstNameReporter;
            string LastNameReporter;
            string FirstNameTarget;
            string LastNameTarget;

            Console.WriteLine("enter report");
            string Report = Console.ReadLine();

            string[] Repo = Report.Split(' ');
            List<string> FullNameTarget = Class1.FindFullName(Repo);
            List<string> FullNameReporter = CheckIsPerson();

            if (FullNameReporter.Count == 2 && FullNameTarget.Count == 2)
            {
                FirstNameReporter = FullNameReporter[0];
                LastNameReporter = FullNameReporter[1];

                FirstNameTarget = FullNameTarget[0];
                LastNameTarget = FullNameTarget[1];

                if (!DalPerson2.IsPerson(FirstNameTarget, LastNameTarget))
                {
                    Person person = Class1.CreatedPerson(FirstNameTarget, LastNameTarget, "target");
                    DalPerson2.AddPerson(person);
                }
                try
                {
                    int idReporter = DalPerson2.returnID(FirstNameReporter);
                    int idTarget = DalPerson2.returnID(FirstNameTarget);

                    if (idReporter > 0 && idTarget > 0 && Report.Length > 30)
                    {
                        IntelReports intelReports = Class1.CreatedIntelRepots(idReporter, idTarget, Report);
                        DalIntelReports.AddIntelRepots(intelReports);
                    }
                    else
                    {
                        Console.WriteLine("full name no find in report");
                        return;
                    }
                    

                    string Type = Class1.ReturnType(idTarget);
                    DalPerson2.TypeChange(idTarget, Type);


                    DalIntelReports.ToIncreaseNumMentions(idReporter);
                    DalIntelReports.ToIncreaseNumReports(idReporter);

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

        public List<string> CheckIsPerson()
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

                FullName = Class1.FindFullName(inputList);
                if (FullName.Count == 2)
                {
                    Console.WriteLine("The full name is correct");
                }
                else
                {
                    Console.WriteLine("enter only letter and at least tow letter");
                }
            }
            if (!DalPerson2.IsPerson(FirstName, LastName))
            {

                DalPerson2.AddPerson(Class1.CreatedPerson(FirstName, LastName, "reporter"));
            }
            else
            {
                int id = DalPerson2.returnID(FirstName);
                string Type = Class1.ReturnType(id);
                DalPerson2.TypeChange(id, Type);
            }

            return FullName;
        }
    }
}
