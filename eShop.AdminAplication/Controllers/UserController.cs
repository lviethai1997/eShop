using eShop.AdminAplication.Services;
using eShop.ViewModels.System.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace eShop.AdminAplication.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserApiClient _userApiClient;

        public UserController(IUserApiClient userApiClient, IConfiguration configuration)
        {
            _userApiClient = userApiClient;
        }

        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 1, string keyword = null)
        {
            var session = HttpContext.Session.GetString("Token");
            var request = new UserPagingRequest()
            {
                BearerToken = session,
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            var data = await _userApiClient.GetUserPaging(request);

            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = await _userApiClient.CreateUser(request);

            if (result)
            {
                return RedirectToAction("Index");
            }

            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _userApiClient.GetUserById(id.ToString());

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userApiClient.GetUserById(id);

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = await _userApiClient.UpdateUser(request);

            if (result)
            {
                return RedirectToAction("Index");
            }

            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _userApiClient.DeleteUser(id);
            if (!result) { return View(); }
            return RedirectToAction("Index", "User");
        }
    }
}