using System;
using System.Collections.Generic;
using System.Text;

namespace ProteinCase.Entities
{
    public class CurrencyMaster : BaseEntity
    {
        public List<CurrencyDetail> CurrencyDetails { get; set; }
    }
}
