using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProteinCase.Entities
{
    public class CurrencyDetail : BaseEntity
    {
        [ForeignKey("CurrencyMaster")]
        public int CurrencyMasterId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public Decimal ForexBuying { get; set; }
        public Decimal ForexSelling { get; set; }
        public virtual CurrencyMaster CurrencyMaster { get; set; }
    }
}

