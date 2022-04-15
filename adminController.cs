using BootstraptemplateTesting.connect;
using BootstraptemplateTesting.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;

namespace BootstraptemplateTesting.Controllers
{
    public class adminController : Controller
    {

        Uri baseAddress = new Uri("http://localhost:56294/");

        HttpClient Client;


        public adminController()
        {

            Client = new HttpClient();

            Client.BaseAddress = baseAddress;

        }
        // GET: admin

        public ActionResult ragister()
        {
            return View();
        }
        [HttpPost]
     
        public ActionResult ragister(Class2 obj1)
        {
            mbvdatabaseEntities3 obj = new mbvdatabaseEntities3();
            Table obj2 = new Table();
        
            obj2.name = obj1.name;
            obj2.email = obj1.email;
            obj2.password = obj1.password;
             obj.Tables.Add(obj2);
                obj.SaveChanges();
            return RedirectToAction("login");
        }
       
        public ActionResult login()
        {

            return View();
        }
        [Authorize]
        public ActionResult Logout()
        {

            FormsAuthentication.SignOut();

            return RedirectToAction("login");
          
        }
        [HttpPost]
        
        public ActionResult login(Class2 obj1)
        {
            mbvdatabaseEntities3 obj = new mbvdatabaseEntities3();
            var log = obj.Tables.Where(a => a.email == obj1.email).FirstOrDefault();
            if(log==null)
            {
                TempData["abc"] = "email not valid";
            }
            else
            {
                if(log.email==obj1.email && log.password == obj1.password)
                {
                    FormsAuthentication.SetAuthCookie(log.email, false);
                    Session["Abc"] = log.name;
                    //Session["b"] = log.email;
                    return RedirectToAction("indexdasbord");
                }
                else
                {
                    TempData["abc"] = "password not valid";
                }
            }
            return View();
        }
        [Authorize]
        public ActionResult indexdasbord()
        {
            return View();
        }
        [Authorize]
        public ActionResult table()
        {
            
            List<Class1> obj2 = new List<Class1>();
            List<mytable> obj1 = new List<mytable>();
            HttpResponseMessage emp = Client.GetAsync(Client.BaseAddress + "Emp/GetEmployee").Result;

            if (emp.IsSuccessStatusCode)
            {

                string data = emp.Content.ReadAsStringAsync().Result;


                obj1 = JsonConvert.DeserializeObject<List<mytable>>(data);

            }
            //foreach (var item in obj1)
            //{
            //    obj2.Add(new Class1
            //    {
            //        id=item.id,
            //        name=item.name,
            //        email=item.email
            //    });
            //}
            return View(obj1);
           
        }
        [Authorize]
        public ActionResult form()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult form(Class1 obj1)
        {

            mytable obj2 = new mytable();
            obj2.name = obj1.name;
            obj2.email = obj1.email;
            string data = JsonConvert.SerializeObject(obj2);

            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage res = Client.PostAsync(Client.BaseAddress + "Emp/SaveEmployee", content).Result;

            if (res.IsSuccessStatusCode)
            {


            }
            return RedirectToAction("table");
        }
        [Authorize]
        public ActionResult delete(int id)
        {
            mbvdatabaseEntities3 obj = new mbvdatabaseEntities3();
            var delete = obj.mytables.Where(x => x.id == id).First();
            obj.mytables.Remove(delete);
            obj.SaveChanges();
            return RedirectToAction("table");
        }
        [HttpGet]
        
      
        [Authorize]
        public ActionResult edit(Class1 obj1)
        {
            mytable obj2 = new mytable();
            mbvdatabaseEntities3 obj = new mbvdatabaseEntities3();
            

            obj2.name = obj1.name;
            obj2.email = obj1.email;
            obj.Entry(obj2).State = EntityState.Modified;
           
            
                return RedirectToAction("table");
            
            //mytable obj2 = new mytable();
            //obj2.name = obj1.name;
            //obj2.email = obj1.email;

            //mytable edi = new mytable();
            //mbvdatabaseEntities3 obj = new mbvdatabaseEntities3();
            //var edit = obj.mytables.Where(x => x.id == id).First();


            //edi.name = edit.name;
            //edi.email = edit.email;


            //return View("form", edi);
           
        }
        [HttpPost]
        [Authorize]
        public ActionResult edit(int id)
        {
            mbvdatabaseEntities3 obj = new mbvdatabaseEntities3();
            var del = obj.mytables.Where(m => m.id == id).FirstOrDefault();
            return View("edit", del);
        }

    }
}