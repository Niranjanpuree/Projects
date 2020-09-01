using Northwind.Core.Interfaces;
using Northwind.Core.Interfaces.DocumentMgmt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Northwind.Core.Services
{
    public class FolderService : IFolderService
    {
        private readonly IDocumentManagementService _documentManagementService;
        private readonly IFolderStructureMasterService _folderStructureMasterService;
        private readonly IFolderStructureFolderService _folderStructureFolderService;

        public FolderService(IDocumentManagementService documentManagementService,IFolderStructureMasterService folderStructureMasterService,
            IFolderStructureFolderService folderStructureFolderService)
        {
            _documentManagementService = documentManagementService;
            _folderStructureMasterService = folderStructureMasterService;
            _folderStructureFolderService = folderStructureFolderService;
        }

        public void CreateFolderTemplate(string contractGuid,string projectNumber, string resourceType, Guid resourceId, Guid userGuid)
        {
            
            var structureApplied = _documentManagementService.HasDefaultStructure(resourceType, resourceId);
            if (!structureApplied)
            {
                var masterData = _folderStructureMasterService.GetActive("ESSWeb", resourceType);
                if (masterData.Count() > 0)
                {
                    var templateFolders = _folderStructureFolderService.GetFolderTree(masterData.SingleOrDefault().FolderStructureMasterGuid);
                    _documentManagementService.ManageDefaultStructure(contractGuid,projectNumber, templateFolders, resourceType, resourceId, userGuid);
                }
            }
        }
    }
}
