﻿using eShop.Data.Entities;
using eShop.ViewModels.System.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShop.Application.System.Roles
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;

        public RoleService(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<List<RolesViewModel>> GetAll()
        {
            var roles = await _roleManager.Roles.Select(x => new RolesViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,

            }).ToListAsync();

            return roles;
        }
    }
}