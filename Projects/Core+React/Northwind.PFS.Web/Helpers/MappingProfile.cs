using AutoMapper;
using Northwind.Core.Entities;
using Northwind.Core.Entities.ContractRefactor;
using Northwind.CostPoint.Entities;
using Northwind.PFS.Web.Models.ViewModels;
using Northwind.Web.Infrastructure.Models.ViewModels;

//using Northwind.Core.Entities;

namespace Northwind.PFS.Web.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProjectViewModel, Contracts>().ReverseMap();
            CreateMap<NotificationMessage, NotificationMessageViewModel>().ReverseMap();
            CreateMap<LaborCP, LaborPayrollViewModel>();
        }
    }
}
