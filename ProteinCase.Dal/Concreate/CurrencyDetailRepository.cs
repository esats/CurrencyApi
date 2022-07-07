using ProteinCase.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProteinCase.Dal.Concreate
{
    public class CurrencyDetailRepository : Repository<CurrencyDetail>
    {
        public CurrencyDetailRepository(CurrencyContext dbContext) : base(dbContext)
        {
        }
    }
}
