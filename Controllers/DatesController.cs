using Microsoft.Reporting.WebForms;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAPIPrueba.Models;

namespace WebAPIPrueba.Controllers
{
    [Authorize(Roles = "User, Reunion")]
    public class DatesController : Controller
    {
        private WebApiPruebaContext db = new WebApiPruebaContext();

        // GET: Dates
        public ActionResult Index(DateTime? fechaini, DateTime? fechafin,int? page = null)
        {
            ViewBag.uno = fechaini;
            ViewBag.dos = fechafin;
            page = (page ?? 1);
            if (fechafin != null || fechaini != null)
            {
                DateTime ini = Convert.ToDateTime(fechaini);
                DateTime fin = Convert.ToDateTime(fechafin);
                var dates = db.Dates.Where(d => d.EventDate >= fechaini && d.EventDate <= fechafin);
                return View(dates.OrderBy(x => x.ProfessionalId).ToPagedList((int)page, 5));
            }           
            
            return View(db.Dates.OrderBy(x=> x.ProfessionalId).ToPagedList((int)page, 5));
        }

        public ActionResult DatesReport(DateTime? fechaini, DateTime? fechafin)
        {
            var inc = new object();
            if(fechafin != null || fechafin!=null)
            {
                inc = (from d in db.Dates
                       where d.EventDate >= fechaini && d.EventDate <= fechafin
                       orderby d.EventDate
                       select new { d.organizador, d.Phone, d.EventDate, d.Hour, d.Observation, d.Address,d.PersonsNumber, d.Asistencia, d.Moderator }).ToList();
            }
            else
            {
                inc = (from d in db.Dates
                       orderby d.EventDate
                       select new { d.organizador, d.Phone, d.EventDate, d.Hour, d.Observation, d.Address, d.PersonsNumber, d.Asistencia, d.Moderator }).ToList();
            }
            string path = Path.Combine(Server.MapPath("~/Reports"), "DatesList.rdlc");

            LocalReport lr = new LocalReport();
            lr.EnableExternalImages = true;
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return RedirectToAction("Index", "Dates");
            }

            var company = db.Companies.Find(1);//TRAE LA EMPRESA UNICA

            string fullname = string.Empty;
            if (company.Logo == null)
            {
                fullname = AppDomain.CurrentDomain.BaseDirectory + "Content/CompanyLogo/NoLogoCompany.png";
            }
            else
            {
                fullname = AppDomain.CurrentDomain.BaseDirectory + company.Logo.Substring(2);
            }
            ReportParameter paramLogo = new ReportParameter();
            paramLogo.Name = "Path";
            paramLogo.Values.Add(fullname);
            lr.SetParameters(paramLogo);

            ReportDataSource rd = new ReportDataSource("DataSet1", inc);
            lr.DataSources.Add(rd);
            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;



            string deviceInfo =

            "<DeviceInfo>" +
            "  <OutputFormat>" + "ReporteReuniones" + "</OutputFormat>" +
            "  <PageWidth>11in</PageWidth>" +
            "  <PageHeight>8.5in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>1in</MarginLeft>" +
            "  <MarginRight>1in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);

            return File(renderedBytes, mimeType);
        }
        

