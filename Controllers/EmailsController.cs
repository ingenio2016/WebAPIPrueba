using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using WebAPIPrueba.Models;

namespace WebAPIPrueba.Controllers
{
    public class EmailsController : Controller
    {
        private WebApiPruebaContext db = new WebApiPruebaContext();

        // GET: Emails/Create
        public ActionResult Create()
        {
            ViewBag.userId = new SelectList(
                CombosHelper.GetUserNoVoters(),
                "userId",
                "name");

            ViewBag.ReferId = new SelectList(
                CombosHelper.GetStandaloneRefer(),
                "ReferId",
                "FullName");

            ViewBag.FilterId = new SelectList(
                    CombosHelper.GetVoterFilters(),
                    "FiltersId",
                    "name");

            ViewBag.VotingPlaceId = new SelectList(
                CombosHelper.GetVotingPlaces(),
                "VotingPlaceId",
                "Name");

            ViewBag.WorkPlaceId = new SelectList(
                CombosHelper.GetWorkPlaces(),
                "WorkPlaceId",
                "Name");

            ViewBag.Comuna = new SelectList(
                    CombosHelper.GetCommunes(),
                    "CommuneId",
                    "Name");
            return View();
        }

        public JsonResult GetPerfil(int userId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var refer = db.Refers.Where(r => r.ReferType == userId);
            return Json(refer);
        }

        [HttpPost]
        public JsonResult loadExcelContacts()
        {
            List<ExcelEmail> contacts = new List<ExcelEmail>();
            List<Mistakes> errores = new List<Mistakes>();
            for (int i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];
                string extension = Path.GetExtension(file.FileName).ToLower();
                string connString = "";
                string[] validFileTypes = { ".xlsx" };

                string path1 = string.Format("{0}/{1}", Server.MapPath("~/Content/ExcelContactsUpload"), file.FileName);

                if (!Directory.Exists(path1))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Content/ExcelContactsUpload"));
                }
                if (validFileTypes.Contains(extension))
                {
                    if (System.IO.File.Exists(path1))
                    { System.IO.File.Delete(path1); }
                    file.SaveAs(path1);

                    if (extension.Trim() == ".xlsx")
                    {
                        connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path1 + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                        DataTable dt = Utility.ConvertXSLXtoDataTable(path1, connString);

                        foreach (DataRow row in dt.Rows)
                        {
                            if (row[0].ToString() != "")
                            {
                                var contact = new ExcelEmail();
                                contact.name = row[0].ToString();
                                contact.email = row[1].ToString();
                                if (ValidateEmail(contact.email))
                                {
                                    contacts.Add(contact);
                                }
                            }
                        }
                    }
                }
            }

            var jsonResult = Json(contacts, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        public JsonResult SearchVoters(string user, string refer, string comuna, string votacion)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Voter> voters = new List<Voter>();

            if (user != "0" && refer != "0")
            {
                var referUser = db.Refers.Find(Convert.ToInt32(refer));

                var voters1 = db.Voters.Where(v => v.ReferId == referUser.ReferId).ToList();
                foreach(var voter in voters1)
                {
                    if(voter.UserName != null)
                    {
                        if (ValidateEmail(voter.UserName))
                        {
                            voters.Add(voter);
                        }
                    }                    
                }

                if (comuna != "0")
                {
                    voters = new List<Voter>();
                    var voters2 = db.Voters.Where(v => v.ReferId == referUser.ReferId && v.CommuneId == comuna).ToList();
                    foreach (var voter2 in voters2)
                    {
                        if (voter2.UserName != null)
                        {
                            if (ValidateEmail(voter2.UserName))
                            {
                                voters.Add(voter2);
                            }
                        }
                    }
                }

                if (votacion != "[Seleccione un Lugar de Votación]")
                {
                    voters = new List<Voter>();
                    var voters3 = db.Voters.Where(v => v.ReferId == referUser.ReferId && v.CommuneId == comuna && v.VotingPlaceId == votacion).ToList();
                    foreach (var voter3 in voters3)
                    {
                        if (voter3.UserName != null)
                        {
                            if (ValidateEmail(voter3.UserName))
                            {
                                voters.Add(voter3);
                            }
                        }
                    }
                }
            }
            else
            {
                if (comuna != "0")
                {
                    var voters1 = db.Voters.Where(v => v.CommuneId == comuna).ToList();
                    foreach (var voter in voters1)
                    {
                        if (voter.UserName != null)
                        {
                            if (ValidateEmail(voter.UserName))
                            {
                                voters.Add(voter);
                            }
                        }
                    }

                    if (votacion != "[Seleccione un Lugar de Votación]")
                    {
                        voters = new List<Voter>();
                        var voters2 = db.Voters.Where(v => v.CommuneId == comuna && v.VotingPlaceId == votacion).ToList();
                        foreach (var voter2 in voters2)
                        {
                            if (voter2.UserName != null)
                            {
                                if (ValidateEmail(voter2.UserName))
                                {
                                    voters.Add(voter2);
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (votacion != "[Seleccione un Lugar de Votación]")
                    {
                        var voters1 = db.Voters.Where(v => v.VotingPlaceId == votacion).ToList();
                        foreach (var voter in voters1)
                        {
                            if (voter.UserName != null)
                            {
                                if (ValidateEmail(voter.UserName))
                                {
                                    voters.Add(voter);
                                }
                            }
                        }
                    }
                    else
                    {
                        voters = new List<Voter>();
                        var voters1 = db.Voters.ToList();
                        foreach (var voter in voters1)
                        {
                            if (voter.UserName != null)
                            {
                                if (ValidateEmail(voter.UserName))
                                {
                                    voters.Add(voter);
                                }
                            }
                        }
                    }
                }
            }

            var jsonResult = Json(voters, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult sendEmail(EmailData data)
        {
            string from = "joer04011992@gmail.com";
            string password = "ITCcolp23++";
            string subject = data.subject;
            string body = data.message;

            foreach(var contact in data.emails)
            {
                Thread email = new Thread(delegate ()
                {
                    SendEmail(contact.email, from, password, subject, body);
                });

                email.IsBackground = true;
                email.Start();
            }

            ResponseNotification response = new ResponseNotification();

            response.Success = true;
            response.Message = "Correos enviados exitosamente";

            var jsonResult = Json(response, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        private void SendEmail(string to, string from, string password, string subject, string body)
        {
            using (MailMessage mm = new MailMessage(from, to))
            {
                mm.Subject = subject;
                mm.Body = body;
                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                NetworkCredential NetworkCred = new NetworkCredential(from, password);
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                using (var context = new WebApiPruebaContext())
                {
                    try
                    {
                        smtp.Send(mm);
                        var emailStatus = new EmailSendStatus
                        {
                            Email = to,
                            Send = true
                        };
                        context.EmailSendStatus.Add(emailStatus);
                        context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        var emailStatus = new EmailSendStatus
                        {
                            Email = to,
                            Send = false
                        };
                        context.EmailSendStatus.Add(emailStatus);
                        context.SaveChanges();
                    }
                }
            }
        }

        private bool ValidateEmail(string email)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            if (match.Success)
                return true;
            else
                return false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
