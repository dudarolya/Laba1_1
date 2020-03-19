using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laba1.Models;
using Laba1.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Laba1.Controllers
{
    public class RolesController : Controller
    {
        //private readonly IdentityContext _context;
        RoleManager<IdentityRole> _roleManager;
        UserManager<User> _userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index() => View(_roleManager.Roles.ToList());
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id", "Name")] IdentityRole identityRole)
        {
            if (ModelState.IsValid)
            {
                identityRole.NormalizedName = identityRole.Name.ToUpper();
                await _roleManager.CreateAsync(identityRole);
                return RedirectToAction("Index", "Roles", new { id = identityRole.Id, name = identityRole.Name});
            }
                return View();
        }
        public IActionResult UserList() => View(_userManager.Users.ToList());

        public async Task<IActionResult> Edit(string userId)
        {
            // get a user
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // list of user's roles
                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();
                ChangeRoleViewModel model = new ChangeRoleViewModel
                {
                    UserId = user.Id,
                    UserEmail = user.Email,
                    UserRoles = userRoles,
                    AllRoles = allRoles
                };
                return View(model);

            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(string userId, List<string> roles)
        {
            // get a user
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // list of user's roles
                var userRoles = await _userManager.GetRolesAsync(user);
                // get all roles
                var allRoles = _roleManager.Roles.ToList();
                // list of added roles
                var addedRoles = roles.Except(userRoles);
                // list of deleted roles
                var removedRoles = userRoles.Except(roles);
                await _userManager.AddToRolesAsync(user, addedRoles);
                await _userManager.RemoveFromRolesAsync(user, removedRoles);
                return RedirectToAction("UserList");
            }
            return NotFound();
        }

        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        // POST: Artists/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            await _roleManager.DeleteAsync(role);
            return RedirectToAction("Index", "Roles");
        }
    }
}