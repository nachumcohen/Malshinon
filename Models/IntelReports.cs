using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinons.Models
{
    internal class IntelReports
    {
            public int Id { get; set; }
            public int ReporterId { get; set; }
            public int TargetId { get; set; }
            public string Text { get; set; }
            public IntelReports(int reporterId , int targetId , string text)
            {
                ReporterId = reporterId;
                TargetId = targetId;
                Text = text;
            }
    }
}
