using Dfinance.Application.Services.Interface;
using Dfinance.AuthAppllication.Authorization;
using Dfinance.AuthAppllication.Dto;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.AuthCore.Infrastructure;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Views.PagePermission;
using Dfinance.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Dfinance.Shared.Configuration.Service;

namespace Dfinance.AuthAppllication.Services;

public class AuthService : IAuthService
{
    private readonly DFCoreContext _dfCoreContext;
    private static AuthResponseDto? _User = null!;
    private readonly IEncryptService _encryptService;
    private readonly IJwtSecret _jwtsecret;
    private readonly AuthCoreContext _authCoreContext;
    private readonly IConfiguration _configuration;
    private readonly IConnectionServices _connectionServices;


    public AuthService(IJwtSecret jwtSecret, DFCoreContext dFCoreContext, IEncryptService encryptService, AuthCoreContext authCoreContext,
        IConfiguration configuration,
        IConnectionServices connectionServices)
    {
        _jwtsecret = jwtSecret;
        _dfCoreContext = dFCoreContext;
        _encryptService = encryptService;
        _authCoreContext = authCoreContext;
        _configuration = configuration;
        _connectionServices = connectionServices;
    }

    public CommonResponse Authenticate(AuthenticateRequestDto model)
    {
        try
        {
            List<UserPageListView> loginMenu = null;
            var Passwordtemp = _encryptService.Encrypt(model.Password);
            // GET LOGININFO
            int mod = 10;
            var existUser=(from E in _dfCoreContext.MaEmployees
                           join ED in _dfCoreContext.MaEmployeeBranchDet on E.Id equals ED.EmployeeId
                           where E.Username==model.Username && E.Password== Passwordtemp && ED.BranchId==model.Branch.Id
                           select E.Id).Any();
            if (!existUser) 
                return CommonResponse.Invalid(new { Message = "Authentication failed! Try again" });
            var userData = _dfCoreContext.UserInfo.FromSqlRaw($"EXEC spAuthendication  @Mode='{mod}', @BranchID='{model.Branch.Id}', @Username='{model.Username}', @Password='{Passwordtemp}'").AsEnumerable().FirstOrDefault();
            int empid = userData.EmployeeID;
            // Get Menu only if authentication is successful
            if (userData != null)
            {
                string criteria = "FillMenuWeb";
                string Language = "English";
                loginMenu = _dfCoreContext.UserPageListView.FromSqlRaw($"EXEC MenuSP @Criteria='{criteria}',@Language='{Language}',@EmployeeID='{empid}',@BranchID='{model.Branch.Id}'").ToList();

                var data = GetTree(loginMenu, null);

                var Token = _jwtsecret.GenerateJwtToken(userData);

                //get settings from Masettings
                var settingsPath = _configuration["AppSettings:SettingsJsonFilePath"];
                var json = File.ReadAllText(settingsPath);
                string[] keys = JsonConvert.DeserializeObject<string[]>(json);
                var settings = _dfCoreContext.MaSettings
            .Where(m => keys.Contains(m.Key))
            .Select(m => new
            {
                Key = m.Key,
                Value = (
                    m.Value.ToLower() == "true" ||
                    m.Value.ToLower() == "yes" ||
                    m.Value == "1"
                ) ? "true" : "false"
            }).ToList();
                var jsonSettings = JsonConvert.SerializeObject(settings, Formatting.Indented);

                _User = new AuthResponseDto
                {
                    Users = userData,
                    UserPageListView = data,
                    Token = Token,
                    Settings=jsonSettings
                };
                //Log.Information("Authentication successful. AuthResponse: {@AuthResponse}", authResponse);
                return CommonResponse.Ok(_User);
            }
            else 
            {
                // Authentication failed
                return CommonResponse.Invalid(new { Message = "An error occurred during authentication." });
            }
        }
        catch (Exception ex) 
        {
            // Log the exception details
           // Log.Error("An error occurred during authentication.");
            return CommonResponse.Invalid(new { Message = "An error occurred during authentication." });
        }
    }


