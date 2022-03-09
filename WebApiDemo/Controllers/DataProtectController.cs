using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiDemo.Controllers
{
    [Route("api/[controller]")]
    public class DataProtectController : MyBaseController
    {
        //IDataProtectionProvider _dataProtectionProvider;
        IDataProtector _dataProtector;
        public DataProtectController(IDataProtectionProvider dataProtectionProvider)
        {
            //_dataProtectionProvider = dataProtectionProvider;

            _dataProtector = dataProtectionProvider.CreateProtector("webapidemo2020");
        }

        [HttpPost("secure")]
        public string Protect(string input)
        {
            return _dataProtector.Protect(input);
        }

        [HttpPost("clear")]
        public string Unprotect(string encrypted)
        {
            return _dataProtector.Unprotect(encrypted);
        }
    }
}
