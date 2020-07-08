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
                var skip = userList.SkipWhile(x => x.Id == item.Id).Where(x => x.Id == user.Id);
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

        protected void sendEmail(object sender, EventArgs e)
        {
            {

                MailMessage message = new MailMessage();
                message.Subject = "Email Subject ";
                message.Body = "Email Message";
                message.From = new MailAddress("MyEmail@mail.com");

                var fromAddress = "MyEmail@mail.com";
                const string fromPassword = "password";

                var smtp = new System.Net.Mail.SmtpClient();
                {
                    smtp.Host = "smtp.mail.com";
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    smtp.Credentials = new NetworkCredential(fromAddress, fromPassword);
                    smtp.Timeout = 20000;
                }
                SqlCommand cmd = null;
                string connectionString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;
                string queryString = @"SELECT Email FROM User WHERE Email = Email";

                using (SqlConnection connection =
                           new SqlConnection(connectionString))
                {
                    SqlCommand command =
                        new SqlCommand(queryString, connection);
                    connection.Open();
                    cmd = new SqlCommand(queryString);
                    cmd.Connection = connection;

                    SqlDataReader reader = cmd.ExecuteReader();

                    
                    while (reader.Read())
                    {

                        var to = new MailAddress(reader["Email"].ToString());
                        message.To.Add(to);

                    }


                    smtp.Send(message);
                    reader.Close();
                }
            }
        }

    }

}
