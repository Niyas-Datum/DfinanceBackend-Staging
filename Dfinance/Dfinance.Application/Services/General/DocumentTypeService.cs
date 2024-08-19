using Dfinance.Application.Services.General.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.DataModels.Dto.General;
using Dfinance.Shared.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Dfinance.Application.Services.General
{
    public class DocumentTypeService : IDocumentTypeService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly ILogger<DocumentTypeService> _logger;
        public DocumentTypeService(DFCoreContext context, IAuthService authService, ILogger<DocumentTypeService> logger)
        {
            _authService = authService;
            _context = context;
            _logger = logger;
        }
        private CommonResponse PermissionDenied(string msg)
        {
            _logger.LogInformation("No Permission for " + msg);
            return CommonResponse.Error("No Permission ");
        }
        private CommonResponse PageNotValid(int pageId)
        {
            _logger.LogInformation("Page not Exists :" + pageId);
            return CommonResponse.Error("Page not Exists");
        }
        public CommonResponse FillDocTypeMaster()
        {
            int companyId = _authService.GetBranchId().Value;
            var docTypes = _context.FillDocTypeMasterView.FromSqlRaw($"Exec spDocumentTypes @Criteria='FillDocTypeMaster',@CompanyID={companyId}").ToList();
            return CommonResponse.Ok(docTypes);
        }
        public CommonResponse FillById(int id)
        {
            bool check = _context.DocType.Any(x => x.Id == id);
            if (!check)
                return CommonResponse.NotFound("Not Exists");
            var docType = _context.FillDocTypeByIdView.FromSqlRaw($"exec spDocumentTypes @Criteria='FillDocTypeWithID',@ID={id}").AsEnumerable().FirstOrDefault();
            return CommonResponse.Ok(docType);
        }
        public CommonResponse SaveDocType(int pageId, DocumentTypeDto docTypeDto)
        {
            try
            {
                if (!_authService.IsPageValid(pageId))
                {
                    return PageNotValid(pageId);
                }
                if (!_authService.UserPermCheck(pageId, 2))
                {
                    return PermissionDenied("Save DocumentType");
                }
                int createdBy = _authService.GetId().Value;
                DateTime createdOn = DateTime.Now;
                int branchId = _authService.GetBranchId().Value;
                byte active = 1;
                string criteria = "";
                if (docTypeDto.Id == null || docTypeDto.Id == 0)
                {
                    criteria = "InsertDocTypes";
                    SqlParameter newId = new SqlParameter("@NewID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    var res = _context.Database.ExecuteSqlRaw("Exec spDocumentTypes @Criteria={0},@Description={1},@ActiveFlag={2},@IsLCDoc={3},@IsPODoc={4},@CreatedBy={5},@CreatedOn={6},@Directory={7},@PIRDescription={8}," +
                        "@BranchID={9},@DocRowType={10},@IsLCMandatoryDoc={11},@NewID={12} OUTPUT", criteria, docTypeDto.DocumentName, 1, docTypeDto.LC ?? false, docTypeDto.PurchaseOrder ?? false, createdBy, createdOn, docTypeDto.Directory, docTypeDto.PIRDescription,
                        branchId, docTypeDto.DocEntry.Id, docTypeDto.MandatoryLC??false, newId);
                    int Id = (int)newId.Value;
                    _logger.LogInformation("DocumentType Saved Successfully");
                    return CommonResponse.Ok("Saved Successfully");
                }
                else
                {
                    bool check = _context.DocType.Any(x => x.Id == docTypeDto.Id);
                    if (!check)
                        return CommonResponse.NotFound("Not Exists");
                    criteria = "UpdateDocTypes";
                    var res = _context.Database.ExecuteSqlRaw("Exec spDocumentTypes @Criteria={0},@Description={1},@ActiveFlag={2},@IsLCDoc={3},@IsPODoc={4},@CreatedBy={5},@CreatedOn={6},@Directory={7},@PIRDescription={8}," +
                        "@BranchID={9},@DocRowType={10},@IsLCMandatoryDoc={11},@ID={12} ", criteria, docTypeDto.DocumentName, active, docTypeDto.LC ?? false, docTypeDto.PurchaseOrder ?? false, createdBy, createdOn, docTypeDto.Directory, docTypeDto.PIRDescription,
                        branchId, docTypeDto.DocEntry.Id, docTypeDto.MandatoryLC, docTypeDto.Id);
                    return CommonResponse.Ok("Updated Successfully");
                }
            }
            catch
            {
                _logger.LogError("Failed to Save DocumentType");
                return CommonResponse.Error("Failed to Save DocumentType");
            }
        }
        public CommonResponse DeleteDocType(int pageId, int Id)
        {
            try
            {
                if (!_authService.IsPageValid(pageId))
                {
                    return PageNotValid(pageId);
                }
                if (!_authService.UserPermCheck(pageId, 5))
                {
                    return PermissionDenied("Delete DocumentType");
                }
                bool check = _context.DocType.Any(x => x.Id == Id);
                if (!check)
                    return CommonResponse.NotFound("Not Exists");
                string criteria = "DeleteDocTypes";
                var del = _context.Database.ExecuteSqlRaw("Exec spDocumentTypes @Criteria={0},@ID={1}",criteria,Id);
                _logger.LogInformation("Document type Deleted Successfully");
                return CommonResponse.Ok("Deleted Successfully");
            }
            catch
            {
                _logger.LogError("Failed to Delete DocumentType");
                return CommonResponse.Error("Failed to Delete DocumentType");
            }
        }
    }
}
