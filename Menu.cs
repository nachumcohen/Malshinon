using Malshinons.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZstdSharp.Unsafe;
using Malshinons.Logic;

namespace Malshinons
{
    internal class Menu
    {
        public Menu()
        {
            Console.WriteLine(@"Chooce
                                1. Add Person
                                2. Add IntelReports
                                3. is id in alert");

            string Choos = Console.ReadLine();

            switch (Choos)
            {
                case "1":
                    Service.ServicePerson();
                    break;
                case "2":
                    Service.ServiceIntelSubmission();
                    break;
                case "3":
                    Service.ServiceAlert();
                    break;
            }
        }
    }
}
