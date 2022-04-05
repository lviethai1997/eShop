using eShop.AdminAplication.Services;
using eShop.ViewModels.Catalog.Common;
using eShop.ViewModels.System.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace eShop.AdminAplication.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;
        private readonly IRoleApiClient _roleApiClient;

        public UserController(IUserApiClient userApiClient, IConfiguration configuration, IRoleApiClient roleApiClient)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
            _roleApiClient = roleApiClient;
        }

        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10, string keyword = null)
        {
            var request = new UserPagingRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            var data = await _userApiClient.GetUserPaging(request);
            ViewBag.keyword = keyword;

            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }

            return View(data.ResultObject);
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

            if (result.IsSuccessed)
            {
                TempData["result"] = "Success";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);

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

            return View(user.ResultObject);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = await _userApiClient.UpdateUser(id, request);

            if (result.IsSuccessed)
            {
                TempData["result"] = "Success";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Message);
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _userApiClient.DeleteUser(id);
            if (!result.IsSuccessed) { ModelState.AddModelError("", result.Message); return View(); }
            TempData["result"] = "Success";
            return RedirectToAction("Index", "User");
        }

        [HttpGet]
        public async Task<IActionResult> RoleAssign(Guid id)
        {
            var roleAssignRequest = await GetRoleAssignRequest(id);

            return View(roleAssignRequest);
        }

        [HttpPost]
        public async Task<IActionResult> RoleAssign(RoleAssignRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = await _userApiClient.GetUserById(request.id.ToString());

            if (result.IsSuccessed)
            {
                var setRole = await _userApiClient.RoleAssign(request.id, request);
                if (setRole.IsSuccessed)
                {
                    TempData["result"] = "Set role Successful";
                    return RedirectToAction("Index");
                }
                return View(setRole.Message);
            }

            ModelState.AddModelError("", result.Message);
            var roleAssignRequest = await GetRoleAssignRequest(request.id);

            return View(roleAssignRequest);
        }

        private async Task<RoleAssignRequest> GetRoleAssignRequest(Guid id)
        {
            var result = await _userApiClient.GetUserById(id.ToString());
            var roles = await _roleApiClient.GetAll();

            var roleAssignRequest = new RoleAssignRequest();

            foreach (var item in roles.ResultObject)
            {
                roleAssignRequest.Roles.Add(new SelectItem()
                {
                    Id = item.Id.ToString(),
                    Name = item.Name,
                    Selected = result.ResultObject.Roles.Contains(item.Name)
                });
            }

            return roleAssignRequest;
        }
    }
}