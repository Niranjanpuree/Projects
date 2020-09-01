using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoCare.Product.Application.Infrastructure.Bus.Event;

namespace AutoCare.Product.Application.Models.Events
{
    public class MakeUpdatedEvent : Event
    {
        public MakeUpdatedEvent(int makeId, string makeName)
        {
            if (makeId == 0)
            {
                throw new ArgumentNullException(nameof(makeId));
            }
            if (string.IsNullOrWhiteSpace(makeName))
            {
                throw new ArgumentNullException(nameof(makeName));
            }

            MakeId = makeId;
            MakeName = makeName;
        }

        public int MakeId { get; private set; }
        public string MakeName { get; private set; }
    }
}
 