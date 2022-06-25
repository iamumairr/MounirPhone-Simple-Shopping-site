using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MounirPhone.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;

        public UserController(UserManager<IdentityUser> _userManager)
        {
            userManager = _userManager;
        }
        // GET: UserController
        public ActionResult Index() //display all users other than Admin
        {
            var usersWithPermission = userManager.GetUsersInRoleAsync(WC.AdminRole).Result; //get Admin Users

            var idsWithPermission = usersWithPermission.Select(u => u.Id); //selecting Id from users data

            var users = userManager.Users.Where(a => !idsWithPermission.Contains(a.Id)).ToList();//getting data other than Admin
            
            return View(users);
        }
        public async Task<ActionResult> Delete(string Id)
        {
            var user = await userManager.FindByIdAsync(Id);
            if (user != null)
            {
                await userManager.DeleteAsync(user);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
