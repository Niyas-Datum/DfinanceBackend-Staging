using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfinance.Application.Services.Interface
{
    public interface IEncryptService
    {
        string Encrypt(string textToBeEncrypted);
    }
}
