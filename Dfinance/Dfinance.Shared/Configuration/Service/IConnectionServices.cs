using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Shared.Configuration.Service
{
   
    public interface IConnectionServices
    {

        public void Setcon(string con);

        public string getcon();
       
    }
    public class ConnectionServices : IConnectionServices
    {
        public string connecionSTR;
        public void Setcon(string con)
        {
            connecionSTR = con;
        }
        public string getcon()
        {
            return connecionSTR;
        }

    }
}
