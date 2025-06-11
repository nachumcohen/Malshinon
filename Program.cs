using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Malshinons.DAL;
using Malshinons.Logic;
using Malshinons.Models;
using Malshinons.Modles;


namespace Malshinons
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Servis servis = new Servis();
            //servis.IntelSubmissionFlow();
            //Alert alert = DalAlerts.CreateAlert(16 , 150000);
            //alert.Reason = "Rapid reports detected";
            //DalAlerts.AddAlert(alert);
            Person person = DalPerson.getPersonByName("Leila");
            Console.WriteLine(person.Type);

            Alert alert = DalAlerts.GetAlert(1);
            Console.WriteLine(alert.Time1);
            
            
        }
    }
}
