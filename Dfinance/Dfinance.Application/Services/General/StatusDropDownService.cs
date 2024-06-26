﻿using Dfinance.Application.Services.General.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Views.General;
using Dfinance.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Application.Services.General
{
    public class StatusDropDownService : IStatusDropDownService
    {
        private readonly DFCoreContext _contxt;
        public StatusDropDownService(DFCoreContext context)
        {
            _contxt = context;
        }
        public CommonResponse FillStatus()
        {
            try
            {
                string Criteria = "FillMaMisc";
                string StrParam = "ProjectStatus";
                var data = _contxt.DropDownViewValue.FromSqlRaw($"Exec DropDownListSP @Criteria ='{Criteria}',@StrParam='{StrParam}'").ToList();
                return CommonResponse.Ok(data);
            }
            catch (Exception ex)
            {
                return CommonResponse.Error(ex);
            }
        }
    }
}
