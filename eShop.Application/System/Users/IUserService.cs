using eShop.ViewModels.Catalog.Common;
using eShop.ViewModels.System.Users;
using System.Threading.Tasks;

namespace eShop.Application.System.Users
{
    public interface IUserService
    {
        Task<string> Authencate(LoginRequest request);

        Task<bool> Register(RegisterRequest request);

        Task<PagedResult<UserViewModel>> GetUserPaging(UserPagingRequest request);

        Task<bool> UpdateUser(UserUpdateRequest user);

        Task<UserUpdateRequest> GetUserById(string id);

        Task<bool> DeleteUser(string id);
    }
}