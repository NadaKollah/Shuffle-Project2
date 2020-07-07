using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using shuffle2.Entity;
using System;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using shuffle2.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using shuffle2.Models;

namespace shuffle2.Controllers
{
    public class ShuffleController: Controller
    {
        private readonly ShuffleDbContext _db;

        public ShuffleController(ShuffleDbContext db)
        {
            _db = db;
           
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Index(int id)
        {
            var user = await _db.user.FindAsync(id);
            return View();
        }

        public async Task<UserModel> GetUser(int id)
        {
            var user = await _db.GetUser(id);

            var output = new UserModel
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email
            };

            return output;
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


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _db.user.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Surname,Email")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(user);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (user!= null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(user);
            
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _db.user
                .FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, [Bind("Id,Name,Surname,Email")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Remove(user);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (user != null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(user);

        }
       

        public void shuffleId()
        {
            IEnumerable<User> Query = (from user in User.()
                                        select new User()
                                        {
                                            Name = user.Name,
                                            Surname = user.Surname,
                                            Email = user.Email
                                        }).ToList(); 

            /*Random rnd = new Random();
            string[] MyRandomArray = MyArray.OrderBy(x => rnd.Next()).ToArray();
            var user = @"SELECT Name
                                FROM User
                                ORDER BY RAND()
                                LIMIT 1";

            var user2= @" SELECT TOP 1 Name FROM User
            ORDER BY NEWID()";

            var user2 = @"SELECT Name
            FROM User
            ORDER BY NEWID()";*/

            var countList1 = list.Where(x => x.NAVI_USER == Name && x.MONTH == monthList[1] && x.PPZN_ID == 0).Select(x => x.COUNT).FirstOrDefault();

        }

        protected void start(object sender, EventArgs e)

        {
            shuffleId();
        }
    }

}
