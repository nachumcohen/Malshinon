using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Malshinons.DAL;
using Malshinons.Logic;

namespace Malshinons
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Servis servis = new Servis();
            servis.IntelSubmissionFlow();

            
        }
    }
}
