using Dfinance.Application.Services.General.Interface;
using Dfinance.Core.Infrastructure;
using Dfinance.Core.Migrations;
using Dfinance.Core.Views.Common;
using Dfinance.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;

namespace Dfinance.Application.Services.General
{

    public class MiscellaneousService : IMiscellaneousService
    {
        private readonly DFCoreContext _context;
        public MiscellaneousService(DFCoreContext dFCoreContext)
        {

            _context = dFCoreContext;
        }

        public CommonResponse GetPopup(string[] keys)
        {           
            List<List<ReadViewVal>> data=new List<List<ReadViewVal>>();            
            foreach (var k in keys)
            {
                data.Add(FillPopup(k));                
            }
            return CommonResponse.Ok(data);
        }
       private List<ReadViewVal> FillPopup(string key)//input=>Item color,country,Item brand
        {
            return _context.MaMisc
                   .Where(m => m.Key == key)
                   .Select(m => new ReadViewVal
                   {
                       ID = m.Id,
                       Code = m.Code,
                       Value = m.Value
                   }).ToList();
        }       

        ////called by MiscellaneousController/FillDropDown
        ////dropdown for quality,country
        public CommonResponse GetDropDown(string[] keys)
        {
            List<List<DropDownViewValue>> data=new List<List<DropDownViewValue>>();
            foreach(var k in keys)
            {
                data.Add(FillDropDown(k)); 
            }
            return CommonResponse.Ok(data);
        }
        //input=>"Country" for country dropdown
        //"ProjectStatus" for status dropdown
        //"CreditnoteType" =>Page:CreditNote,Dropdown=>Particualrs

        public List<DropDownViewValue> FillDropDown(string key)
        {           
                 if(key== "Quality")
                return _context.DropDownViewValue.FromSqlRaw("Exec DropDownListSP @Criteria = 'FillQuality'").ToList();         
           
            return _context.DropDownViewValue.FromSqlRaw($"Exec DropDownListSP @Criteria = 'FillMaMisc',@StrParam='{key}'").ToList();
            //    switch (key)
            //    {
            //        case key:
            //            {
            //                return _context.DropDownViewValue.FromSqlRaw("Exec DropDownListSP @Criteria = 'FillMaMisc',@StrParam='Country'").ToList();
            //                break;
            //            }
            //        case "Quality":
            //            {
            //                return _context.DropDownViewValue.FromSqlRaw("Exec DropDownListSP @Criteria = 'FillQuality'").ToList();
            //                break;
            //            }
            //        case "Status":
            //            {
            //                return _context.DropDownViewValue.FromSqlRaw($"Exec DropDownListSP @Criteria ='FillMaMisc',@StrParam='ProjectStatus'").ToList();
            //                break;
            //            }
            //    default:
            //            {
            //                return null;
            //            }
            //}        
			
	   }
        
       
    }
}