        // GET: Dates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dates dates = db.Dates.Find(id);
            if (dates == null)
            {
                return HttpNotFound();
            }
            return View(dates);
        }

        // GET: Dates/Create
        public ActionResult Create()
        {
            var date = new Dates
            {
                EventDate = DateTime.Now,
                SystemDate = DateTime.Now,
            };
            List<SelectListItem> mn = new List<SelectListItem>();
            mn.Add(new SelectListItem { Text = "Seleccione una Hora", Value = "0" });
            mn.Add(new SelectListItem { Text = "06:00 AM", Value = "06:00 AM" });
            mn.Add(new SelectListItem { Text = "06:15 AM", Value = "06:15 AM" });
            mn.Add(new SelectListItem { Text = "06:30 AM", Value = "06:30 AM" });
            mn.Add(new SelectListItem { Text = "06:45 AM", Value = "06:45 AM" });
            mn.Add(new SelectListItem { Text = "07:00 AM", Value = "07:00 AM" });
            mn.Add(new SelectListItem { Text = "07:15 AM", Value = "07:15 AM" });
            mn.Add(new SelectListItem { Text = "07:30 AM", Value = "07:30 AM" });
            mn.Add(new SelectListItem { Text = "07:45 AM", Value = "07:45 AM" });
            mn.Add(new SelectListItem { Text = "08:00 AM", Value = "08:00 AM" });
            mn.Add(new SelectListItem { Text = "08:15 AM", Value = "08:15 AM" });
            mn.Add(new SelectListItem { Text = "08:30 AM", Value = "08:30 AM" });
            mn.Add(new SelectListItem { Text = "08:45 AM", Value = "08:45 AM" });
            mn.Add(new SelectListItem { Text = "09:00 AM", Value = "09:00 AM" });
            mn.Add(new SelectListItem { Text = "09:15 AM", Value = "09:15 AM" });
            mn.Add(new SelectListItem { Text = "09:30 AM", Value = "09:30 AM" });
            mn.Add(new SelectListItem { Text = "09:45 AM", Value = "09:45 AM" });
            mn.Add(new SelectListItem { Text = "10:00 AM", Value = "10:00 AM" });
            mn.Add(new SelectListItem { Text = "10:15 AM", Value = "10:15 AM" });
            mn.Add(new SelectListItem { Text = "10:30 AM", Value = "10:30 AM" });
            mn.Add(new SelectListItem { Text = "10:45 AM", Value = "10:45 AM" });
            mn.Add(new SelectListItem { Text = "11:00 AM", Value = "11:00 AM" });
            mn.Add(new SelectListItem { Text = "11:15 AM", Value = "11:15 AM" });
            mn.Add(new SelectListItem { Text = "11:30 AM", Value = "11:30 AM" });
            mn.Add(new SelectListItem { Text = "11:45 AM", Value = "11:45 AM" });
            mn.Add(new SelectListItem { Text = "12:00 PM", Value = "12:00 PM" });
            mn.Add(new SelectListItem { Text = "12:15 PM", Value = "12:15 PM" });
            mn.Add(new SelectListItem { Text = "12:30 PM", Value = "12:30 PM" });
            mn.Add(new SelectListItem { Text = "12:45 PM", Value = "12:45 PM" });
            mn.Add(new SelectListItem { Text = "01:00 PM", Value = "01:00 PM" });
            mn.Add(new SelectListItem { Text = "01:15 PM", Value = "01:15 PM" });
            mn.Add(new SelectListItem { Text = "01:30 PM", Value = "01:30 PM" });
            mn.Add(new SelectListItem { Text = "01:45 PM", Value = "01:45 PM" });
            mn.Add(new SelectListItem { Text = "02:00 PM", Value = "02:00 PM" });
            mn.Add(new SelectListItem { Text = "02:15 PM", Value = "02:15 PM" });
            mn.Add(new SelectListItem { Text = "02:30 PM", Value = "02:30 PM" });
            mn.Add(new SelectListItem { Text = "02:45 PM", Value = "02:45 PM" });
            mn.Add(new SelectListItem { Text = "03:00 PM", Value = "03:00 PM" });
            mn.Add(new SelectListItem { Text = "03:15 PM", Value = "03:15 PM" });
            mn.Add(new SelectListItem { Text = "03:30 PM", Value = "03:30 PM" });
            mn.Add(new SelectListItem { Text = "03:45 PM", Value = "03:45 PM" });
            mn.Add(new SelectListItem { Text = "04:00 PM", Value = "04:00 PM" });
            mn.Add(new SelectListItem { Text = "04:15 PM", Value = "04:15 PM" });
            mn.Add(new SelectListItem { Text = "04:30 PM", Value = "04:30 PM" });
            mn.Add(new SelectListItem { Text = "04:45 PM", Value = "04:45 PM" });
            mn.Add(new SelectListItem { Text = "05:00 PM", Value = "05:00 PM" });
            mn.Add(new SelectListItem { Text = "05:15 PM", Value = "05:15 PM" });
            mn.Add(new SelectListItem { Text = "05:30 PM", Value = "05:30 PM" });
            mn.Add(new SelectListItem { Text = "05:45 PM", Value = "05:45 PM" });
            mn.Add(new SelectListItem { Text = "06:00 PM", Value = "06:00 PM" });
            mn.Add(new SelectListItem { Text = "06:15 PM", Value = "06:15 PM" });
            mn.Add(new SelectListItem { Text = "06:30 PM", Value = "06:30 PM" });
            mn.Add(new SelectListItem { Text = "06:45 PM", Value = "06:45 PM" });
            mn.Add(new SelectListItem { Text = "07:00 PM", Value = "07:00 PM" });
            mn.Add(new SelectListItem { Text = "07:15 PM", Value = "07:15 PM" });
            mn.Add(new SelectListItem { Text = "07:30 PM", Value = "07:30 PM" });
            mn.Add(new SelectListItem { Text = "07:45 PM", Value = "07:45 PM" });
            mn.Add(new SelectListItem { Text = "08:00 PM", Value = "08:00 PM" });
            mn.Add(new SelectListItem { Text = "08:15 PM", Value = "08:15 PM" });
            mn.Add(new SelectListItem { Text = "08:30 PM", Value = "08:30 PM" });
            mn.Add(new SelectListItem { Text = "08:45 PM", Value = "08:45 PM" });
            mn.Add(new SelectListItem { Text = "09:00 PM", Value = "09:00 PM" });
            mn.Add(new SelectListItem { Text = "09:15 PM", Value = "09:15 PM" });
            mn.Add(new SelectListItem { Text = "09:30 PM", Value = "09:30 PM" });
            mn.Add(new SelectListItem { Text = "09:45 PM", Value = "09:45 PM" });

            ViewData["Hours"] = mn;
            return View(date);
        }

        // POST: Dates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Dates date)
        {
            if (string.IsNullOrEmpty(date.Observation))
            {
                date.Observation = "Reunión General";
            }
            if(date.ProfessionalId == 0 || date.EventDate == null || date.Hour == "0")
            {
                ModelState.AddModelError(string.Empty, "Ocurrió un error al asignar la Reunión, Verifique los Datos");
                List<SelectListItem> mn2 = new List<SelectListItem>();
                mn2.Add(new SelectListItem { Text = "Seleccione una Hora", Value = "0" });
                mn2.Add(new SelectListItem { Text = "06:00 AM", Value = "06:00 AM" });
                mn2.Add(new SelectListItem { Text = "06:15 AM", Value = "06:15 AM" });
                mn2.Add(new SelectListItem { Text = "06:30 AM", Value = "06:30 AM" });
                mn2.Add(new SelectListItem { Text = "06:45 AM", Value = "06:45 AM" });
                mn2.Add(new SelectListItem { Text = "07:00 AM", Value = "07:00 AM" });
                mn2.Add(new SelectListItem { Text = "07:15 AM", Value = "07:15 AM" });
                mn2.Add(new SelectListItem { Text = "07:30 AM", Value = "07:30 AM" });
                mn2.Add(new SelectListItem { Text = "07:45 AM", Value = "07:45 AM" });
                mn2.Add(new SelectListItem { Text = "08:00 AM", Value = "08:00 AM" });
                mn2.Add(new SelectListItem { Text = "08:15 AM", Value = "08:15 AM" });
                mn2.Add(new SelectListItem { Text = "08:30 AM", Value = "08:30 AM" });
                mn2.Add(new SelectListItem { Text = "08:45 AM", Value = "08:45 AM" });
                mn2.Add(new SelectListItem { Text = "09:00 AM", Value = "09:00 AM" });
                mn2.Add(new SelectListItem { Text = "09:15 AM", Value = "09:15 AM" });
                mn2.Add(new SelectListItem { Text = "09:30 AM", Value = "09:30 AM" });
                mn2.Add(new SelectListItem { Text = "09:45 AM", Value = "09:45 AM" });
                mn2.Add(new SelectListItem { Text = "10:00 AM", Value = "10:00 AM" });
                mn2.Add(new SelectListItem { Text = "10:15 AM", Value = "10:15 AM" });
                mn2.Add(new SelectListItem { Text = "10:30 AM", Value = "10:30 AM" });
                mn2.Add(new SelectListItem { Text = "10:45 AM", Value = "10:45 AM" });
                mn2.Add(new SelectListItem { Text = "11:00 AM", Value = "11:00 AM" });
                mn2.Add(new SelectListItem { Text = "11:15 AM", Value = "11:15 AM" });
                mn2.Add(new SelectListItem { Text = "11:30 AM", Value = "11:30 AM" });
                mn2.Add(new SelectListItem { Text = "11:45 AM", Value = "11:45 AM" });
                mn2.Add(new SelectListItem { Text = "12:00 PM", Value = "12:00 PM" });
                mn2.Add(new SelectListItem { Text = "12:15 PM", Value = "12:15 PM" });
                mn2.Add(new SelectListItem { Text = "12:30 PM", Value = "12:30 PM" });
                mn2.Add(new SelectListItem { Text = "12:45 PM", Value = "12:45 PM" });
                mn2.Add(new SelectListItem { Text = "01:00 PM", Value = "01:00 PM" });
                mn2.Add(new SelectListItem { Text = "01:15 PM", Value = "01:15 PM" });
                mn2.Add(new SelectListItem { Text = "01:30 PM", Value = "01:30 PM" });
                mn2.Add(new SelectListItem { Text = "01:45 PM", Value = "01:45 PM" });
                mn2.Add(new SelectListItem { Text = "02:00 PM", Value = "02:00 PM" });
                mn2.Add(new SelectListItem { Text = "02:15 PM", Value = "02:15 PM" });
                mn2.Add(new SelectListItem { Text = "02:30 PM", Value = "02:30 PM" });
                mn2.Add(new SelectListItem { Text = "02:45 PM", Value = "02:45 PM" });
                mn2.Add(new SelectListItem { Text = "03:00 PM", Value = "03:00 PM" });
                mn2.Add(new SelectListItem { Text = "03:15 PM", Value = "03:15 PM" });
                mn2.Add(new SelectListItem { Text = "03:30 PM", Value = "03:30 PM" });
                mn2.Add(new SelectListItem { Text = "03:45 PM", Value = "03:45 PM" });
                mn2.Add(new SelectListItem { Text = "04:00 PM", Value = "04:00 PM" });
                mn2.Add(new SelectListItem { Text = "04:15 PM", Value = "04:15 PM" });
                mn2.Add(new SelectListItem { Text = "04:30 PM", Value = "04:30 PM" });
                mn2.Add(new SelectListItem { Text = "04:45 PM", Value = "04:45 PM" });
                mn2.Add(new SelectListItem { Text = "05:00 PM", Value = "05:00 PM" });
                mn2.Add(new SelectListItem { Text = "05:15 PM", Value = "05:15 PM" });
                mn2.Add(new SelectListItem { Text = "05:30 PM", Value = "05:30 PM" });
                mn2.Add(new SelectListItem { Text = "05:45 PM", Value = "05:45 PM" });
                mn2.Add(new SelectListItem { Text = "06:00 PM", Value = "06:00 PM" });
                mn2.Add(new SelectListItem { Text = "06:15 PM", Value = "06:15 PM" });
                mn2.Add(new SelectListItem { Text = "06:30 PM", Value = "06:30 PM" });
                mn2.Add(new SelectListItem { Text = "06:45 PM", Value = "06:45 PM" });
                mn2.Add(new SelectListItem { Text = "07:00 PM", Value = "07:00 PM" });
                mn2.Add(new SelectListItem { Text = "07:15 PM", Value = "07:15 PM" });
                mn2.Add(new SelectListItem { Text = "07:30 PM", Value = "07:30 PM" });
                mn2.Add(new SelectListItem { Text = "07:45 PM", Value = "07:45 PM" });
                mn2.Add(new SelectListItem { Text = "08:00 PM", Value = "08:00 PM" });
                mn2.Add(new SelectListItem { Text = "08:15 PM", Value = "08:15 PM" });
                mn2.Add(new SelectListItem { Text = "08:30 PM", Value = "08:30 PM" });
                mn2.Add(new SelectListItem { Text = "08:45 PM", Value = "08:45 PM" });
                mn2.Add(new SelectListItem { Text = "09:00 PM", Value = "09:00 PM" });
                mn2.Add(new SelectListItem { Text = "09:15 PM", Value = "09:15 PM" });
                mn2.Add(new SelectListItem { Text = "09:30 PM", Value = "09:30 PM" });
                mn2.Add(new SelectListItem { Text = "09:45 PM", Value = "09:45 PM" });

                ViewData["Hours"] = mn2;

                return View(date);
            }
            else
            {
                if (date.Hour == "06:00 AM") date.HourId = 1;
                if (date.Hour == "06:15 AM") date.HourId = 2;
                if (date.Hour == "06:30 AM") date.HourId = 3;
                if (date.Hour == "06:45 AM") date.HourId = 4;
                if (date.Hour == "07:00 AM") date.HourId = 5;
                if (date.Hour == "07:15 AM") date.HourId = 6;
                if (date.Hour == "07:30 AM") date.HourId = 7;
                if (date.Hour == "07:45 AM") date.HourId = 8;
                if (date.Hour == "08:00 AM") date.HourId = 9;
                if (date.Hour == "08:15 AM") date.HourId = 10;
                if (date.Hour == "08:30 AM") date.HourId = 11;
                if (date.Hour == "08:45 AM") date.HourId = 12;
                if (date.Hour == "09:00 AM") date.HourId = 13;
                if (date.Hour == "09:15 AM") date.HourId = 14;
                if (date.Hour == "09:30 AM") date.HourId = 15;
                if (date.Hour == "09:45 AM") date.HourId = 16;
                if (date.Hour == "10:00 AM") date.HourId = 17;
                if (date.Hour == "10:15 AM") date.HourId = 18;
                if (date.Hour == "10:30 AM") date.HourId = 19;
                if (date.Hour == "10:45 AM") date.HourId = 20;
                if (date.Hour == "11:00 AM") date.HourId = 21;
                if (date.Hour == "11:15 AM") date.HourId = 22;
                if (date.Hour == "11:30 AM") date.HourId = 23;
                if (date.Hour == "11:45 AM") date.HourId = 24;
                if (date.Hour == "12:00 PM") date.HourId = 25;
                if (date.Hour == "12:15 PM") date.HourId = 26;
                if (date.Hour == "12:30 PM") date.HourId = 27;
                if (date.Hour == "12:45 PM") date.HourId = 28;
                if (date.Hour == "01:00 PM") date.HourId = 29;
                if (date.Hour == "01:15 PM") date.HourId = 30;
                if (date.Hour == "01:30 PM") date.HourId = 31;
                if (date.Hour == "01:45 PM") date.HourId = 32;
                if (date.Hour == "02:00 PM") date.HourId = 33;
                if (date.Hour == "02:15 PM") date.HourId = 34;
                if (date.Hour == "02:30 PM") date.HourId = 35;
                if (date.Hour == "02:45 PM") date.HourId = 36;
                if (date.Hour == "03:00 PM") date.HourId = 37;
                if (date.Hour == "03:15 PM") date.HourId = 38;
                if (date.Hour == "03:30 PM") date.HourId = 39;
                if (date.Hour == "03:45 PM") date.HourId = 40;
                if (date.Hour == "04:00 PM") date.HourId = 41;
                if (date.Hour == "04:15 PM") date.HourId = 42;
                if (date.Hour == "04:30 PM") date.HourId = 43;
                if (date.Hour == "04:45 PM") date.HourId = 44;
                if (date.Hour == "05:00 PM") date.HourId = 45;
                if (date.Hour == "05:15 PM") date.HourId = 46;
                if (date.Hour == "05:30 PM") date.HourId = 47;
                if (date.Hour == "05:45 PM") date.HourId = 48;
                if (date.Hour == "06:00 PM") date.HourId = 49;
                if (date.Hour == "06:15 PM") date.HourId = 50;
                if (date.Hour == "06:30 PM") date.HourId = 51;
                if (date.Hour == "06:45 PM") date.HourId = 52;
                if (date.Hour == "07:00 PM") date.HourId = 53;
                if (date.Hour == "07:15 PM") date.HourId = 54;
                if (date.Hour == "07:30 PM") date.HourId = 55;
                if (date.Hour == "07:45 PM") date.HourId = 56;
                if (date.Hour == "08:00 PM") date.HourId = 57;
                if (date.Hour == "08:15 PM") date.HourId = 58;
                if (date.Hour == "08:30 PM") date.HourId = 59;
                if (date.Hour == "08:45 PM") date.HourId = 60;
                if (date.Hour == "09:00 PM") date.HourId = 61;
                if (date.Hour == "09:15 PM") date.HourId = 62;
                if (date.Hour == "09:30 PM") date.HourId = 63;
                if (date.Hour == "09:45 PM") date.HourId = 64;

                string Nombre = string.Empty;
                var boss = db.Bosses.Where(b => b.Document == date.ProfessionalId).FirstOrDefault();
                if (boss != null)
                {
                    Nombre = boss.FullName;
                }
                else
                {
                    var enlace = db.Links.Where(b => b.Document == date.ProfessionalId).FirstOrDefault();
                    if (enlace != null)
                    {
                        Nombre = enlace.FullName;
                    }
                    else
                    {
                        var coordinador = db.Coordinators.Where(b => b.Document == date.ProfessionalId).FirstOrDefault();
                        if (coordinador != null)
                        {
                            Nombre = coordinador.FullName;
                        }
                        else
                        {
                            var lider = db.Leaders.Where(b => b.Document == date.ProfessionalId).FirstOrDefault();
                            Nombre = lider.FullName;
                        }
                    }

                }

                if (ModelState.IsValid)
                {
                    date.organizador = Nombre;
                    date.Asistencia = false;
                    db.Dates.Add(date);
                    try
                    {
                        var contacitas = db.Dates.Where(x => x.EventDate.Day == DateTime.Now.Day && x.EventDate.Month == DateTime.Now.Month && x.EventDate.Year == DateTime.Now.Year && x.HourId == date.HourId).ToList();
                        if (contacitas.Count < 8)
                        {
                            db.SaveChanges();
                            //INSERTO ID CITA EN TIMESDATE
                            var timedate = db.TimesDates.Where(x => x.ProfessionalId == date.ProfessionalId && x.EventDate == date.EventDate).FirstOrDefault();
                            if (timedate == null)
                            {
                                TimesDates timesdate = new TimesDates();
                                timesdate.ProfessionalId = date.ProfessionalId;
                                timesdate.EventDate = date.EventDate;
                                if (date.HourId == 1) timesdate.seis00 = date.DateId;
                                if (date.HourId == 2) timesdate.seis15 = date.DateId;
                                if (date.HourId == 3) timesdate.seis30 = date.DateId;
                                if (date.HourId == 4) timesdate.seis45 = date.DateId;
                                if (date.HourId == 5) timesdate.siete00 = date.DateId;
                                if (date.HourId == 6) timesdate.siete15 = date.DateId;
                                if (date.HourId == 7) timesdate.siete30 = date.DateId;
                                if (date.HourId == 8) timesdate.siete45 = date.DateId;
                                if (date.HourId == 9) timesdate.ocho00 = date.DateId;
                                if (date.HourId == 10) timesdate.ocho15 = date.DateId;
                                if (date.HourId == 11) timesdate.ocho30 = date.DateId;
                                if (date.HourId == 12) timesdate.ocho45 = date.DateId;
                                if (date.HourId == 13) timesdate.nueve00 = date.DateId;
                                if (date.HourId == 14) timesdate.nueve15 = date.DateId;
                                if (date.HourId == 15) timesdate.nueve30 = date.DateId;
                                if (date.HourId == 16) timesdate.nueve45 = date.DateId;
                                if (date.HourId == 17) timesdate.diez00 = date.DateId;
                                if (date.HourId == 18) timesdate.diez15 = date.DateId;
                                if (date.HourId == 19) timesdate.diez30 = date.DateId;
                                if (date.HourId == 20) timesdate.diez45 = date.DateId;
                                if (date.HourId == 21) timesdate.once00 = date.DateId;
                                if (date.HourId == 22) timesdate.once15 = date.DateId;
                                if (date.HourId == 23) timesdate.once30 = date.DateId;
                                if (date.HourId == 24) timesdate.once45 = date.DateId;
                                if (date.HourId == 25) timesdate.doce00 = date.DateId;
                                if (date.HourId == 26) timesdate.doce15 = date.DateId;
                                if (date.HourId == 27) timesdate.doce30 = date.DateId;
                                if (date.HourId == 28) timesdate.doce45 = date.DateId;
                                if (date.HourId == 29) timesdate.uno00 = date.DateId;
                                if (date.HourId == 30) timesdate.uno15 = date.DateId;
                                if (date.HourId == 31) timesdate.uno30 = date.DateId;
                                if (date.HourId == 32) timesdate.uno45 = date.DateId;
                                if (date.HourId == 33) timesdate.dos00 = date.DateId;
                                if (date.HourId == 34) timesdate.dos15 = date.DateId;
                                if (date.HourId == 35) timesdate.dos30 = date.DateId;
                                if (date.HourId == 36) timesdate.dos45 = date.DateId;
                                if (date.HourId == 37) timesdate.tres00 = date.DateId;
                                if (date.HourId == 38) timesdate.tres15 = date.DateId;
                                if (date.HourId == 39) timesdate.tres30 = date.DateId;
                                if (date.HourId == 40) timesdate.tres45 = date.DateId;
                                if (date.HourId == 41) timesdate.cuatro00 = date.DateId;
                                if (date.HourId == 42) timesdate.cuatro15 = date.DateId;
                                if (date.HourId == 43) timesdate.cuatro30 = date.DateId;
                                if (date.HourId == 44) timesdate.cuatro45 = date.DateId;
                                if (date.HourId == 45) timesdate.cinco00 = date.DateId;
                                if (date.HourId == 46) timesdate.cinco15 = date.DateId;
                                if (date.HourId == 47) timesdate.cinco30 = date.DateId;
                                if (date.HourId == 48) timesdate.cinco45 = date.DateId;
                                if (date.HourId == 49) timesdate.seisp00 = date.DateId;
                                if (date.HourId == 50) timesdate.seisp15 = date.DateId;
                                if (date.HourId == 51) timesdate.seisp30 = date.DateId;
                                if (date.HourId == 52) timesdate.seisp45 = date.DateId;
                                if (date.HourId == 53) timesdate.sietep00 = date.DateId;
                                if (date.HourId == 54) timesdate.sietep15 = date.DateId;
                                if (date.HourId == 55) timesdate.sietep30 = date.DateId;
                                if (date.HourId == 56) timesdate.sietp45 = date.DateId;
                                if (date.HourId == 57) timesdate.ochop00 = date.DateId;
                                if (date.HourId == 58) timesdate.ochop15 = date.DateId;
                                if (date.HourId == 59) timesdate.ochop30 = date.DateId;
                                if (date.HourId == 60) timesdate.ochop45 = date.DateId;
                                if (date.HourId == 61) timesdate.nuevep00 = date.DateId;
                                if (date.HourId == 62) timesdate.nuevep15 = date.DateId;
                                if (date.HourId == 63) timesdate.nuevep30 = date.DateId;
                                if (date.HourId == 64) timesdate.nuevep45 = date.DateId;
                                db.TimesDates.Add(timesdate);
                            }
                            else
                            {
                                if (date.HourId == 1) timedate.seis00 = date.DateId;
                                if (date.HourId == 2) timedate.seis15 = date.DateId;
                                if (date.HourId == 3) timedate.seis30 = date.DateId;
                                if (date.HourId == 4) timedate.seis45 = date.DateId;
                                if (date.HourId == 5) timedate.siete00 = date.DateId;
                                if (date.HourId == 6) timedate.siete15 = date.DateId;
                                if (date.HourId == 7) timedate.siete30 = date.DateId;
                                if (date.HourId == 8) timedate.siete45 = date.DateId;
                                if (date.HourId == 9) timedate.ocho00 = date.DateId;
                                if (date.HourId == 10) timedate.ocho15 = date.DateId;
                                if (date.HourId == 11) timedate.ocho30 = date.DateId;
                                if (date.HourId == 12) timedate.ocho45 = date.DateId;
                                if (date.HourId == 13) timedate.nueve00 = date.DateId;
                                if (date.HourId == 14) timedate.nueve15 = date.DateId;
                                if (date.HourId == 15) timedate.nueve30 = date.DateId;
                                if (date.HourId == 16) timedate.nueve45 = date.DateId;
                                if (date.HourId == 17) timedate.diez00 = date.DateId;
                                if (date.HourId == 18) timedate.diez15 = date.DateId;
                                if (date.HourId == 19) timedate.diez30 = date.DateId;
                                if (date.HourId == 20) timedate.diez45 = date.DateId;
                                if (date.HourId == 21) timedate.once00 = date.DateId;
                                if (date.HourId == 22) timedate.once15 = date.DateId;
                                if (date.HourId == 23) timedate.once30 = date.DateId;
                                if (date.HourId == 24) timedate.once45 = date.DateId;
                                if (date.HourId == 25) timedate.doce00 = date.DateId;
                                if (date.HourId == 26) timedate.doce15 = date.DateId;
                                if (date.HourId == 27) timedate.doce30 = date.DateId;
                                if (date.HourId == 28) timedate.doce45 = date.DateId;
                                if (date.HourId == 29) timedate.uno00 = date.DateId;
                                if (date.HourId == 30) timedate.uno15 = date.DateId;
                                if (date.HourId == 31) timedate.uno30 = date.DateId;
                                if (date.HourId == 32) timedate.uno45 = date.DateId;
                                if (date.HourId == 33) timedate.dos00 = date.DateId;
                                if (date.HourId == 34) timedate.dos15 = date.DateId;
                                if (date.HourId == 35) timedate.dos30 = date.DateId;
                                if (date.HourId == 36) timedate.dos45 = date.DateId;
                                if (date.HourId == 37) timedate.tres00 = date.DateId;
                                if (date.HourId == 38) timedate.tres15 = date.DateId;
                                if (date.HourId == 39) timedate.tres30 = date.DateId;
                                if (date.HourId == 40) timedate.tres45 = date.DateId;
                                if (date.HourId == 41) timedate.cuatro00 = date.DateId;
                                if (date.HourId == 42) timedate.cuatro15 = date.DateId;
                                if (date.HourId == 43) timedate.cuatro30 = date.DateId;
                                if (date.HourId == 44) timedate.cuatro45 = date.DateId;
                                if (date.HourId == 45) timedate.cinco00 = date.DateId;
                                if (date.HourId == 46) timedate.cinco15 = date.DateId;
                                if (date.HourId == 47) timedate.cinco30 = date.DateId;
                                if (date.HourId == 48) timedate.cinco45 = date.DateId;
                                if (date.HourId == 49) timedate.seisp00 = date.DateId;
                                if (date.HourId == 50) timedate.seisp15 = date.DateId;
                                if (date.HourId == 51) timedate.seisp30 = date.DateId;
                                if (date.HourId == 52) timedate.seisp45 = date.DateId;
                                if (date.HourId == 53) timedate.sietep00 = date.DateId;
                                if (date.HourId == 54) timedate.sietep15 = date.DateId;
                                if (date.HourId == 55) timedate.sietep30 = date.DateId;
                                if (date.HourId == 56) timedate.sietp45 = date.DateId;
                                if (date.HourId == 57) timedate.ochop00 = date.DateId;
                                if (date.HourId == 58) timedate.ochop15 = date.DateId;
                                if (date.HourId == 59) timedate.ochop30 = date.DateId;
                                if (date.HourId == 60) timedate.ochop45 = date.DateId;
                                if (date.HourId == 61) timedate.nuevep00 = date.DateId;
                                if (date.HourId == 62) timedate.nuevep15 = date.DateId;
                                if (date.HourId == 63) timedate.nuevep30 = date.DateId;
                                if (date.HourId == 64) timedate.nuevep45 = date.DateId;
                                db.Entry(timedate).State = EntityState.Modified;
                            }
                            db.SaveChanges();
                            ModelState.AddModelError(string.Empty, "La Reunión ha sido Asignada Correctamente!!");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Se ha alcanzado el Límite de Reuniones para esta Hora!!");
                        }

                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException != null &&
                                                                                                        ex.InnerException.InnerException != null &&
                                                                                                        ex.InnerException.InnerException.Message.Contains("_Index"))
                        {

                            ModelState.AddModelError(string.Empty, "Ya existe una Reunión agendada por este Organizador!!");

                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, ex.ToString());
                        }
                    }
                }

                List<SelectListItem> mn = new List<SelectListItem>();
                mn.Add(new SelectListItem { Text = "Seleccione una Hora", Value = "0" });
                mn.Add(new SelectListItem { Text = "06:00 AM", Value = "06:00 AM" });
                mn.Add(new SelectListItem { Text = "06:15 AM", Value = "06:15 AM" });
                mn.Add(new SelectListItem { Text = "06:30 AM", Value = "06:30 AM" });
                mn.Add(new SelectListItem { Text = "06:45 AM", Value = "06:45 AM" });
                mn.Add(new SelectListItem { Text = "07:00 AM", Value = "07:00 AM" });
                mn.Add(new SelectListItem { Text = "07:15 AM", Value = "07:15 AM" });
                mn.Add(new SelectListItem { Text = "07:30 AM", Value = "07:30 AM" });
                mn.Add(new SelectListItem { Text = "07:45 AM", Value = "07:45 AM" });
                mn.Add(new SelectListItem { Text = "08:00 AM", Value = "08:00 AM" });
                mn.Add(new SelectListItem { Text = "08:15 AM", Value = "08:15 AM" });
                mn.Add(new SelectListItem { Text = "08:30 AM", Value = "08:30 AM" });
                mn.Add(new SelectListItem { Text = "08:45 AM", Value = "08:45 AM" });
                mn.Add(new SelectListItem { Text = "09:00 AM", Value = "09:00 AM" });
                mn.Add(new SelectListItem { Text = "09:15 AM", Value = "09:15 AM" });
                mn.Add(new SelectListItem { Text = "09:30 AM", Value = "09:30 AM" });
                mn.Add(new SelectListItem { Text = "09:45 AM", Value = "09:45 AM" });
                mn.Add(new SelectListItem { Text = "10:00 AM", Value = "10:00 AM" });
                mn.Add(new SelectListItem { Text = "10:15 AM", Value = "10:15 AM" });
                mn.Add(new SelectListItem { Text = "10:30 AM", Value = "10:30 AM" });
                mn.Add(new SelectListItem { Text = "10:45 AM", Value = "10:45 AM" });
                mn.Add(new SelectListItem { Text = "11:00 AM", Value = "11:00 AM" });
                mn.Add(new SelectListItem { Text = "11:15 AM", Value = "11:15 AM" });
                mn.Add(new SelectListItem { Text = "11:30 AM", Value = "11:30 AM" });
                mn.Add(new SelectListItem { Text = "11:45 AM", Value = "11:45 AM" });
                mn.Add(new SelectListItem { Text = "12:00 PM", Value = "12:00 PM" });
                mn.Add(new SelectListItem { Text = "12:15 PM", Value = "12:15 PM" });
                mn.Add(new SelectListItem { Text = "12:30 PM", Value = "12:30 PM" });
                mn.Add(new SelectListItem { Text = "12:45 PM", Value = "12:45 PM" });
                mn.Add(new SelectListItem { Text = "01:00 PM", Value = "01:00 PM" });
                mn.Add(new SelectListItem { Text = "01:15 PM", Value = "01:15 PM" });
                mn.Add(new SelectListItem { Text = "01:30 PM", Value = "01:30 PM" });
                mn.Add(new SelectListItem { Text = "01:45 PM", Value = "01:45 PM" });
                mn.Add(new SelectListItem { Text = "02:00 PM", Value = "02:00 PM" });
                mn.Add(new SelectListItem { Text = "02:15 PM", Value = "02:15 PM" });
                mn.Add(new SelectListItem { Text = "02:30 PM", Value = "02:30 PM" });
                mn.Add(new SelectListItem { Text = "02:45 PM", Value = "02:45 PM" });
                mn.Add(new SelectListItem { Text = "03:00 PM", Value = "03:00 PM" });
                mn.Add(new SelectListItem { Text = "03:15 PM", Value = "03:15 PM" });
                mn.Add(new SelectListItem { Text = "03:30 PM", Value = "03:30 PM" });
                mn.Add(new SelectListItem { Text = "03:45 PM", Value = "03:45 PM" });
                mn.Add(new SelectListItem { Text = "04:00 PM", Value = "04:00 PM" });
                mn.Add(new SelectListItem { Text = "04:15 PM", Value = "04:15 PM" });
                mn.Add(new SelectListItem { Text = "04:30 PM", Value = "04:30 PM" });
                mn.Add(new SelectListItem { Text = "04:45 PM", Value = "04:45 PM" });
                mn.Add(new SelectListItem { Text = "05:00 PM", Value = "05:00 PM" });
                mn.Add(new SelectListItem { Text = "05:15 PM", Value = "05:15 PM" });
                mn.Add(new SelectListItem { Text = "05:30 PM", Value = "05:30 PM" });
                mn.Add(new SelectListItem { Text = "05:45 PM", Value = "05:45 PM" });
                mn.Add(new SelectListItem { Text = "06:00 PM", Value = "06:00 PM" });
                mn.Add(new SelectListItem { Text = "06:15 PM", Value = "06:15 PM" });
                mn.Add(new SelectListItem { Text = "06:30 PM", Value = "06:30 PM" });
                mn.Add(new SelectListItem { Text = "06:45 PM", Value = "06:45 PM" });
                mn.Add(new SelectListItem { Text = "07:00 PM", Value = "07:00 PM" });
                mn.Add(new SelectListItem { Text = "07:15 PM", Value = "07:15 PM" });
                mn.Add(new SelectListItem { Text = "07:30 PM", Value = "07:30 PM" });
                mn.Add(new SelectListItem { Text = "07:45 PM", Value = "07:45 PM" });
                mn.Add(new SelectListItem { Text = "08:00 PM", Value = "08:00 PM" });
                mn.Add(new SelectListItem { Text = "08:15 PM", Value = "08:15 PM" });
                mn.Add(new SelectListItem { Text = "08:30 PM", Value = "08:30 PM" });
                mn.Add(new SelectListItem { Text = "08:45 PM", Value = "08:45 PM" });
                mn.Add(new SelectListItem { Text = "09:00 PM", Value = "09:00 PM" });
                mn.Add(new SelectListItem { Text = "09:15 PM", Value = "09:15 PM" });
                mn.Add(new SelectListItem { Text = "09:30 PM", Value = "09:30 PM" });
                mn.Add(new SelectListItem { Text = "09:45 PM", Value = "09:45 PM" });

                ViewData["Hours"] = mn;

                return View(date);
            }
        }

        public ActionResult CreateDates(int? CustomerId, DateTime? EventDate)
        {
            DateTime fecha = Convert.ToDateTime(EventDate);
            ViewData["Fecha"] = fecha.Year + "/" + fecha.Month + "/" + fecha.Day;
            if (CustomerId == null || CustomerId == 0 || EventDate == null)
            {
                return RedirectToAction("Create", "Dates");
            }
           
            //VERIFICAR TIMESDATE
            
            TimesDates dates = new TimesDates();
            dates = db.TimesDates.Where(x => x.ProfessionalId == CustomerId && x.EventDate == EventDate).FirstOrDefault();
            if (dates == null)
            {
                try
                {
                    dates = new TimesDates();
                    dates.ProfessionalId = Convert.ToInt32(CustomerId);
                    dates.EventDate = Convert.ToDateTime(EventDate);
                    db.TimesDates.Add(dates);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw;
                }

            }

            ViewBag.Id = dates.TimesDateId;
            string Telefono = string.Empty;
            var boss = db.Bosses.Where(b => b.Document == CustomerId).FirstOrDefault();
            if (boss != null)
            {
                Telefono = boss.Phone;
            }
            else
            {
                var enlace = db.Links.Where(b => b.Document == CustomerId).FirstOrDefault();
                if (enlace != null)
                {
                    Telefono = enlace.Phone;
                }
                else
                {
                    var coordinador = db.Coordinators.Where(b => b.Document == CustomerId).FirstOrDefault();
                    if (coordinador != null)
                    {
                        Telefono = coordinador.Phone;
                    }
                    else
                    {
                        var lider = db.Leaders.Where(b => b.Document == CustomerId).FirstOrDefault();
                        Telefono = lider.Phone;
                    }
                }

            }

            var date = new Dates
            {
                EventDate = Convert.ToDateTime(EventDate),
                SystemDate = DateTime.Now,
                ProfessionalId = Convert.ToInt32(CustomerId),
                Phone = Telefono,
            };
            List<SelectListItem> mn = new List<SelectListItem>();
            mn.Add(new SelectListItem { Text = "Seleccione una Hora", Value = "0" });
            mn.Add(new SelectListItem { Text = "06:00 AM", Value = "06:00 AM" });
            mn.Add(new SelectListItem { Text = "06:15 AM", Value = "06:15 AM" });
            mn.Add(new SelectListItem { Text = "06:30 AM", Value = "06:30 AM" });
            mn.Add(new SelectListItem { Text = "06:45 AM", Value = "06:45 AM" });
            mn.Add(new SelectListItem { Text = "07:00 AM", Value = "07:00 AM" });
            mn.Add(new SelectListItem { Text = "07:15 AM", Value = "07:15 AM" });
            mn.Add(new SelectListItem { Text = "07:30 AM", Value = "07:30 AM" });
            mn.Add(new SelectListItem { Text = "07:45 AM", Value = "07:45 AM" });
            mn.Add(new SelectListItem { Text = "08:00 AM", Value = "08:00 AM" });
            mn.Add(new SelectListItem { Text = "08:15 AM", Value = "08:15 AM" });
            mn.Add(new SelectListItem { Text = "08:30 AM", Value = "08:30 AM" });
            mn.Add(new SelectListItem { Text = "08:45 AM", Value = "08:45 AM" });
            mn.Add(new SelectListItem { Text = "09:00 AM", Value = "09:00 AM" });
            mn.Add(new SelectListItem { Text = "09:15 AM", Value = "09:15 AM" });
            mn.Add(new SelectListItem { Text = "09:30 AM", Value = "09:30 AM" });
            mn.Add(new SelectListItem { Text = "09:45 AM", Value = "09:45 AM" });
            mn.Add(new SelectListItem { Text = "10:00 AM", Value = "10:00 AM" });
            mn.Add(new SelectListItem { Text = "10:15 AM", Value = "10:15 AM" });
            mn.Add(new SelectListItem { Text = "10:30 AM", Value = "10:30 AM" });
            mn.Add(new SelectListItem { Text = "10:45 AM", Value = "10:45 AM" });
            mn.Add(new SelectListItem { Text = "11:00 AM", Value = "11:00 AM" });
            mn.Add(new SelectListItem { Text = "11:15 AM", Value = "11:15 AM" });
            mn.Add(new SelectListItem { Text = "11:30 AM", Value = "11:30 AM" });
            mn.Add(new SelectListItem { Text = "11:45 AM", Value = "11:45 AM" });
            mn.Add(new SelectListItem { Text = "12:00 PM", Value = "12:00 PM" });
            mn.Add(new SelectListItem { Text = "12:15 PM", Value = "12:15 PM" });
            mn.Add(new SelectListItem { Text = "12:30 PM", Value = "12:30 PM" });
            mn.Add(new SelectListItem { Text = "12:45 PM", Value = "12:45 PM" });
            mn.Add(new SelectListItem { Text = "01:00 PM", Value = "01:00 PM" });
            mn.Add(new SelectListItem { Text = "01:15 PM", Value = "01:15 PM" });
            mn.Add(new SelectListItem { Text = "01:30 PM", Value = "01:30 PM" });
            mn.Add(new SelectListItem { Text = "01:45 PM", Value = "01:45 PM" });
            mn.Add(new SelectListItem { Text = "02:00 PM", Value = "02:00 PM" });
            mn.Add(new SelectListItem { Text = "02:15 PM", Value = "02:15 PM" });
            mn.Add(new SelectListItem { Text = "02:30 PM", Value = "02:30 PM" });
            mn.Add(new SelectListItem { Text = "02:45 PM", Value = "02:45 PM" });
            mn.Add(new SelectListItem { Text = "03:00 PM", Value = "03:00 PM" });
            mn.Add(new SelectListItem { Text = "03:15 PM", Value = "03:15 PM" });
            mn.Add(new SelectListItem { Text = "03:30 PM", Value = "03:30 PM" });
            mn.Add(new SelectListItem { Text = "03:45 PM", Value = "03:45 PM" });
            mn.Add(new SelectListItem { Text = "04:00 PM", Value = "04:00 PM" });
            mn.Add(new SelectListItem { Text = "04:15 PM", Value = "04:15 PM" });
            mn.Add(new SelectListItem { Text = "04:30 PM", Value = "04:30 PM" });
            mn.Add(new SelectListItem { Text = "04:45 PM", Value = "04:45 PM" });
            mn.Add(new SelectListItem { Text = "05:00 PM", Value = "05:00 PM" });
            mn.Add(new SelectListItem { Text = "05:15 PM", Value = "05:15 PM" });
            mn.Add(new SelectListItem { Text = "05:30 PM", Value = "05:30 PM" });
            mn.Add(new SelectListItem { Text = "05:45 PM", Value = "05:45 PM" });
            mn.Add(new SelectListItem { Text = "06:00 PM", Value = "06:00 PM" });
            mn.Add(new SelectListItem { Text = "06:15 PM", Value = "06:15 PM" });
            mn.Add(new SelectListItem { Text = "06:30 PM", Value = "06:30 PM" });
            mn.Add(new SelectListItem { Text = "06:45 PM", Value = "06:45 PM" });
            mn.Add(new SelectListItem { Text = "07:00 PM", Value = "07:00 PM" });
            mn.Add(new SelectListItem { Text = "07:15 PM", Value = "07:15 PM" });
            mn.Add(new SelectListItem { Text = "07:30 PM", Value = "07:30 PM" });
            mn.Add(new SelectListItem { Text = "07:45 PM", Value = "07:45 PM" });
            mn.Add(new SelectListItem { Text = "08:00 PM", Value = "08:00 PM" });
            mn.Add(new SelectListItem { Text = "08:15 PM", Value = "08:15 PM" });
            mn.Add(new SelectListItem { Text = "08:30 PM", Value = "08:30 PM" });
            mn.Add(new SelectListItem { Text = "08:45 PM", Value = "08:45 PM" });
            mn.Add(new SelectListItem { Text = "09:00 PM", Value = "09:00 PM" });
            mn.Add(new SelectListItem { Text = "09:15 PM", Value = "09:15 PM" });
            mn.Add(new SelectListItem { Text = "09:30 PM", Value = "09:30 PM" });
            mn.Add(new SelectListItem { Text = "09:45 PM", Value = "09:45 PM" });

            ViewData["Hours"] = mn;
            return View(date);
        }

        // GET: Dates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dates = db.Dates.Find(id);
            if (dates == null)
            {
                return HttpNotFound();
            }
            return View(dates);
        }

        // POST: Dates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Dates dates, IEnumerable<HttpPostedFileBase> files)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dates).State = EntityState.Modified;
                db.SaveChanges();
                foreach (var archivo in files)
                {
                    if (archivo != null)
                    {
                        var folder = "~/Content/DateFiles";
                        var file = string.Format("{0}_{1}", dates.DateId, archivo.FileName);
                        var response = FilesHelper.UploadPhoto(archivo, folder, file);
                        if (response)
                        {

                            var pic = string.Format("{0}/{1}", folder, file);
                            var Files = new DatesFiles
                            {
                                CompanyId = 1,
                                Path = pic,
                                DateId = dates.DateId
                            };

                            db.DatesFiles.Add(Files);
                            db.SaveChanges();
                        }
                    }
                }
                return RedirectToAction("Index");
            }
            return View(dates);
        }

        // GET: Dates/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dates dates = db.Dates.Find(id);
            if (dates == null)
            {
                return HttpNotFound();
            }
            return View(dates);
        }

        // POST: Dates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Dates dates = db.Dates.Find(id);
            db.Dates.Remove(dates);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult Buscar(string Prefix)
        {
            var jefes = (from voter in db.Bosses
                          where voter.FirstName.Contains(Prefix)
                          select new
                          {
                              label = voter.FirstName + voter.LastName,
                              val = voter.Document
                          }).ToList();
            if(jefes.Count > 0)
            {
                return Json(jefes);
            }
            else
            {
                var coor = (from voter in db.Coordinators
                             where voter.FirstName.Contains(Prefix)
                             select new
                             {
                                 label = voter.FirstName + voter.LastName,
                                 val = voter.Document
                             }).ToList();
                if(coor.Count > 0)
                {
                    return Json(coor);
                }
                else
                {
                    var enlace = (from voter in db.Links
                                  where voter.FirstName.Contains(Prefix)
                                  select new
                                  {
                                      label = voter.FirstName + voter.LastName,
                                      val = voter.Document
                                  }).ToList();
                    if(enlace.Count > 0)
                    {
                        return Json(enlace);
                    }
                    else
                    {
                        var leader = (from voter in db.Leaders
                                      where voter.FirstName.Contains(Prefix)
                                      select new
                                      {
                                          label = voter.FirstName + voter.LastName,
                                          val = voter.Document
                                      }).ToList();
                        return Json(leader);
                    }
                }
            }

            
        }

        public ActionResult DateDelete(int DateId)
        {
            int profid = 0;
            DateTime time = new DateTime();

            Dates date = db.Dates.Find(DateId);
            profid = date.ProfessionalId;
            time = date.EventDate;
            TimesDates timedate = new TimesDates();
            timedate = db.TimesDates.Where(x => x.ProfessionalId == date.ProfessionalId && x.EventDate == date.EventDate).FirstOrDefault();
            if (timedate != null)
            {
                if (date.HourId == 1) timedate.seis00 = 0;
                if (date.HourId == 2) timedate.seis15 = 0;
                if (date.HourId == 3) timedate.seis30 = 0;
                if (date.HourId == 4) timedate.seis45 = 0;
                if (date.HourId == 5) timedate.siete00 = 0;
                if (date.HourId == 6) timedate.siete15 = 0;
                if (date.HourId == 7) timedate.siete30 = 0;
                if (date.HourId == 8) timedate.siete45 = 0;
                if (date.HourId == 9) timedate.ocho00 = 0;
                if (date.HourId == 10) timedate.ocho15 = 0;
                if (date.HourId == 11) timedate.ocho30 = 0;
                if (date.HourId == 12) timedate.ocho45 = 0;
                if (date.HourId == 13) timedate.nueve00 = 0;
                if (date.HourId == 14) timedate.nueve15 = 0;
                if (date.HourId == 15) timedate.nueve30 = 0;
                if (date.HourId == 16) timedate.nueve45 = 0;
                if (date.HourId == 17) timedate.diez00 = 0;
                if (date.HourId == 18) timedate.diez15 = 0;
                if (date.HourId == 19) timedate.diez30 = 0;
                if (date.HourId == 20) timedate.diez45 = 0;
                if (date.HourId == 21) timedate.once00 = 0;
                if (date.HourId == 22) timedate.once15 = 0;
                if (date.HourId == 23) timedate.once30 = 0;
                if (date.HourId == 24) timedate.once45 = 0;
                if (date.HourId == 25) timedate.doce00 = 0;
                if (date.HourId == 26) timedate.doce15 = 0;
                if (date.HourId == 27) timedate.doce30 = 0;
                if (date.HourId == 28) timedate.doce45 = 0;
                if (date.HourId == 29) timedate.uno00 = 0;
                if (date.HourId == 30) timedate.uno15 = 0;
                if (date.HourId == 31) timedate.uno30 = 0;
                if (date.HourId == 32) timedate.uno45 = 0;
                if (date.HourId == 33) timedate.dos00 = 0;
                if (date.HourId == 34) timedate.dos15 = 0;
                if (date.HourId == 35) timedate.dos30 = 0;
                if (date.HourId == 36) timedate.dos45 = 0;
                if (date.HourId == 37) timedate.tres00 = 0;
                if (date.HourId == 38) timedate.tres15 = 0;
                if (date.HourId == 39) timedate.tres30 = 0;
                if (date.HourId == 40) timedate.tres45 = 0;
                if (date.HourId == 41) timedate.cuatro00 = 0;
                if (date.HourId == 42) timedate.cuatro15 = 0;
                if (date.HourId == 43) timedate.cuatro30 = 0;
                if (date.HourId == 44) timedate.cuatro45 = 0;
                if (date.HourId == 45) timedate.cinco00 = 0;
                if (date.HourId == 46) timedate.cinco15 = 0;
                if (date.HourId == 47) timedate.cinco30 = 0;
                if (date.HourId == 48) timedate.cinco45 = 0;
                if (date.HourId == 49) timedate.seisp00 = 0;
                if (date.HourId == 50) timedate.seisp15 = 0;
                if (date.HourId == 51) timedate.seisp30 = 0;
                if (date.HourId == 52) timedate.seisp45 = 0;
                if (date.HourId == 53) timedate.sietep00 = 0;
                if (date.HourId == 54) timedate.sietep15 = 0;
                if (date.HourId == 55) timedate.sietep30 = 0;
                if (date.HourId == 56) timedate.sietp45 = 0;
                if (date.HourId == 57) timedate.ochop00 = 0;
                if (date.HourId == 58) timedate.ochop15 = 0;
                if (date.HourId == 59) timedate.ochop30 = 0;
                if (date.HourId == 60) timedate.ochop45 = 0;
                if (date.HourId == 61) timedate.nuevep00 = 0;
                if (date.HourId == 62) timedate.nuevep15 = 0;
                if (date.HourId == 63) timedate.nuevep30 = 0;
                if (date.HourId == 64) timedate.nuevep45 = 0;
                db.Entry(timedate).State = EntityState.Modified;
            }
            db.Dates.Remove(date);
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null &&
                                                                    ex.InnerException.InnerException != null &&
                                                                    ex.InnerException.InnerException.Message.Contains("REFERENCE"))
                {
                    ModelState.AddModelError(string.Empty, "La reunión no pudo ser Borrada");

                }
                else
                {
                    ModelState.AddModelError(string.Empty, ex.ToString());
                }
            }
            return RedirectToAction("CreateDates", "Dates", new { ProfessionalId = profid, EventDate = time });
        }

        public ActionResult Check(int DateId)
        {
            Dates date = db.Dates.Find(DateId);
            date.Asistencia = true;
            db.Entry(date).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("CreateDates", "Dates", new { ProfessionalId = date.ProfessionalId, EventDate = date.EventDate });
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
