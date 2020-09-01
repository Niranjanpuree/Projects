using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.Core.Entities
{
   public class QuestionaireManagerType
    {
        public Guid QuestionaireManagerTypeGuid { get; set; }
        public string ManagerType { get; set; }
        public string ManagerTypeNormalize { get; set; }
    }
}
