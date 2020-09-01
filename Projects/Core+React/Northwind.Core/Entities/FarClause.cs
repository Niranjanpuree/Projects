using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities
{
    public class FarClause
    {
        public Guid FarClauseGuid { get; set; }
        public string Number { get; set; }
        public string Title { get; set; }
        public string Paragraph { get; set; }
        public bool IsDeleted { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}
