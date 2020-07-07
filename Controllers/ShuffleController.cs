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
using System.Linq;
using Microsoft.Extensions.Logging;

namespace shuffle2.Controllers
{
    [Route("[controller]/[action]")]
    public class ShuffleController: Controller
    {
       
        private readonly ShuffleDbContext _db;

        public ShuffleController(ShuffleDbContext db)
        {
            _db = db;
           
        }

        [Route("Shuffle/Index")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Index(int id)
        {
            var user = await _db.users.FindAsync(id);
            return View();
        }

        [HttpGet]
        public Task<List<User>> GetAllUsers()
        {     
            return _db.users.OrderByDescending(s => s.Id).ToListAsync();  
        }


        [Route("Shuffle/Create")]
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

            var user = await _db.users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [Route("Shuffle/Edit")]
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

            var user = await _db.users
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
        
            var user2 = @"SELECT Name
            FROM User
            ORDER BY NEWID()";

        }

        protected void start(object sender, EventArgs e)

        {
            shuffleId();
        }
    }

}
