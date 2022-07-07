using System;
using System.Collections.Generic;
using System.Text;

namespace ProteinCase.Application.Dto
{
    public class CurrencyResponseDto
    {
        public string Currency { get; set; }
        public string LastUpdated { get; set; }
        public decimal CurrentRate { get; set; }
    }
}
