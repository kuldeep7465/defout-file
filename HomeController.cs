using aspCoreDataFirstApproch.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace aspCoreDataFirstApproch.Controllers
{
    public class HomeController : Controller
    {
        private readonly EmployeeContext _db;

        public HomeController(EmployeeContext db)
        {
            _db = db;
        }

        [Authorize]
        public IActionResult Index()
        {
            var rese = _db.Employees.ToList();
            return View(rese);
        }
        public IActionResult login(login obj)
        {
            var log = _db.logins.Where(m => m.email == obj.email).FirstOrDefault();
            if(log==null)
            {
                TempData["email"] = "invalid ";
            }
            else
            {
                if(log.email==obj.email && log.password==obj.password)
                {
                    var claims = new[] { new Claim(ClaimTypes.Name, log.name),
                                        new Claim(ClaimTypes.Email, log.email) };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };
                    HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity),
                    authProperties);


                    return RedirectToAction("index");
                }
                else
                {
                    TempData["pass"] = "invalid ";
                }
            }
            return View();
        }
        [Authorize]
        public ActionResult logout()
        {

            HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme
                );

                return RedirectToAction("login");

           
        }
        [Authorize]
        public ActionResult form()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult form(Employee obj1)
        {
         
            Employee obj2 = new Employee();
            obj2.id = obj1.id;
            obj2.name = obj1.name;
            obj2.email = obj1.email;
            obj2.mobile = obj1.mobile;
            if (obj1.id == 0)
            {
                _db.Employees.Add(obj2);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                _db.Entry(obj2).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }
        [Authorize]
        public ActionResult delete(int id)
        {
          
            var delete = _db.Employees.Where(x => x.id == id).FirstOrDefault();
            _db.Employees.Remove(delete);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize]
        public ActionResult edit(int Id)
        {
            Employee edi = new Employee();
         
            var dit = _db.Employees.Where(x => x.id == Id).FirstOrDefault();
            edi.id = dit.id;

            edi.name = dit.name;
            edi.email = dit.email;
            edi.mobile = dit.mobile;


            return View("form", edi);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
