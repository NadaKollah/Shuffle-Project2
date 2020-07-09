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
using System.Net.Mail;
using System.Configuration;
using System.Net;

namespace shuffle2.Controllers
{
    public class ShuffleController : Controller
    {

        private readonly ShuffleDbContext _db;

        public ShuffleController(ShuffleDbContext db)
        {
            _db = db;
        }

        public ActionResult Index()
        {
            var userList = _db.users.ToList();
            var userListViewModel = new List<UserModel>();

            foreach (var item in userList)
            {
                var user = new UserModel()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Surname = item.Surname,
                    Email = item.Email
                };
                userListViewModel.Add(user);
            }

            return View(userListViewModel);
        }

        public ActionResult Create()
        {
            return View("create");
        }


        [HttpPost("/Shuffle/Create")]
        public ActionResult Create(UserModel newUser)
        {

            if (ModelState.IsValid)
            {
                var userEntity = new User()
                {
                    Name = newUser.Name,
                    Surname = newUser.Surname,
                    Email = newUser.Email
                };
                _db.Add(userEntity);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return View(newUser);
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
            var userModel = new UserModel()
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email
            };
            if (user == null)
            {
                return NotFound();
            }
            return View(userModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Surname,Email")] UserModel user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }
            var userEntity = _db.users.Find(id);

            userEntity.Name = user.Name;
            userEntity.Surname = user.Surname;
            userEntity.Email = user.Email;

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(userEntity);
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


        [HttpGet("/Shuffle/Delete/{id?}")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            User user = _db.users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            _db.users.Remove(user);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet("/Shuffle/shuffle")]
        public ActionResult Shuffle()
        {

            var userList = _db.users.ToList();

            var userListViewModel = userList.Select(x => new UserModel
            {
                Id = x.Id,
                Name = x.Name,
                Surname = x.Surname,
                Email = x.Email
            }).ToList();

            var shuffleModel = new ShuffelModel();
            var list = new List<NamesModel>();
            foreach (var item in userListViewModel)
            {
                Random random = new Random();
                int id1 = random.Next(userList.Count);
                var user = userList[id1];
                if (item.Name == user.Name) {
                    var nuser = _db.users.Skip(id1);
                }
                userList.Remove(user);

                var names = new NamesModel()
                {
                    Name1 = item.Name,
                    Name2 = user.Name

                };

                list.Add(names);
            }

            shuffleModel.names = list;
            return View(shuffleModel);
        }

        protected void sendEmail(string MailFrom, string MailTo, string MailSubject, string MailBody)
        {
            var emailList = _db.users.Select(x=>x.Email);
            foreach (string email in emailList) 
            {
                MailMessage message = new MailMessage();
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();

                message.Subject = "Name of user to be gifted";
                message.Body = "Add Email Body Part";
                message.From = new MailAddress("Valid Email Address");
                message.To.Add("Valid Email Address");
                message.IsBodyHtml = true;
                client.Host = "smtp.gmail.com";
                System.Net.NetworkCredential basicauthenticationinfo = new System.Net.NetworkCredential("Valid Email Address", "Password");
                client.Port = int.Parse("587");
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = basicauthenticationinfo;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Send(message);

                /*System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("Email@gmail.com", "xxxx");
                smtp.EnableSsl = true;

                MailMessage message = new MailMessage("Email@gmail.com", email);
                message.Subject = "Name of user to be gifted";
                message.Body = "Test2";

                smtp.Send(message);*/
            }
    
                }
            }
        }


