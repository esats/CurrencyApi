using System;
using System.Collections.Generic;
using System.Text;

namespace ProteinCase.Application.Dto
{
    public class CurrencyChangesDto
    {
        public string Currency { get; set; }
        public string Date { get; set; }
        public decimal Rate { get; set; }
        public string Changes { get; set; }
    }
}
