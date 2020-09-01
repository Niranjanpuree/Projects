using System;
using System.Linq;
using System.Threading.Tasks;
using AutoCare.Product.Infrastructure.Command;
using AutoCare.Product.Infrastructure.Serializer;
using AutoCare.Product.Vcdb.Model;
using AutoCare.Product.VcdbSearch.Indexing.Command;
using AutoCare.Product.VcdbSearch.Indexing.RepositoryServices;
using AutoCare.Product.VcdbSearch.Model;
using AutoCare.Product.VcdbSearch.RepositoryService;

namespace AutoCare.Product.VcdbSearch.Indexing.Handlers
{
    public class MakeHandler : 
        ICommandHandler<ApplyMakeChangeRequestChanges>,
        ICommandHandler<ApplyMakeNameChange>
    {
        private readonly IVehicleIndexingRepositoryService _vehicleIndexRepositoryService;
        private readonly IVehicleSearchRepositoryService _vehicleSearchRepositoryService;
        private readonly ITextSerializer _serializer;

        public MakeHandler(IVehicleIndexingRepositoryService vehicleIndexRepositoryService,
            IVehicleSearchRepositoryService vehicleSearchRepositoryService, ITextSerializer serializer)
        {
            _vehicleIndexRepositoryService = vehicleIndexRepositoryService;
            _vehicleSearchRepositoryService = vehicleSearchRepositoryService;
            _serializer = serializer;
        }

        public async Task Handle(ApplyMakeChangeRequestChanges applyChangeRequestChanges)
        {
            if (String.IsNullOrWhiteSpace(applyChangeRequestChanges?.Entity))
            {
                throw new ArgumentNullException(nameof(applyChangeRequestChanges));
            }

            if (!applyChangeRequestChanges.Entity.ToLower().Equals(typeof (Make).Name.ToLower()))
            {
                throw new ArgumentException("Change request type invalid for applying make changes");
            }

            if (String.IsNullOrWhiteSpace(applyChangeRequestChanges.Payload))
            {
                throw new ArgumentNullException(nameof(applyChangeRequestChanges));
            }

            if (applyChangeRequestChanges.ChangeType != ChangeType.Modify)
            {
                return;
            }

            var make = _serializer.Deserialize<Make>(applyChangeRequestChanges.Payload);
            if (make == null)
            {
                throw new ArgumentException("Make change request has some invalid payload");
            }

            await Handle(new ApplyMakeNameChange {Make = make});
        }

        public async Task Handle(ApplyMakeNameChange payload)
        {
            if (payload?.Make == null)
            {
                return;
            }

            var make = payload.Make;

            var vehicleSearchResult = await _vehicleSearchRepositoryService.SearchAsync(null, x => x.MakeId == make.Id) as VehicleSearchResult;
            if (vehicleSearchResult?.Documents == null || !vehicleSearchResult.Documents.Any())
            {
                return;
            }

            foreach (var vehicle in vehicleSearchResult.Documents)
            {
                vehicle.MakeName = make.Name;
            }

            var documentIndexingResult =
                await _vehicleIndexRepositoryService.UpdateDocumentsAsync(vehicleSearchResult.Documents.ToList());
        }
    }
}
