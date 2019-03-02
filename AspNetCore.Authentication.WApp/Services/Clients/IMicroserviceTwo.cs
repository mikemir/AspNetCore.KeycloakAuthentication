using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.Authentication.WApp.Services.Clients
{
    public interface IMicroserviceTwo
    {
        [Get("/values")]
        Task<IEnumerable<string>> GetValues();
    }
}
