using eShop.Data.Entities;
using eShop.ViewModels.Catalog.Common;
using eShop.ViewModels.System.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Application.System.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _config;

        //UserManager &  SignInManager là thư viên của entity
        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _config = config;
        }

        public async Task<string> Authencate(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                return null;
            }

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true);

            if (result.Succeeded)
            {
                var roles = _userManager.GetRolesAsync(user);

                var claims = new[]
                {
                    new Claim(ClaimTypes.Email,user.Email),
                    new Claim(ClaimTypes.Name,user.UserName),
                    new Claim(ClaimTypes.GivenName,user.FirstName),
                    new Claim(ClaimTypes.Role,String.Join(";", roles)),
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                    _config["Tokens:Issuer"],
                    claims,
                    expires: DateTime.Now.AddHours(3),
                    signingCredentials: creds);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> DeleteUser(string id)
        {
            var DeleteUser = await _userManager.DeleteAsync(await _userManager.FindByIdAsync(id));
            if (DeleteUser.Succeeded)
                return true;
            return false;
        }

        public async Task<UserUpdateRequest> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            var UserViewModel = new UserUpdateRequest()
            {
                Username = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                DoB = user.Dob
            };

            return UserViewModel;
        }

        public async Task<PagedResult<UserViewModel>> GetUserPaging(UserPagingRequest request)
        {
            var query = _userManager.Users;

            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.UserName.Contains(request.Keyword) || x.PhoneNumber.Contains(request.Keyword));
            }

            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).Select(x => new UserViewModel()
            {
                Email = x.Email,
                Phone = x.PhoneNumber,
                UserName = x.UserName,
                FirstName = x.FirstName,
                Id = x.Id,
                LastName = x.LastName
            }).ToListAsync();

            var pagedResult = new PagedResult<UserViewModel>()
            {
                TotalRecord = totalRow,
                Items = data
            };
            return pagedResult;
        }

        public async Task<bool> Register(RegisterRequest request)
        {
            var user = new AppUser()
            {
                Dob = request.DoB,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.UserName,
                PhoneNumber = request.Phone
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> UpdateUser(UserUpdateRequest request)
        {
            var findUser = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == request.Username);

            if (findUser == null)
            {
                return false;
            }

            findUser.FirstName = request.FirstName;
            findUser.LastName = request.LastName;
            findUser.Email = request.Email;
            findUser.Dob = request.DoB;
            findUser.UserName = request.Username;
            findUser.PhoneNumber = request.Phone;

            var appUser = await _userManager.FindByIdAsync(findUser.Id.ToString());

            if (!string.IsNullOrEmpty(request.NewPassword) && !string.IsNullOrEmpty(request.OldPassword))
            {
                var rs = await _userManager.ChangePasswordAsync(appUser, request.OldPassword, request.NewPassword);
                if (rs.Succeeded)
                {
                    var result = await _userManager.UpdateAsync(findUser);
                    if (result.Succeeded)
                        return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }
    }
}