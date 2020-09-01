using AutoCare.Product.VcdbSearchIndex.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoCare.Product.VcdbSearch.Indexing.ApplicationService
{
    public interface IVehicleToBedConfigIndexingService
    {
        Task UploadDocumentsAsync(List<VehicleToBedConfigDocument> vehicleToBedConfigDocuments);
        Task UploadDocumentAsync(VehicleToBedConfigDocument vehicleToBedConfigDocuments);
        Task UpdateVehicleToBedConfigChangeRequestIdAsync(int vehicleToBedConfigId, long vehicleToBedConfigChangeRequestId);
        Task UpdateBedConfigChangeRequestIdAsync(string vehicleToBedConfigId,long bedConfigChangeRequestId);
        Task DeleteDocumentByVehicleToBedConfigIdAsync(string vehicleToBedConfigId);
    }
}
