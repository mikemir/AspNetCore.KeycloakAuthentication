using AspNetCore.Authentication.WApp.Services.Clients;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.Authentication.WApp.Services
{
    public class TestService : ITestService
    {
        private readonly IMicroserviceTwo _msTwo;
        private readonly IMicroserviceThree _msThree;

        public TestService(IMicroserviceTwo msTwo, IMicroserviceThree msThree)
        {
            _msTwo = msTwo;
            _msThree = msThree;
        }

        public async Task<IEnumerable<string>> GetValues()
        {
            var result = new List<string>();

            try
            {
                result.AddRange(await _msTwo.GetValues());
                result.AddRange(await _msThree.GetValues());
            }
            catch (ApiException ex)
            {
                result = new List<string> { $"{ex.Headers.WwwAuthenticate}, {ex.Message}" };
            }

            return result;
        }
    }
}
