using System.Security.Claims;

namespace SV22T1020283.Shop
{
    public static class ShopUserHelper
    {
        public static ShopUserDataModel? GetUserData(this ClaimsPrincipal principal)
        {
            try
            {
                if (principal == null || principal.Identity == null || !principal.Identity.IsAuthenticated)
                    return null;

                var userData = new ShopUserDataModel();

                userData.UserId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
                userData.UserName = principal.FindFirstValue(ClaimTypes.Name);
                userData.DisplayName = principal.FindFirstValue("DisplayName");
                userData.Email = principal.FindFirstValue(ClaimTypes.Email);

                return userData;
            }
            catch
            {
                return null;
            }
        }
    }

    public class ShopUserDataModel
    {
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? DisplayName { get; set; }
        public string? Email { get; set; }
    }
}