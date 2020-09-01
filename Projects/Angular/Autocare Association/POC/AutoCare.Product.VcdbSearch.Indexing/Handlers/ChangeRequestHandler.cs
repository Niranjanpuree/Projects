using System;
using System.Threading.Tasks;
using AutoCare.Product.Infrastructure.Command;
using AutoCare.Product.Vcdb.Model;
using AutoCare.Product.VcdbSearch.Indexing.Command;

namespace AutoCare.Product.VcdbSearch.Indexing.Handlers
{
    public class ChangeRequestHandler : ICommandHandler<ApplyChangeRequestChanges>
    {
        private readonly ICommandBus _commandBus;
        public ChangeRequestHandler(ICommandBus commandBus)
        {
            _commandBus = commandBus;
        }

        public async Task Handle(ApplyChangeRequestChanges applyChangeRequestChangesCommand)
        {
            if (applyChangeRequestChangesCommand == null)
            {
                throw new ArgumentNullException(nameof(applyChangeRequestChangesCommand));
            }

            if (applyChangeRequestChangesCommand.Entity == typeof (Make).Name)
            {
                await _commandBus.SendAsync(new ApplyMakeChangeRequestChanges
                {
                    ChangeType = applyChangeRequestChangesCommand.ChangeType,
                    Entity = applyChangeRequestChangesCommand.Entity,
                    Payload = applyChangeRequestChangesCommand.Payload,
                    EntityId = applyChangeRequestChangesCommand.EntityId,
                    ChangeRequestId = applyChangeRequestChangesCommand.ChangeRequestId
                });
            }
        }
    }
}
