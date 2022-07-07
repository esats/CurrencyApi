using System;
using System.Collections.Generic;
using System.Text;

namespace ProteinCase.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now.Date;
        public DateTime UpdateddDate { get; set; } = DateTime.Now.Date;
        public bool IsActive { get; set; } = true;
    }
}
