using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Redbeard_Cues.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Redbeard_Cues.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyContext _context;

        public HomeController(MyContext context)
        {
            _context = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            // string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            string userName = User.Identity.Name;
            return View();
        }

        [HttpGet("products")]
        public IActionResult Products()
        {
            return View("Products");
        }

        [HttpGet("services")]
        public IActionResult Services()
        {
            return View("Services");
        }

        [HttpGet("signin")]
        public IActionResult Signin()
        {
            return View("Signin");
        }

        [HttpGet("about")]
        public IActionResult About()
        {
            return View("About");
        }

        [HttpGet("registration")]
        public IActionResult Registration()
        {
            return View("Registration");
        }

        [HttpPost("register")]
        public IActionResult Register(User NewUser)
        {
            if(ModelState.IsValid)
            {
                if(_context.Users.Any(u => u.Email == NewUser.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("index");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                NewUser.Password = Hasher.HashPassword(NewUser, NewUser.Password);
                _context.Users.Add(NewUser);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("uuid", NewUser.UserId);
                return RedirectToAction("Index");
            }
            return View("index");
        }

        [HttpPost("newlogin")]
        public IActionResult Login(LoginUser userSubmission)
        {
            if(ModelState.IsValid)
            {
                var userInDb = _context.Users.FirstOrDefault(u => u.Email == userSubmission.Email);
                if(userInDb == null)
                {
                    ModelState.AddModelError("Email", "Invalid Email/Password");;
                    return View("Signin");
                }
                var hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.Password);
                if(result == 0)
                {
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("Signin");
                }
                HttpContext.Session.SetInt32("uuid", userInDb.UserId);
                return RedirectToAction("Index");
            }
            return View("index");
        }

        [HttpGet("logout")]
        public IActionResult Logout(User user)
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
