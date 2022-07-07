using ProteinCase.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProteinCase.Dal.Concreate
{
    public class CurrencyMasterRepository : Repository<CurrencyMaster>
    {
        public CurrencyMasterRepository(CurrencyContext dbContext) : base(dbContext)
        {
        }
    }
}
