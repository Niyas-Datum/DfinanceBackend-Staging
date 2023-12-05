using Dfinance.Application.Services.Interface;
using Dfinance.AuthAppllication.Authorization;
using Dfinance.AuthAppllication.Dto;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.AuthCore.Infrastructure;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Views.PagePermission;
using Dfinance.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Dfinance.AuthAppllication.Services;

public class AuthService : IAuthService
{
    private readonly DFCoreContext _dfCoreContext;
    private static AuthResponseDto? _User = null!;
    private readonly IEncryptService _encryptService;
    private readonly IJwtSecret _jwtsecret;
    private readonly AuthCoreContext _authCoreContext;
  


    public AuthService(IJwtSecret jwtSecret, DFCoreContext dFCoreContext, IEncryptService encryptService, AuthCoreContext authCoreContext)
    {
        _jwtsecret = jwtSecret;
        _dfCoreContext = dFCoreContext;
        _encryptService = encryptService;
        _authCoreContext = authCoreContext;
       
    }

    public CommonResponse Authenticate(AuthenticateRequestDto model)
    {
        try
        {
            List<UserPageListView> loginMenu = null;
            var Passwordtemp = _encryptService.Encrypt(model.Password);
            // GET LOGININFO
            int mod = 10;

            var userData = _dfCoreContext.UserInfo.FromSqlRaw($"EXEC spAuthendication  @Mode='{mod}', @BranchID='{model.Branch.Id}', @Username='{model.Username}', @Password='{Passwordtemp}'").AsEnumerable().FirstOrDefault();

            // Get Menu only if authentication is successful
            if (userData != null)
            {
                string criteria = "FillMenuWeb";
                string Language = "English";
                loginMenu = _dfCoreContext.UserPageListView.FromSqlRaw($"EXEC MenuSP @Criteria='{criteria}',@Language='{Language}',@EmployeeID='{94}',@BranchID='{model.Branch.Id}'").ToList();

                var data = GetTree(loginMenu, null);

                var Token = _jwtsecret.GenerateJwtToken(userData);


                _User = new AuthResponseDto
                {
                    Users = userData,
                    UserPageListView = data,
                    Token = Token,
                };
                //Log.Information("Authentication successful. AuthResponse: {@AuthResponse}", authResponse);
                return CommonResponse.Ok(_User);
            }
            else
            {
                // Authentication failed
                return CommonResponse.Error();
            }
        }
        catch
        {
            // Log the exception details
           // Log.Error("An error occurred during authentication.");
            return CommonResponse.Error();
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
    }
