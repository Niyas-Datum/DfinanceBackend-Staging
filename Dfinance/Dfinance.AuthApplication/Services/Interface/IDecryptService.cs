using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.AuthApplication.Services.Interface
{
    public interface IDecryptService
    {
        string Decrypt(string TextToBeDecrypted);
    }
}
