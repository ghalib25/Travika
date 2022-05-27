using HotChocolate.AspNetCore.Authorization;
using Model.Model;
using System.Security.Claims;

namespace UserService.GraphQL
{
    public class Query
    {
        [Authorize(Roles = new[] { "MANAGER" })]
        public IQueryable<UserData> GetUsers(
         [Service] TravikaContext context) =>
         context.Users.Select(p => new UserData()
         {
             Id = p.Id,
             Email = p.Email,
             Username = p.Username
         });

        //-------------------------------------VIEW ALL USER ROLE BY ADMIN---------------------------------------//
        [Authorize(Roles = new[] { "MANAGER" })]
        public IQueryable<UserRole> GetUserRoles(
            [Service] TravikaContext context) =>
            context.UserRoles.Select(p => new UserRole()
            {
                Id = p.Id,
                UserId = p.UserId,
                RoleId = p.RoleId
            });

        //-------------------------------------VIEW CUSTOMER PROFILE BY TOKEN--------------------------------------------//
        [Authorize]
        public IQueryable<CustomerProfile> GetCustomerProfilesbyToken(
            [Service] TravikaContext context,
            ClaimsPrincipal claimsPrincipal)
        {
            var username = claimsPrincipal.Identity.Name;
            var user = context.Users.Where(o => o.Username == username).FirstOrDefault();
            if (user != null)
            {
                var profiles = context.CustomerProfiles.Where(o => o.UserId == user.Id);
                return profiles.AsQueryable();
            }
            return new List<CustomerProfile>().AsQueryable();
        }

        //-------------------------------------VIEW MERCHANT PROFILE BY TOKEN--------------------------------------------//
        [Authorize]
        public IQueryable<MerchantProfile> GetMerchantProfilesbyToken(
            [Service] TravikaContext context,
            ClaimsPrincipal claimsPrincipal)
        {
            var username = claimsPrincipal.Identity.Name;
            var user = context.Users.Where(o => o.Username == username).FirstOrDefault();
            if (user != null)
            {
                var profiles = context.MerchantProfiles.Where(o => o.UserId == user.Id);
                return profiles.AsQueryable();
            }
            return new List<MerchantProfile>().AsQueryable();
        }

        //-------------------------------------VIEW ALL MERCHANT BY MANAGER--------------------------------------------//
        [Authorize(Roles = new[] { "MANAGER" })]
        public IQueryable<User> GetViewMerchants(
            [Service] TravikaContext context)
        {
            var userRole = context.Roles.Where(k => k.Role1 == "MERCHANT").FirstOrDefault();
            var merchants = context.Users.Where(k => k.UserRoles.Any(o => o.RoleId == userRole.Id));
            return merchants.AsQueryable();
        }

        //-------------------------------------VIEW ALL CUSTOMER BY MANAGER--------------------------------------------//
        [Authorize(Roles = new[] { "MANAGER" })]
        public IQueryable<User> GetViewCustomers(
            [Service] TravikaContext context)
        {
            var userRole = context.Roles.Where(k => k.Role1 == "CUSTOMER").FirstOrDefault();
            var cistomers = context.Users.Where(k => k.UserRoles.Any(o => o.RoleId == userRole.Id));
            return cistomers.AsQueryable();
        }
    }
}
