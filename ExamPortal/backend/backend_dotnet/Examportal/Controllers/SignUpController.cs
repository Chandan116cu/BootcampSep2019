using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Examportal.Custom_Models;
using Examportal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Bcrypt =  BCrypt.Net;

namespace Examportal.Controllers
{
    
    [ApiController]
    public class SignUpController : ControllerBase
    {
        ExamportalContext db = new ExamportalContext();
        [Route("/signUp")]
        [HttpPost]
        public IActionResult student([FromBody] Users value)
        {
            var data = (from c in db.Users where c.Email == value.Email select c).FirstOrDefault();
            if(data == null)
            {
                value.CreatedBy = "Student";
                value.AccountType = "Examiner";
                value.CreatedDate = DateTime.Now;
                value.Password = Bcrypt.BCrypt.HashPassword(value.Password);
                db.Users.Add(value);
                db.SaveChanges();
                return Ok(true);
            }
            else
            {
                return BadRequest();
            }
            
        }

      
    }
}
