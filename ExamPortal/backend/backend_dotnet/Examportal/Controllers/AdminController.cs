using System;
using System.Linq;
using Examportal.Custom_Models;
using Examportal.Models;
using Microsoft.AspNetCore.Mvc;
using Bcrypt = BCrypt.Net;


namespace Examportal.Controllers
{
    [Route("/examiner")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        ExamportalContext db = new ExamportalContext();
   
        [HttpGet]
        public IActionResult Display()
        {
            var item = db.Users.Where(e => e.AccountType == "Examiner").ToList().
                Select(a =>new AdminCustomModel{
                    CreatedDate = a.CreatedDate?.ToString("dd MMMM yyyy"),
                    _id = a.Email,Name = a.Name ,Email = a.Email
                });
           
            return Ok(item);
        }
        
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
      
        [HttpDelete("{id}")]
        public IActionResult DeleteExaminer(String id)
        {
            var data = db.Users.Where(s => s.Email == id).FirstOrDefault();
            db.Users.Remove(data);
            db.SaveChanges();
            return Ok(true);
        }
    }
}
