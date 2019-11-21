using System.Linq;
using Examportal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Examportal.Auth;
using Examportal.Custom_Models;
using System.Collections.Generic;
using Examportal.Handlers;
using System.Web;
using System;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;

namespace Examportal.Controllers
{
    [ApiController]
    [Route("/exam")]
    public class ExamController : ControllerBase
    {
        IConfiguration configuration;
        public ExamController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        ExamportalContext db = new ExamportalContext();
        [Authorize, HttpDelete,Route("/exam/question/{id}")]
        
        public IActionResult RemoveQuestions(int id)
        {
            QuestionOperation question = new QuestionOperation();
            if(question.RemoveQuestion(id))
            {
                return Ok(new {flag=true , msg = "deletion successfull", status = 200, });
            }
            return BadRequest(new { flag = false, msg = "data not found", status = 404 });
        }

        [Authorize,HttpGet,Route("/exam/{id}/question")]
        
        public IActionResult ViewQuestions(String id)
        {
            id = HttpUtility.UrlDecode(id);
            QuestionOperation question = new QuestionOperation();
            var data = question.ViewQuestion(id);
                if ( data != null )
                    return Ok(data);
                else
                return BadRequest(new { msg = "Not Found",status = 404 });
        }

        [Authorize,HttpGet,Route("/exam/question/{id}")]    
        public IActionResult EditviewQuestions(int id)
        {
            QuestionOperation question = new QuestionOperation();
            ViewCustomModel data = question.EditViewQuestion(id);
            if (data != null)
            return Ok( data);
            return BadRequest(new { msg = "data not found", status = 404 });
        }
        //update question
        [Authorize,HttpPatch,Route("/exam/question/{id}")]
        
        public IActionResult editQuestions(int id)
        {
            try
            {
                var req = HttpContext.Request.Form;

                string currentpath = Directory.GetCurrentDirectory().ToString();
                string parent = Directory.GetParent(currentpath).ToString();
                string parentdirectory = Directory.GetParent(parent).ToString();
                string root = Directory.GetParent(parentdirectory).ToString();
                string dest = Path.Combine(root, "assets");

                var file = req.Files != null && HttpContext.Request.Form.Files.Count() > 0 ? req.Files[0] : null;

                String ImageURL = null;
                if (file != null)
                {
                    if (Directory.Exists(dest))
                    {

                        var filename = ContentDispositionHeaderValue
                                          .Parse(file.ContentDisposition)
                                          .FileName
                                          .Trim('"');
                       
                        String date = DateTime.Now.ToString();

                        date = Regex.Replace(date, @"s", "");

                        filename = dest + "\\" + filename;

                        using (FileStream fs = System.IO.File.Create(filename))
                        {
                            file.CopyTo(fs);
                            fs.Flush();
                        }

                        ImageURL = "../public/assets/" + file.FileName;

                    }
                }
                var obj = db.Questions.FirstOrDefault(e => e.Id == id);

                obj.Answer = req["answer"];

                obj.QuestionText = req["questionText"]; obj.Option1 = req["option1"]; obj.Option2 = req["option2"];

                obj.Option3 = req["option3"]; obj.Option4 = req["option4"];

                obj.Weightage = Convert.ToInt32(req["weightage"]); obj.AnswerType = req["answerType"];

                obj.ModifiedDate = DateTime.Now;

                obj.QuestionImage = ImageURL != null ? ImageURL : obj.QuestionImage;

                obj.ModifiedBy = obj.CreatedBy; obj.ExamCode = obj.ExamCode;

                obj.CreatedBy = obj.CreatedBy; obj.CreatedDate = obj.CreatedDate;
                   
                db.SaveChanges();
                return Ok(new { msg ="update successfull", status = 200});
            }
            catch (Exception e)
            {
                return BadRequest(new { error = e });
            }
        }

        [Authorize,HttpPost, Route("/exam/question")]
       //upload questions
        public IActionResult UploadQuestion()
        {            
            try
            {
                var req = HttpContext.Request.Form;

                string currentpath = Directory.GetCurrentDirectory().ToString();
                string parent = Directory.GetParent(currentpath).ToString();
                string parentdirectory = Directory.GetParent(parent).ToString();
                string root = Directory.GetParent(parentdirectory).ToString();
                string dest = Path.Combine(root, "assets");

                var file = HttpContext.Request.Form.Files != null && HttpContext.Request.Form.Files.Count() > 0 ? HttpContext.Request.Form.Files[0] : null;

                String imageURL = null;
                if (file != null)
                {
                    if (Directory.Exists(dest))
                    {

                        var filename = ContentDispositionHeaderValue
                                          .Parse(file.ContentDisposition)
                                          .FileName
                                          .Trim('"');
                        String date = DateTime.Now.Ticks.ToString();
                        String imageName = date + file.FileName;
                        filename = dest + "\\" + date + filename;

                        using (FileStream fs = System.IO.File.Create(filename))
                        {
                            file.CopyTo(fs);
                            fs.Flush();
                        }
                   
                        imageURL = "../../../assets/" + imageName;

                    }
                }
                Dictionary<string, string> email = new Dictionary<string, string>();

                Authentication auth = new Authentication();
                email = auth.getAllClaims(HttpContext);

                Questions obj = new Questions();

                obj.Answer = req["answer"];

                obj.QuestionText = req["questionText"];
                obj.Option1 = req["option1"];
                obj.Option2 = req["option2"];

                obj.Option3 = req["option3"];
                obj.Option4 = req["option4"];
                obj.ExamCode = req["examCode"];

                obj.Weightage = Convert.ToInt32(req["weightage"]);
                obj.QuestionImage = imageURL;
                obj.AnswerType = req["answerType"];

                obj.CreatedDate = DateTime.Now;

                obj.CreatedBy = db.Users.FirstOrDefault(u => u.Email == email["Email"]).Name;

                db.Questions.Add(obj);
                db.SaveChanges();

                return Ok(new { msg = "question saved successfully" });
            }
            catch (Exception e)
            {
                return BadRequest(new { error = e });
            }
        }

        [Authorize,HttpPost, Route("/exam/questions/uploadExcel")]
        
        public async Task createDirectoryAsync()
        {

            string currentpath = System.IO.Directory.GetCurrentDirectory();
            string foldername = "Files";
            string path = Path.Combine(currentpath, foldername);


            try
            {
                // Determine whether the directory exists.
                DirectoryInfo di = Directory.CreateDirectory(path);

                Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(path));
                //if (Directory.Exists(path))
                {

                    var filePayload = HttpContext.Request.Form.Files[0];
                    var examcode = HttpContext.Request.Form["examCode"];

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
                                if (col == 5)
                                {
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
                                questions.ExamCode = examcode;
                                questions.CreatedDate = DateTime.Now;
                                rawText.Remove(0, rawText.Length);
                            }
                            result = result.Trim();



                            db.Questions.Add(questions);
                            db.SaveChanges();
                        }
                    }
                    return;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }

        }
    }
}
