using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductServer
{
    public class ProductServerConfiguration
    {
        public int ServerPort { get; set; } = 1024;
        public bool AutoStart { get; set; }
    }
}
