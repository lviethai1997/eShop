using eShop.ViewModels.Catalog.Common;
using eShop.ViewModels.System.Users;
using System;
using System.Threading.Tasks;

namespace eShop.Application.System.Users
{
    public interface IUserService
    {
        Task<ApiResult<string>> Authencate(LoginRequest request);

        Task<ApiResult<bool>> Register(RegisterRequest request);

        Task<ApiResult<PagedResult<UserViewModel>>> GetUserPaging(UserPagingRequest request);

        Task<ApiResult<bool>> UpdateUser(Guid id,UserUpdateRequest user);

        Task<ApiResult<UserUpdateRequest>> GetUserById(string id);

        Task<ApiResult<bool>> DeleteUser(string id);
    }
}