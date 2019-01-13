using System.Linq;
using System.Security.Claims;

namespace RecrutimentApp.Utilities
{
    public static class UserInformations
    {
        private const string GivenNameType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname";
        private const string SurnameType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname";
        private const string EmailType = "emails";

        public static string GetUserGivenName(ClaimsPrincipal user)
        {
            return user?.Claims?.Where(c => c.Type == GivenNameType).FirstOrDefault()?.Value;
        }

        public static string GetUserSurname(ClaimsPrincipal user)
        {
            return user?.Claims?.Where(c => c.Type == SurnameType).FirstOrDefault()?.Value;
        }

        public static string GetUserEmail(ClaimsPrincipal user)
        {
            return user?.Claims?.Where(c => c.Type == EmailType).FirstOrDefault()?.Value;
        }
    }
}
