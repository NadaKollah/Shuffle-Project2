using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using shuffle2.Entity;
using System;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using shuffle2.Data;

namespace shuffle2.Controllers
{
    public class ShuffleController: Controller
    {
        private readonly ShuffleDbContext _db;

        public ShuffleController(ShuffleDbContext db)
        {
            _db = db;
        }

        private UserManager<User> userManager;

        public ShuffleController(UserManager<User> usrMgr)
        {
            userManager = usrMgr;
        }

        public IActionResult Index()
        {
            return View(userManager.Users);
        }


        [HttpPost]
        public ActionResult Create(User newUser)
        {
            if (ModelState.IsValid)
            {
                db.AddToUsers(newUser);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return View(newUser);
            }
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"This user with Id = {id} doesn't exist";
                return View("Shared.Error");
            }
            else
            {
                var result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View("edit");
            }
        }     

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"This user with Id = {id} doesn't exist";
                return View("Shared.Error");
            }
            else
            {
                var result = await userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View("index");
            }
        }


        protected void start(object sender, EventArgs e)

        {
        }
    }

}
