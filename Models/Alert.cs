using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinons.Models
{
    internal class Alert
    {
        public int TargetId { get; set; }
        public DateTime Time1 { get; set; }
        public DateTime Time2 { get; set; }
        public string Reason { get; set; }
    }
}
