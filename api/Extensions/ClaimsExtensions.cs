using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using K4os.Hash.xxHash;

namespace api.Extensions
{
    public static class ClaimsExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            // Not sure why this is not working
            // user.Claims.SingleOrDefault(x => x.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname")) returns null
            // return user.Claims.SingleOrDefault(x => x.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname")).Value;
            return "Invinvinv111";      // Hard coded for testing
        }
    }
}