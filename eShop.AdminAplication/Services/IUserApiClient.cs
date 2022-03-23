using eShop.ViewModels.System.Users;
using System.Threading.Tasks;

namespace eShop.AdminAplication.Services
{
    public interface IUserApiClient
    {
        Task<string> Authenticate(LoginRequest request);
    }
}
