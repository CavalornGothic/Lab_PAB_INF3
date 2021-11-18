using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_PAB_INF3.Lab_1
{
    class ResearchTime
    {
        public DateTime Start { get; set; }
        public IEnumerable<double> Meas { get; set; }
        public DateTime End { get; set; }
    }
}
