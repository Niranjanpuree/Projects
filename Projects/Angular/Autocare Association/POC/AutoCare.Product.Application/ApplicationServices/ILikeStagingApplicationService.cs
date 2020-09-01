using System.Collections.Generic;
using System.Threading.Tasks;
using AutoCare.Product.Application.Models.BusinessModels;
using AutoCare.Product.Vcdb.Model;


namespace AutoCare.Product.Application.ApplicationServices
{
    public interface ILikeStagingApplicationService: IVcdbApplicationService<LikesStaging>
    {
        Task<LikesModel> GetLikesByChangeRequestId(long changeRequestId,string currentUser);
        Task<IList<LikesModel>> GetAllLikedBy(long changeRequestId);
    }
}
