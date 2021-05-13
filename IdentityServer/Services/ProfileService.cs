using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Extensions;
using IdentityServer.Data.Identity;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileService(UserManager<ApplicationUser> userManager, IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory)
        {
            _userManager = userManager;
            _claimsFactory = claimsFactory;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            var principal = await _claimsFactory.CreateAsync(user);
            List<string> roles = (List<string>)await _userManager.GetRolesAsync(user);

            var claims = principal.Claims.ToList();
            claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();            

            claims.Add(new Claim("userName", user.UserName ?? string.Empty));
            claims.Add(new Claim("surname", user.Surname ?? string.Empty));
            claims.Add(new Claim("email", user.Email ?? string.Empty));
            claims.Add(new Claim("phoneNumber", user.PhoneNumber ?? string.Empty));
            claims.Add(new Claim("creationDate", user.CreationDate.ToString("dd/MM/yyyy HH:mm:ss") ?? string.Empty));
            roles.ForEach(role => 
            {
                claims.Add(new Claim("role", role ?? string.Empty));
            });                     

            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}

