using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.UI.WebControls;
using webapi3.connetion;

namespace webapi3.Controllers
{
    public class ValuesController : ApiController
    {


        [HttpGet]
        [Route("Emp/getemployee")]
        public List<mytable> show()
        {
            mbvdatabaseEntities1 obj = new mbvdatabaseEntities1();
            var rev = obj.mytables.ToList();
            return rev;
        }
        [HttpPost]
        [Route("Emp/SaveEmployee")]
        public HttpResponseMessage SaveEmp(mytable obj)
        {

            mbvdatabaseEntities1 Dbcon = new mbvdatabaseEntities1();

            mytable tblEmp = new mytable();

            if (obj.id == 0)
            {
                Dbcon.mytables.Add(obj);
                Dbcon.SaveChanges();

            }

            else
            {
                Dbcon.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                Dbcon.SaveChanges();

            }

            HttpResponseMessage res = new HttpResponseMessage(HttpStatusCode.OK);

            return res;


        }
        [HttpPut]
        [Route("Emp/putemployee")]
        public IHttpActionResult edit(mytable obj)
        {
            mbvdatabaseEntities1 Dbcon = new mbvdatabaseEntities1();

            Dbcon.Entry(obj).State = System.Data.Entity.EntityState.Modified;
            Dbcon.SaveChanges();

          

            return Ok();

        }
    }
}