    public List<treeview> GetTree(List<UserPageListView> list, int? parent)
    {
        return list.Where(x => x.ParentID == parent).Select(x => new treeview
        {
            ID = x.ID,
            MenuText = x.MenuText,
            MenuValue = x.MenuValue,
            Url = x.Url,
            ParentID = x.ParentID ?? 0, // Use 0 if ParentID is null
            IsPage = x.IsPage,
            VoucherID = x.VoucherID,
            ShortcutKey = x.ShortcutKey,
            ToolTipText = x.ToolTipText,
            IsView = x.IsView,
            IsCreate = x.IsCreate,
            IsCancel = x.IsCancel,
            IsApprove = x.IsApprove,
            IsEditApproved = x.IsEditApproved,
            IsHigherApprove = x.IsHigherApprove,
            IsPrint = x.IsPrint,
            IsEMail = x.IsEMail,

            Submenu = GetTree(list, x.ID)
        }).ToList();
    }
    public int? GetId()
    {
        return _User.Users.EmployeeID;
    }

    public string GetUserName()
    {
        return _User.Users.FirstName;
    }

    public int? GetBranchId()
    {
        return _User.Users.BranchId;
    }
    public string? GetUserRole()
    {
        return _User.Users.UserRole;
    }

    public AuthResponseDto GetUserById(int? id)
    {
        if (_User == null) return null!;
        if (_User.Users.EmployeeID == id) return _User;

        return null!;

    }

    public string SetCon(DropdownLoginDto company)
    {
        var companyInfo = _authCoreContext.Companies.FirstOrDefault(c => c.Id == company.Id);

            if (companyInfo == null)
             {
            return null;
            }
         var con = $"Data Source={companyInfo.ServerName};TrustServerCertificate=true;Initial Catalog={companyInfo.DatabaseName};User ID=sa;Password=Datum123!";
   
            return con ;
        }

    /// <summary>
    /// @use:  check every request user have permission 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="pageid"></param>
    /// <param name="branchid"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    ///[1: view, 2.create, 3.Edit, 4.Cancel, 5.Delete, 6.approve,7.editapprove, 8.higherapprove,9: print
    
    public bool UserPermCheck(int pageid, int method )
    {
         var Userdetid  = _dfCoreContext.MaEmployeeBranchDet.Where(x=> x.EmployeeId == _User.Users.EmployeeID && x.BranchId == _User.Users.BranchId ).Select(x=> x.Id).FirstOrDefault();
        var UserPermission = _dfCoreContext.UserRolePermission
                                .Where(x=>x.UserDetailsId == Userdetid
                                && x.PageMenuId == pageid).FirstOrDefault();
        if (UserPermission == null) return false;
        switch (method)
        {
            case 1:{ return UserPermission.IsView ? true : false;}
            case 2:{return UserPermission.IsCreate ? true : false;}
            case 3:{return UserPermission.IsEdit ? true : false;}
            case 4:{return UserPermission.IsCancel ? true : false;}
            case 5:{return UserPermission.IsDelete ? true : false;}
            case 6: { return UserPermission.IsApprove ? true : false; }
            case 7: { return UserPermission.IsEditApproved ? true : false; }
            case 8: { return UserPermission.IsHigherApprove ? true : false; }
            case 9: { return UserPermission.IsPrint ? true : false; }
            default: { return false; }
        
        }
    }

    public bool IsPageValid(int pageId)
    {
        var page = _dfCoreContext.MaPageMenus.Any(m => m.Id == pageId);
        if (!page)
            return false;
        return true;
    }
    /// <summary>
    /// This function used for application Qrcode read and return valid data 
    /// </summary>
    /// <param name="Qrtext">Qrcode</param>
    /// <returns> Company name , api URL, branch name</returns>
    public async Task<CommonResponse> AppQrRead(string qrText)
    {

        var QRDECODE = Convert.FromBase64String(qrText);
        string[] DecodedArray = System.Text.Encoding.UTF8.GetString(QRDECODE).Split("$#$");
        if (DecodedArray.Length == 4)
        {
            var _companyData = _authCoreContext.Companies.Where(x => x.Id == Convert.ToInt32(DecodedArray[1])).Select(x => new DropdownLoginDto { Id = x.Id, Value = x.Name }).FirstOrDefault();
            var _branchData = new DropdownLoginDto() { Id = Convert.ToInt32(DecodedArray[2]), Value = DecodedArray[3] };
           
            return CommonResponse.Ok(new { apilink = DecodedArray[0], companyData = _companyData, branchData = _branchData });
            
        }
        return CommonResponse.Error("Qrcode no valid");
    }
   
    public CommonResponse LogOut()
    {       
        _connectionServices.RemoveConnection();
        _connectionServices.setconkey(null);
        return CommonResponse.Ok("LogOut Successfully");
    }
}
