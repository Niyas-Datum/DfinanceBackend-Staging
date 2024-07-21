using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Shared.Configuration.Service
{
   
    public interface IConnectionServices
    {

        public Task<bool> Setcon(string con);

        public string getcon();
       
    }
    public class ConnectionServices : IConnectionServices
    {
        public string connecionSTR;
        public Task<bool> Setcon(string con)
        {
            connecionSTR = con;
            return Task.Run(()=>true);
        }
        public string getcon()
        {
            return connecionSTR;
        }

    }
}
