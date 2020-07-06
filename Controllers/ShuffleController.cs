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
        private UserManager<User> userManager;

        public ShuffleController(ShuffleDbContext db, UserManager<User> usrMgr)
        {
            _db = db;
            userManager = usrMgr;
        }

   
        public IActionResult Index()
        {
            var input = _db.user.Find();

            return View(input);
        }


        [HttpPost]
        public ActionResult Create(User newuser)
        {
            if (ModelState.IsValid)
            {

                var input = new User
                {
                    Id = newuser.Id,
                    Name = newuser.Name,
                    Surname = newuser.Surname,
                    Email = newuser.Email
                 
                };

                _db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return View("create");
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

        public void shuffleId()
        {
            var rnd = new Random();
            
        }

        protected void start(object sender, EventArgs e)

        {
            shuffleId();
        }
    }

}
