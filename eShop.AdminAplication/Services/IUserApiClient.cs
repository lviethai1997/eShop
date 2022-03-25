using eShop.ViewModels.Catalog.Common;
using eShop.ViewModels.System.Users;
using System;
using System.Threading.Tasks;

namespace eShop.AdminAplication.Services
{
    public interface IUserApiClient
    {
        Task<ApiResult<string>> Authenticate(LoginRequest request);

        Task<ApiResult<PagedResult<UserViewModel>>> GetUserPaging(UserPagingRequest request);

        Task<ApiResult<bool>> CreateUser(RegisterRequest request);

        Task<ApiResult<bool>> UpdateUser(Guid id,UserUpdateRequest request);

        Task<ApiResult<UserUpdateRequest>> GetUserById(string id);

        Task<ApiResult<bool>> DeleteUser(string id);
    }
}