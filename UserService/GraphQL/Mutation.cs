using HotChocolate.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Model.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace UserService.GraphQL
{
    public class Mutation
    {
        //===========================REGISTER USER AS BUYER FOR DEFAULT===================================//
        public async Task<UserData> RegisterUserAsync(
        RegisterUser input,
        [Service] TravikaContext context)
        {
            var user = context.Users.Where(o => o.Username == input.Username).FirstOrDefault();
            if (user != null)
            {
                return await Task.FromResult(new UserData());
            }
            var newUser = new User
            {
                Email = input.Email,
                Username = input.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(input.Password) // encrypt password
            };
            var memberRole = context.Roles.Where(m => m.Role1 == "CUSTOMER").FirstOrDefault();
            if (memberRole == null)
                throw new Exception("Invalid Role");
            var userRole = new UserRole
            {
                RoleId = memberRole.Id,
                UserId = newUser.Id
            };
            newUser.UserRoles.Add(userRole);
            // EF
            var ret = context.Users.Add(newUser);
            await context.SaveChangesAsync();

            return await Task.FromResult(new UserData
            {
                Id = newUser.Id,
                Username = newUser.Username,
                Email = newUser.Email,
            });
        }

        public async Task<UserToken> LoginAsync(
            LoginUser input,
            [Service] IOptions<TokenSettings> tokenSettings, // setting token
            [Service] TravikaContext context) // EF
        {
            var user = context.Users.Where(o => o.Username == input.Username).FirstOrDefault();
            if (user == null)
            {
                return await Task.FromResult(new UserToken(null, null, "Username or password was invalid"));
            }
            bool valid = BCrypt.Net.BCrypt.Verify(input.Password, user.Password);
            if (valid)
            {
                // generate jwt token
                var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSettings.Value.Key));
                var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

                // jwt payload
                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, user.Username));

                var userRoles = context.UserRoles.Where(o => o.Id == user.Id).ToList();
                foreach (var userRole in userRoles)
                {
                    var role = context.Roles.Where(o => o.Id == userRole.RoleId).FirstOrDefault();
                    if (role != null)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role.Role1));
                    }
                }

                var expired = DateTime.Now.AddHours(3);
                var jwtToken = new JwtSecurityToken(
                    issuer: tokenSettings.Value.Issuer,
                    audience: tokenSettings.Value.Audience,
                    expires: expired,
                    claims: claims, // jwt payload
                    signingCredentials: credentials // signature
                );

                return await Task.FromResult(
                    new UserToken(new JwtSecurityTokenHandler().WriteToken(jwtToken),
                    expired.ToString(), null));
                //return new JwtSecurityTokenHandler().WriteToken(jwtToken);
            }

            return await Task.FromResult(new UserToken(null, null, Message: "Username or password was invalid"));
        }

        [Authorize(Roles = new[] { "MANAGER" })]
        public async Task<User> UpdateUserAsync(
            UserInput input,
            [Service] TravikaContext context)
        {
            var user = context.Users.Where(o => o.Id == input.Id).FirstOrDefault();
            if (user != null)
            {
                user.Username = input.Username;
                user.Email = input.Email;
                user.Password = BCrypt.Net.BCrypt.HashPassword(input.Password);

                context.Users.Update(user);
                await context.SaveChangesAsync();
            }

            return await Task.FromResult(user);
        }

        [Authorize(Roles = new[] { "MANAGER" })]
        public async Task<User> DeleteUserByIdAsync(
            int id,
            [Service] TravikaContext context)
        {
            var user = context.Users.Where(o => o.Id == id).FirstOrDefault();
            if (user != null)
            {
                context.Users.Remove(user);
                await context.SaveChangesAsync();
            }
            return await Task.FromResult(user);
        }

        //========================================ADD CUSTOMER PROFILE BY USER==========================================// 

        [Authorize]
        public async Task<CustomerProfile> AddCustomerProfileAsync(
           CustomerProfileInput input,
           [Service] TravikaContext context, ClaimsPrincipal claimsPrincipal)
        {
            var userName = claimsPrincipal.Identity.Name;
            var user = context.Users.Where(o => o.Username == userName).FirstOrDefault();
            var profileCustomer = context.CustomerProfiles.Where(o => o.UserId == user.Id).FirstOrDefault();

            if (profileCustomer != null) return new CustomerProfile { Name = "Customer Profil sudah tersedia" };
            if (user == null) return new CustomerProfile();

            var customerProfile = new CustomerProfile
            {
                UserId = user.Id,
                Name = input.Name,
                Address = input.Address,
                City = input.City,
                Phone = input.Phone,
            };
            var ret = context.CustomerProfiles.Add(customerProfile);
            await context.SaveChangesAsync();

            return ret.Entity;
        }

        //========================================ADD MERCHANT PROFILE BY USER==========================================//        

        [Authorize]
        public async Task<MerchantProfile> AddMerchantProfileAsync(
           MerchantProfileInput input,
           [Service] TravikaContext context, ClaimsPrincipal claimsPrincipal)
        {
            var userName = claimsPrincipal.Identity.Name;
            var user = context.Users.Where(o => o.Username == userName).FirstOrDefault();
            var profileMerchant = context.MerchantProfiles.Where(o => o.UserId == user.Id).FirstOrDefault();

            if (profileMerchant != null) return new MerchantProfile { CompanyName = "Merchant profile sudah tersedia" };
            if (user == null) return new MerchantProfile();

            var merchantProfile = new MerchantProfile
            {
                UserId = user.Id,
                CompanyName = input.CompanyName
            };
            var ret = context.MerchantProfiles.Add(merchantProfile);
            await context.SaveChangesAsync();

            return ret.Entity;
        }

        //========================================UPDATE USER ROLE BY MANAGER=====================================//
        [Authorize(Roles = new[] { "MANAGER" })]
        public async Task<UserRole> UpdateUserRoleAsync(
            UserRoleInput input,
            [Service] TravikaContext context)
        {
            var userRole = context.UserRoles.Where(o => o.Id == input.Id).FirstOrDefault();
            if (userRole != null)
            {
                userRole.UserId = input.UserId;
                userRole.RoleId = input.RoleId;

                context.UserRoles.Update(userRole);
                await context.SaveChangesAsync();
            }

            return await Task.FromResult(userRole);
        }

        //===================================CHANGE PASSWORD BY USER===========================================//
        [Authorize]
        public async Task<UserData> ChangePasswordAsync(
            ChangePassword input,
            [Service] TravikaContext context,
            ClaimsPrincipal claimsPrincipal)
        {
            var userToken = claimsPrincipal.Identity;
            var user = context.Users.Where(u => u.Username == userToken.Name).FirstOrDefault();
            if (user != null)
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(input.Password);
                context.Users.Update(user);
                await context.SaveChangesAsync();
            }
            return await Task.FromResult(new UserData
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
            });
        }
    }
}
