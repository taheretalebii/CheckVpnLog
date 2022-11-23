using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckLog
{
    public class LogDto
    {
        public DateTime Date { get; set; }
        public string IP { get; set; }
        public string Username { get; set; }

        public bool IsDuplicate { get; set; }
    }
}
