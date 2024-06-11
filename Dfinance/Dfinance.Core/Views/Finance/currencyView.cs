using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Core.Views.Finance
{
    public class FillcurrencyCode
    {
        public int ID {  get; set; }
        public string? Code { get; set; }
    }
    public class FillCurrencyCodeById
    {
       public int Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }

    }
    public class FillCurrency
    {
        public int CurrencyID { get; set; }
        public string? Currency { get; set; }
        public string? Abbreviation { get; set; }
        //public string CurrencyRate { get; set; }
        //public int Precision { get; set; }
        //public string Culture { get; set; }
        //public string FormatString { get; set; }

    }
    public class FillCurrencyById
    {
        public int CurrencyID { get; set; }
        public string? Currency { get; set; }
        public string? Abbreviation { get; set; }
        public bool? DefaultCurrency { get; set; }
        public double? CurrencyRate { get; set; }
        public int? CreatedBy { get; set; }      
        public DateTime? CreatedOn { get; set; }
        public byte? ActiveFlag {get;set;}
        public byte? Precision { get;set;}
        public string? Culture { get; set;}
        public string? Coin { get; set; }
        public string? FormatString { get; set; }
        public string? Symbol {  get; set; }
    }
}
