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
                                2. Add IntelReports");

            string Choos = Console.ReadLine();

            switch (Choos)
            {
                case "1":
                    Servis.CheckIsPerson();
                    break;
                case "2":
                    Servis.IntelSubmissionFlow();
                    break;
            }
        }
    }
}
