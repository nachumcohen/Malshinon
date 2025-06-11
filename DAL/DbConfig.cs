using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malshinons.DAL
{
  
        public static class DbConfig
        {
            public static string ConnectionString { get; } =
                "server=localhost;user=root;password= ;database=malshinon;";
        }
}
