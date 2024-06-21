using Dfinance.Core.Views.Inventory;
using Dfinance.Core.Views.PagePermission;

namespace Dfinance.AuthAppllication.Dto;

public class AuthResponseDto
{
   
    public string Token { get; set; }
    public UserInfo Users { get; set; }
    public List<treeview> UserPageListView { get; set; }
    public string Settings {  get; set; }
}
