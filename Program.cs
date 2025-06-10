using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Malshinons.DAL;

namespace Malshinons
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DalPerson dalPerson = new DalPerson();
            dalPerson.IntelSubmissionFlow();
        }
    }
}
