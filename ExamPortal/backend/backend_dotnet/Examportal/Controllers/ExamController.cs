using System.Linq;
using System;
using System.IO;
using Examportal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Examportal.Auth;
using System.Net;
using Microsoft.Net.Http.Headers;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using System.Configuration;

using Examportal.Custom_Models;

using Examportal.Handlers;
using System.Web;



namespace Examportal.Controllers
{

    [ApiController]
    [Route("/exam")]
    public class ExamController : ControllerBase
    {
        ExamportalContext db = new ExamportalContext();

        [Route("accessKey")]
        [HttpPost]
        public IActionResult CheckAccessKey([FromBody] ExamDetails value)
        {


            QuestionHandler qh = new QuestionHandler();
            var existingExam = qh.CheckAccessKey(value);

            
            if (existingExam != null)
            {
                return Ok(true);
            }
            else
            {
                return BadRequest();
            }
        }

        [Authorize]

        [Route("exam/accessKey")]
        [HttpGet]

        public IActionResult GetExamTime()
        {
            Authentication auth = new Authentication();
            var header = auth.getAllClaims(HttpContext);
            string examcode = HttpContext.Request.Headers["examCode"];

            var examData = db.ExamDetails.FirstOrDefault(s => s.ExamCode == examcode);

            return Ok(new { examData = examData, submitStatus = false });

        }



        [Route("/exam/questions/uploadExcel")]
        [HttpPost]

        public async Task createDirectoryAsync()
        {

            string currentpath = System.IO.Directory.GetCurrentDirectory();
            string foldername = "Files";
            string path = Path.Combine(currentpath, foldername);


            try
            {
                // Determine whether the directory exists.
                if (Directory.Exists(path))
                {

                    var filePayload = HttpContext.Request.Form.Files[0];

                    if (filePayload.Length > 0)
                        using (var fileStream = new FileStream(Path.Combine(path, filePayload.FileName), FileMode.Create))
                            await filePayload.CopyToAsync(fileStream);


                    if (filePayload.Length > 0)
                        using (var fileStream = new FileStream(Path.Combine(path, filePayload.FileName), FileMode.Create))
                            await filePayload.CopyToAsync(fileStream);

                    var i = 0;
                    var index = 0;
                    string[] filesdirectory = Directory.GetFiles(path, "*.xlsx", SearchOption.AllDirectories);
                    for (i = 0; i < filesdirectory.Length; i++)
                    {
                        String[] str = filesdirectory[i].Split("\\");
                        int length = str.Length;
                        if (str[length - 1] == (filePayload.FileName))
                        {
                            index = i;
                        }
                    }
                    FileInfo file = new FileInfo(filesdirectory[index]);
                    using (var package = new ExcelPackage(file))
                    {
                        var worksheet = package.Workbook.Worksheets[1];


                        int rowCount = worksheet.Dimension.Rows;
                        int ColCount = worksheet.Dimension.Columns;
                        StringBuilder rawText = new StringBuilder();
                        String result = "";
                        for (int row = 1; row <= rowCount; row++)
                        {
                            Questions questions = new Questions();
                            for (int col = 1; col <= ColCount; col++)
                            {


                                // This is just for demo purposes
                                rawText.Append(worksheet.Cells[row, col].Value.ToString() + " ");
                                result = rawText.ToString();
                                if (col == 1)
                                {
                                    questions.QuestionText = result;
                                }
                                if (col == 2)
                                {
                                    questions.Option1 = result;
                                }
                                if (col == 3)
                                {
                                    questions.Option2 = result;
                                }
                                if (col == 4)
                                {
                                    questions.Option3 = result;
                                }
                                if (col == 5) {
                                    questions.Option4 = result;
                                }

                                if (col == 6)
                                {
                                    questions.Answer = result;
                                }
                                if (col == 7)
                                {
                                    questions.Weightage = int.Parse(result);
                                }
                                if (col == 8)
                                {
                                    questions.QuestionImage = result;
                                }
                                if (col == 9)
                                {
                                    questions.AnswerType = result;
                                }
                                rawText.Remove(0, rawText.Length);
                            }
                            result = result.Trim();



                            db.Questions.Add(questions);
                            db.SaveChanges();
                        }
                    }
                    return;


                }

                // create the directory.
                DirectoryInfo di = Directory.CreateDirectory(path);

                Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(path));
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }

        }

        //[Authorize]
        [Route("endTest")]
        [HttpPost]
        public IActionResult SaveAllQuestions([FromBody] SubmitAnswerCustomModel value)
        {
            Dictionary<string, string> email = new Dictionary<string, string>();
            Authentication auth = new Authentication();
            QuestionHandler qh = new QuestionHandler();

            email = auth.getAllClaims(HttpContext);
            qh.submitAllQuestions(value, email);

            return Ok();
        }
        //[Authorize]
        [Route("/exam/question/{id}")]
        [HttpDelete]
        public IActionResult removeQuestions(int id)
        {
            try
            {
                db.Questions.Remove(db.Questions.FirstOrDefault(e => e.Id == id));
                db.SaveChanges();
                return Ok();
            }


            catch (Exception e)
            {
                return BadRequest(new { error = e });
            }
        }

    }
}

    