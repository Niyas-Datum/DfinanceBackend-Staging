using Dfinance.Application.Dto.Inventory;
using Dfinance.Application.Services.Inventory.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Domain;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Views.Inventory;
using Dfinance.Shared.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Application.Services.Inventory
{
    public class AreaMasterService : IAreaMasterService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        public AreaMasterService(DFCoreContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        /************* Fill all Area  *******************/
        public CommonResponse FillAreaMaster()
        {
            try
            {
                string criteria = "FillMaster";
                var result = _context.SpFillAreaMasterG.FromSqlRaw($"EXEC SpArea @Criteria='{criteria}'").ToList();
                var res = result.Select(x => new SpFillAreaMasterG
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    IsGroup = x.IsGroup
                }).ToList();
                return CommonResponse.Ok(res);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error();
            }
        }

        /************* Fill Area By Id *******************/
        public CommonResponse FillAreaMasterById(int Id)
        {
            try
            {
                var AId = _context.MaArea.Where(i => i.Id == Id).
                   Select(i => i.Id).
                   SingleOrDefault();
                if (AId == null)
                {
                    return CommonResponse.NotFound("Area Not Found");
                }
                string criteria = "FillArea";
                var result = _context.SpFillAreaMasterByIdG.FromSqlRaw($"EXEC SpArea @Criteria='{criteria}',@ID='{Id}'").ToList();
                var res = result.Select(x => new SpFillAreaMasterByIdG
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    IsGroup = x.IsGroup,
                    ParentId = x.ParentId,
                    ParentName = x.ParentName,
                    CreatedBranchId = x.CreatedBranchId,
                    CreatedBy = x.CreatedBy,
                    CreatedOn = x.CreatedOn,
                    Active = x.Active,
                    Note = x.Note
                }).ToList();
                return CommonResponse.Ok(res);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error();
            }
        }

        /****************** Save Area  *******************/
        public CommonResponse SaveAreaMaster(MaAreaDto maAreaDto)
        {
            try
            {
                int CreatedBranchId = _authService.GetBranchId().Value;
                int CreatedBy = _authService.GetId().Value;
                DateTime CreatedOn = DateTime.Now;
                string criteria = "InsertMaArea";
                SqlParameter newIdParameter = new SqlParameter("@NewID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                _context.Database.ExecuteSqlRaw("EXEC SpArea @Criteria={0},@Code={1},@Name={2},@Note={3},@ParentID={4},@IsGroup={5}," +
                    "@CreatedBy={6},@CreatedBranchID={7},@CreatedOn={8},@Active={9},@NewID={10} OUTPUT",
                    criteria,
                    maAreaDto.Code,
                    maAreaDto.Name,
                    maAreaDto.Description,
                    maAreaDto.Group,
                    maAreaDto.IsGroup,
                    CreatedBy,
                    CreatedBranchId,
                    CreatedOn,
                    maAreaDto.Active,
                    newIdParameter
                    );
                var newId = newIdParameter.Value;
                return CommonResponse.Created(maAreaDto.Code + ", " + maAreaDto.Name + " Created Successfully");
            }
            catch (Exception ex)
            {
                return CommonResponse.Error();
            }
        }

        /****************** Update Area  *******************/
        public CommonResponse UpdateAreaMaster(MaAreaDto maAreaDto, int Id)
        {
            try
            {
                var AId = _context.MaArea.Where(i => i.Id == Id).
                   Select(i => i.Id).
                   SingleOrDefault();
                if(AId==null)
                {
                    return CommonResponse.NotFound("Area Not Found");
                }
                int CreatedBranchId = _authService.GetBranchId().Value;
                int CreatedBy = _authService.GetId().Value;
                DateTime CreatedOn = DateTime.Now;              
                string criteria = "UpdateMaArea";
                var result = _context.Database.ExecuteSqlRaw($"EXEC SpArea @Criteria='{criteria}',@ID='{Id}',@Code='{maAreaDto.Code}',@Name='{maAreaDto.Name}',@Note='{maAreaDto.Description}',@ParentID='{maAreaDto.Group}',@IsGroup='{maAreaDto.IsGroup}',@CreatedBy='{CreatedBy}',@CreatedOn='{CreatedOn}',@CreatedBranchID='{CreatedBranchId}',@Active='{maAreaDto.Active}'");
                return CommonResponse.Ok("AreaMaster Updated Successfully");               

            }
            catch (Exception ex)
            {
                return CommonResponse.Error();
            }
        }

        /****************** Delete Area  *******************/
        public CommonResponse DeleteAreaMaster(int Id)
        {
            try
            {              
                string msg = null;                          
                var name = _context.MaArea.Where(i => i.Id == Id).
                    Select(i => i.Name).
                    SingleOrDefault();
                if (name == null)
                {
                    msg = "Area Not Found";
                }
                else
                {
                    string criteria = "DeleteMaArea";
                    var result = _context.Database.ExecuteSqlRaw($"EXEC SpArea @Criteria='{criteria}',@ID='{Id}'");
                    msg = name + " Is Deleted Successfully";
                }
                return CommonResponse.Ok(msg);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error();
            }
        }

    }
}
