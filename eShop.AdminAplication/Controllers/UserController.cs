﻿using eShop.AdminAplication.Services;
using eShop.ViewModels.System.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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

        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10, string keyword = null)
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
    }
}