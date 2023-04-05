using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateTests.SharedConfiguration.Utility.Models
{

    public class ConvertRoot
    {
        public Motd? Motd { get; set; }
        public bool Success { get; set; }
        public Query? Query { get; set; }
        public Info? Info { get; set; }
        public bool Historical { get; set; }
        public DateTime? Date { get; set; }
        public double? Result { get; set; }
    }
}


