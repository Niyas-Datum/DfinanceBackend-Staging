using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Shared.Configuration.Service
{
   
    public interface IConnectionServices
    {

         Task<bool> Setcon(string con);

         string getcon();
         void setconkey(string key);

         bool RemoveConnection();

    }
    public class ConnectionServices : IConnectionServices
    {
        public static Dictionary<string, string>  connecionSTR =new Dictionary<string, string>();
        public  string secretAuthKey;

        public ConnectionServices()
        {
         
        }

        public Task<bool> Setcon(string con )
        {
            if (secretAuthKey != null && !connecionSTR.ContainsKey(secretAuthKey))
                connecionSTR.Add(secretAuthKey, con);
            return Task.Run(()=>true);
        }
        public string getcon()
        {
            if (secretAuthKey == null  || !connecionSTR.ContainsKey(secretAuthKey)) return null; 
            
            return connecionSTR[secretAuthKey];
        }

        public bool RemoveConnection()
        {
            return connecionSTR.Remove(secretAuthKey);
        }

        public void setconkey(string key)
        {
            secretAuthKey =  key;
        }
    }
}
