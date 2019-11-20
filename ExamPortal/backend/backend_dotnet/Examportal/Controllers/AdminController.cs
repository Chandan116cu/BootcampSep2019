using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Examportal.Custom_Models;
using Examportal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Bcrypt = BCrypt.Net;


namespace Examportal.Controllers
{
    [Route("/examiner")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        ExamportalContext db = new ExamportalContext();
        // GET: api/Admin
        [HttpGet]
        public IActionResult display()
        {
            var itm = db.Users.Where(e => e.AccountType == "Examiner").ToList().
                Select(a =>new AdminCustomModel{  CreatedDate = a.CreatedDate?.ToString("dd MMMM yyyy"),_id = a.Email,Name = a.Name ,Email = a.Email});
            
            //var b = Convert.ToString(itm[0].CreatedDate);
            //var c = b.ToString("dddd, dd MMMM yyyy");
          
            //var itm = (from data in db.Users select data.CreatedDate).ToList().Select(d => d.ToString("yyyy-MM-dd"));
            //kvar selectQuery = from add in db.Users.AsEnumerable().Select(d=> );
            return Ok(itm);
        }

        // GET: api/Admin/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Admin
        [HttpPost]
        public IActionResult Post([FromBody] Users value)
        {
            var data = (from c in db.Users where c.Email == value.Email select c).FirstOrDefault();
            if (data != null)
            {
                return Ok(new { message = "user already exist" });
            }
             if (data == null)
            {
                value.Password = Bcrypt.BCrypt.HashPassword(value.Password);
                value.CreatedDate =DateTime.Now;
                db.Users.Add(value);
                db.SaveChanges();
                return Ok(true);
            }
            else
            {
                return BadRequest();
            }

        }
        // PUT: api/Admin/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public IActionResult deleteexaminer(String id)
        {
           // db.Users.Remove(db.Users.FirstOrDefault(e => e.Email == id));
            var data = db.Users.Where(s => s.Email == id).FirstOrDefault();
            db.Users.Remove(data);
            db.SaveChanges();
            return Ok(true);
        }
    }
}
