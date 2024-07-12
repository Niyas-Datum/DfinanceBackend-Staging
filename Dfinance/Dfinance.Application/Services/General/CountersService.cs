using Dfinance.Application.Services.General.Interface;
using Dfinance.AuthAppllication.Services.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Views.General;
using Dfinance.DataModels.Dto.General;
using Dfinance.Shared.Domain;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Net;

namespace Dfinance.Application.Services.General
{
    public class CountersService  : ICountersService
    {
        private readonly DFCoreContext _context;
        private readonly IAuthService _authService;
        private readonly ILogger<CountersService> _logger;

        public CountersService(DFCoreContext context, IAuthService authService, ILogger<CountersService> logger)
        {
            _context = context;
            _authService = authService;
            _logger = logger;
        }

        public CommonResponse FillMaster()
        {
            try
            {
                var result = _context.FillCounters.FromSqlRaw($"EXEC MaCountersSP @Criteria ='FillMaster'").ToList();
                _logger.LogInformation("Counters filled!");
                return CommonResponse.Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }

        public CommonResponse FillCountersById(int Id) 
        {
            try
            {
                var counter = _context.InvMaCounters.Where(i => i.Id == Id).Select(i => i.Id).SingleOrDefault();
                if (counter == 0)
                {
                    return CommonResponse.NotFound("Id is not found!");
                }
                var counters = _context.FillCountersById.FromSqlRaw($"EXEC MaCountersSP @Criteria ='FillWithId', @Id = '{Id}'").AsEnumerable().FirstOrDefault();
                _logger.LogInformation("Counter Filled by Id");
                return CommonResponse.Ok(counters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }

        //fetch MachineName and Ip in AddNew click
        public CommonResponse GetNameandIp()
        {
            try
            {
                var machinename = Environment.MachineName;
                var machineIp = Dns.GetHostByName(Environment.MachineName).AddressList[1].ToString(); 
                return CommonResponse.Ok(new { machinename, machineIp });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }

        public CommonResponse SaveCounters(CounterDto counterDto)
        {
            try
            {
                string criteria = "Insert";
                SqlParameter newIdparam = new SqlParameter("@NewID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                var data = _context.Database.ExecuteSqlRaw("EXEC MaCountersSP @Criteria ={0},@MachineName={1}, @CounterCode={2}, @CounterName={3}, @MachineIP={4}, @Active={5},@NewID={6} OUTPUT", criteria,
                            counterDto.MachineName,counterDto.CounterName,counterDto.CounterCode,counterDto.MachineIP,counterDto.Active,newIdparam);
                int NewIdUser = (int)newIdparam.Value;
                _logger.LogInformation("Counter created successfully");
                return CommonResponse.Ok("Counter created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
        }

        public CommonResponse UpdateCounters(CounterDto counterDto, int PageId)
        {
            try
            {
                
                if (!_authService.IsPageValid(PageId))
                {
                    return PageNotValid(PageId);
                }
                if (!_authService.UserPermCheck(PageId, 3))
                {
                    return PermissionDenied("Update Counters");
                }
                var counter = _context.InvMaCounters.Where(i => i.Id == counterDto.ID).Select(i => i.Id).SingleOrDefault();
                if (counter == 0)
                {
                    return CommonResponse.NotFound("Id is not found!");
                }
                string criteria = "Update";
                var data = _context.Database.ExecuteSqlRaw("EXEC MaCountersSP @Criteria ={0},@MachineName={1}, @CounterCode={2}, @CounterName={3}, @MachineIP={4}, @Active={5},@Id={6} ", criteria,
                            counterDto.MachineName, counterDto.CounterName, counterDto.CounterCode, counterDto.MachineIP, counterDto.Active, counterDto.ID);
                _logger.LogInformation("Counter Updated successfully");
                return CommonResponse.Ok("Counter Updated successfully");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }
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

        public CommonResponse DeleteCounter(int Id,int PageId) 
        {
            try
            {
                if (!_authService.IsPageValid(PageId))
                {
                    return PageNotValid(PageId);
                }
                if (!_authService.UserPermCheck(PageId, 5))
                {
                    return PermissionDenied("Delete Counters");
                }
                var counter = _context.InvMaCounters.Where(i => i.Id == Id).Select(i => i.Id).SingleOrDefault();
                if (counter == 0)
                {
                    return CommonResponse.NotFound("Id is not found!");
                }
                string criteria = "Delete";
                var data = _context.Database.ExecuteSqlRaw("EXEC MaCountersSP @Criteria ={0},@Id={1}", criteria, Id);
                _logger.LogInformation("Counter deleted");
                return CommonResponse.Ok("Delete successfully!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return CommonResponse.Error(ex);
            }

        }




    }
}
