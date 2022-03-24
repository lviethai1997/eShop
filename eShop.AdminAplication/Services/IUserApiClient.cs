using eShop.ViewModels.Catalog.Common;
using eShop.ViewModels.System.Users;
using System.Threading.Tasks;

namespace eShop.AdminAplication.Services
{
    public interface IUserApiClient
    {
        Task<string> Authenticate(LoginRequest request);

        Task<PagedResult<UserViewModel>> GetUserPaging(UserPagingRequest request);

        Task<bool> CreateUser(RegisterRequest request);

        Task<bool> UpdateUser(UserUpdateRequest request);

        Task<UserUpdateRequest> GetUserById(string id);

        Task<bool> DeleteUser(string id);
    }
}