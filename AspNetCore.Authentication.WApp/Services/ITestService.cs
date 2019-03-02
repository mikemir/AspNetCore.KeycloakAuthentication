using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.Authentication.WApp.Services
{
    public interface ITestService
    {
        Task<IEnumerable<string>> GetValues();
    }
}
