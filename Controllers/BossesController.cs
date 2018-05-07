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
    [Authorize(Roles = "User, Digitador, Secretario")]
    public class BossesController : Controller
    {
        private WebApiPruebaContext db = new WebApiPruebaContext();

        // GET: Bosses
        public ActionResult Index(string Document, string CustomerId, int? FilterId, int? Comuna, int? VotingPlaceId, int? WorkPlaceId, int? page = null)
        {
            ViewBag.uno = FilterId;
            ViewBag.dos = Comuna;
            ViewBag.tres = VotingPlaceId;
            ViewBag.cuatro = WorkPlaceId;
            page = (page ?? 1);
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (!string.IsNullOrEmpty(Document))
            {
                if (Utilidades.isNumeric(Document))
                {
                    ViewBag.FilterId = new SelectList(
                    CombosHelper.GetFilters(),
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
                    double doc = Convert.ToDouble(Document);
                    var boss = db.Bosses.Where(v => v.Document == doc);
                    return View(boss.OrderBy(b => b.CompanyId).ThenBy(b => b.FirstName).ToPagedList((int)page, 5));
                }
            }

            if (!string.IsNullOrEmpty(CustomerId))
            {
                if (Utilidades.isNumeric(CustomerId))
                {
                    ViewBag.FilterId = new SelectList(
                    CombosHelper.GetFilters(),
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
                    int doc = Convert.ToInt32(CustomerId);
                    var boss = db.Bosses.Where(b => b.BossId == doc);
                    return View(boss.OrderBy(b => b.CompanyId).ThenBy(b => b.FirstName).ToPagedList((int)page, 5));
                }
            }
            
            if (FilterId > 0)
            {
                if (FilterId == 1)//GENERAL
                {
                    ViewBag.FilterId = new SelectList(
                    CombosHelper.GetFilters(),
                    "FiltersId",
                    "name",
                    FilterId);

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

                    var bossGeneral = db.Bosses.Where(b => b.CompanyId == user.CompanyId);
                    return View(bossGeneral.OrderBy(b => b.CompanyId).ThenBy(b => b.FirstName).ToPagedList((int)page, 5));
                }
                if (FilterId == 2)//COMUNA
                {
                    if (Comuna > 0)
                    {
                        ViewBag.FilterId = new SelectList(
                        CombosHelper.GetFilters(),
                        "FiltersId",
                        "name",
                        FilterId);

                        ViewBag.Comuna = new SelectList(
                        CombosHelper.GetCommunes(),
                        "CommuneId",
                        "Name",
                        Comuna);

                        ViewBag.VotingPlaceId = new SelectList(
                        CombosHelper.GetVotingPlaces(),
                        "VotingPlaceId",
                        "Name");

                        ViewBag.WorkPlaceId = new SelectList(
                        CombosHelper.GetWorkPlaces(),
                        "WorkPlaceId",
                        "Name");

                        var bossComuna = db.Bosses.Where(b => b.CompanyId == user.CompanyId && b.CommuneId == Comuna);
                        return View(bossComuna.OrderBy(b => b.CompanyId).ThenBy(b => b.FirstName).ToPagedList((int)page, 10));
                    }
                }
                if (FilterId == 3)//LUGAR DE VOTACION
                {
                    if (VotingPlaceId > 0)
                    {
                        ViewBag.FilterId = new SelectList(
                        CombosHelper.GetFilters(),
                        "FiltersId",
                        "name",
                        FilterId);

                        ViewBag.Comuna = new SelectList(
                        CombosHelper.GetCommunes(),
                        "CommuneId",
                        "Name");

                        ViewBag.VotingPlaceId = new SelectList(
                        CombosHelper.GetVotingPlaces(),
                        "VotingPlaceId",
                        "Name",
                        VotingPlaceId);

                        ViewBag.WorkPlaceId = new SelectList(
                        CombosHelper.GetWorkPlaces(),
                        "WorkPlaceId",
                        "Name");

                        var bossVotacion = db.Bosses.Where(b => b.CompanyId == user.CompanyId && b.VotingPlaceId == VotingPlaceId);
                        return View(bossVotacion.OrderBy(b => b.CompanyId).ThenBy(b => b.FirstName).ToPagedList((int)page, 10));
                    }
                }
                if(FilterId == 4)
                {
                    if(WorkPlaceId > 0)
                    {
                        ViewBag.FilterId = new SelectList(
                        CombosHelper.GetFilters(),
                        "FiltersId",
                        "name",
                        FilterId);

                        ViewBag.Comuna = new SelectList(
                        CombosHelper.GetCommunes(),
                        "CommuneId",
                        "Name");

                        ViewBag.VotingPlaceId = new SelectList(
                        CombosHelper.GetVotingPlaces(),
                        "VotingPlaceId",
                        "Name");

                        ViewBag.WorkPlaceId = new SelectList(
                        CombosHelper.GetWorkPlaces(),
                        "WorkPlaceId",
                        "Name",
                        WorkPlaceId);

                        var bossWork = db.Bosses.Where(b => b.CompanyId == user.CompanyId && b.WorkPlaceId == WorkPlaceId);
                        return View(bossWork.OrderBy(b => b.CompanyId).ThenBy(b => b.FirstName).ToPagedList((int)page, 10));
                    }
                }
            }
            ViewBag.FilterId = new SelectList(
                    CombosHelper.GetFilters(),
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
            var bosses = db.Bosses.Where(b => b.CompanyId == user.CompanyId);
            return View(bosses.OrderBy(b => b.CompanyId).ThenBy(b => b.FirstName).ToPagedList((int)page, 10));
        }

        public ActionResult BossesReport(int? filtro, int? communa, int? votacion, int? work, int? type)
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Index", "Bosses");
            }

            var inc = new object();
            inc = (from b in db.Bosses
                   join c in db.Companies on b.CompanyId equals c.CompanyId
                   join v in db.VotingPlaces on b.VotingPlaceId equals v.VotingPlaceId
                   where b.CompanyId == user.CompanyId
                   orderby b.FirstName
                   select new { c.Name, c.Logo, c.Phone, c.Address, b.FirstName, b.LastName, b.Document, BossAddress = b.Address, b.UserName, b.CommuneId, Telefono = b.Phone, b.Profesion, VotingName = v.Name }).ToList();

            if (filtro > 0)
            {
                if (filtro == 1)//GENERAL
                {
                    inc = (from b in db.Bosses
                           join c in db.Companies on b.CompanyId equals c.CompanyId
                           join v in db.VotingPlaces on b.VotingPlaceId equals v.VotingPlaceId
                           where b.CompanyId == user.CompanyId
                           orderby b.FirstName
                           select new { c.Name, c.Logo, c.Phone, c.Address, b.FirstName, b.LastName, b.Document, BossAddress = b.Address, b.UserName, b.CommuneId, Telefono = b.Phone, b.Profesion, VotingName = v.Name }).ToList();

                }
                if (filtro == 2)//COMUNA
                {
                    if (communa > 0)
                    {
                        inc = (from b in db.Bosses
                               join c in db.Companies on b.CompanyId equals c.CompanyId
                               join v in db.VotingPlaces on b.VotingPlaceId equals v.VotingPlaceId
                               where b.CompanyId == user.CompanyId && b.CommuneId == communa
                               orderby b.FirstName
                               select new { c.Name, c.Logo, c.Phone, c.Address, b.FirstName, b.LastName, b.Document, BossAddress = b.Address, b.UserName, Telefono = b.Phone, b.CommuneId, b.Profesion, VotingName = v.Name }).ToList();

                    }
                }
                if (filtro == 3)//LUGAR DE VOTACION
                {
                    if (votacion > 0)
                    {
                        inc = (from b in db.Bosses
                               join c in db.Companies on b.CompanyId equals c.CompanyId
                               join v in db.VotingPlaces on b.VotingPlaceId equals v.VotingPlaceId
                               where b.CompanyId == user.CompanyId && b.VotingPlaceId == votacion
                               orderby b.FirstName
                               select new { c.Name, c.Logo, c.Phone, c.Address, b.FirstName, b.LastName, b.Document, BossAddress = b.Address, b.UserName, Telefono = b.Phone, b.CommuneId, b.Profesion, VotingName = v.Name }).ToList();

                    }
                }

                if (filtro == 4)//LUGAR DE TRABAJO
                {
                    if (work > 0)
                    {
                        inc = (from b in db.Bosses
                               join c in db.Companies on b.CompanyId equals c.CompanyId
                               join v in db.VotingPlaces on b.VotingPlaceId equals v.VotingPlaceId
                               where b.CompanyId == user.CompanyId && b.WorkPlaceId == work
                               orderby b.FirstName
                               select new { c.Name, c.Logo, c.Phone, c.Address, b.FirstName, b.LastName, b.Document, BossAddress = b.Address, b.UserName, Telefono = b.Phone, b.CommuneId, b.Profesion, VotingName = v.Name }).ToList();

                    }
                }
            }         




            string path = Path.Combine(Server.MapPath("~/Reports"), "Bosses.rdlc");

            LocalReport lr = new LocalReport();
            lr.EnableExternalImages = true;
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return RedirectToAction("Index", "Bosses");
            }

            var company = db.Companies.Find(user.CompanyId);

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
            string reportType = "";
            if(type == 1)
            {
                reportType = "EXCELOPENXML";
            }
            else
            {
                reportType = "PDF";
            }
            string mimeType;
            string encoding;
            string fileNameExtension;



            string deviceInfo =

            "<DeviceInfo>" +
            "  <OutputFormat>" + user.CompanyId + "</OutputFormat>" +
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


        // GET: Bosses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var boss = db.Bosses.Find(id);
            if (boss == null)
            {
                return HttpNotFound();
            }
            return View(boss);
        }

        // GET: Bosses/Create
        public ActionResult Create()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.CountryId = new SelectList(
               CombosHelper.GetCountries(),
               "CountryId",
               "Name");

            ViewBag.CityId = new SelectList(
                CombosHelper.GetCities(),
                "CityId",
                "Name");

            ViewBag.CompanyId = new SelectList(
                CombosHelper.GetCompanies(),
                "CompanyId",
                "Name");

            ViewBag.DepartmentId = new SelectList(
                CombosHelper.GetDepartments(),
                "DepartmentId",
                "Name");

            ViewBag.CommuneId = new SelectList(
                CombosHelper.GetCommunes(),
                "CommuneId",
                "Name");

            ViewBag.WorkPlaceId = new SelectList(
               CombosHelper.GetWorkPlaces(),
               "WorkPlaceId",
               "Name");

            ViewBag.VotingPlaceId = new SelectList(
                CombosHelper.GetVotingPlaces(),
                "VotingPlaceId",
                "Name");



            var boss = new Boss
            {
                CompanyId = user.CompanyId,
                Date = DateTime.Now,
                DateBorn = DateTime.Now,
            };
            return View(boss);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Boss boss, string newWorkPlace)
        {
            if (ModelState.IsValid)
            {
                if (boss.WorkPlaceId == 9999)
                {
                    if (!string.IsNullOrEmpty(newWorkPlace))
                    {
                        var workPlace = new WorkPlace
                        {
                            Name = newWorkPlace,
                        };
                        db.WorkPlaces.Add(workPlace);
                        try
                        {
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            if (ex.InnerException != null &&
                                                                                        ex.InnerException.InnerException != null &&
                                                                                        ex.InnerException.InnerException.Message.Contains("_Index"))
                            {
                                ModelState.AddModelError(string.Empty, "Ya existe un lugar de trabajo con ese nombre");

                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, ex.ToString());
                            }
                            ViewBag.CountryId = new SelectList(
                               CombosHelper.GetCountries(),
                               "CountryId",
                               "Name",
                               boss.CountryId);

                            ViewBag.CityId = new SelectList(
                                CombosHelper.GetCities(),
                                "CityId",
                                "Name",
                                boss.CityId);

                            ViewBag.CompanyId = new SelectList(
                                CombosHelper.GetCompanies(),
                                "CompanyId",
                                "Name",
                                boss.CompanyId);

                            ViewBag.DepartmentId = new SelectList(
                                CombosHelper.GetDepartments(),
                                "DepartmentId",
                                "Name",
                                boss.DepartmentId);

                            ViewBag.CommuneId = new SelectList(
                                CombosHelper.GetCommunes(),
                                "CommuneId",
                                "Name",
                                boss.CommuneId);

                            ViewBag.WorkPlaceId = new SelectList(
                              CombosHelper.GetWorkPlaces(),
                              "WorkPlaceId",
                              "Name",
                              boss.WorkPlaceId);

                            ViewBag.VotingPlaceId = new SelectList(
                                CombosHelper.GetVotingPlaces(),
                                "VotingPlaceId",
                                "Name",
                                boss.VotingPlaceId);
                            return View(boss);
                        }
                        db.SaveChanges();
                        boss.WorkPlaceId = workPlace.WorkPlaceId;
                    }
                }
                db.Bosses.Add(boss);
                try
                {
                    db.SaveChanges();
                    //UsersHelper.CreateUserASP(boss.UserName, "Boss");
                    if (boss.PhotoFile != null)
                    {
                        var folder = "~/Content/Bosses";
                        var file = string.Format("{0}.jpg", boss.BossId);
                        var response = FilesHelper.UploadPhoto(boss.PhotoFile, folder, file);
                        if (response)
                        {
                            var pic = string.Format("{0}/{1}", folder, file);
                            boss.Photo = pic;
                            db.Entry(boss).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }

                    var Refer = new Refer
                    {
                        ReferType = 1,
                        UserId = boss.BossId,
                        FullName = boss.FullName,
                        Active = 1
                    };
                    db.Refers.Add(Refer);
                    db.SaveChanges();

                    var city = db.Cities.Find(boss.CityId);
                    var department = db.Departments.Find(boss.DepartmentId);
                    var country = db.Countries.Find(boss.CountryId);
                    var comune = db.Communes.Find(boss.CommuneId);
                    var votingPlace = db.VotingPlaces.Find(boss.VotingPlaceId);
                    var Voter = new Voter
                    {
                        Address = boss.Address,
                        BossId = boss.BossId,
                        CityId = city.Name,
                        CommuneId = comune.Name,
                        CompanyId = boss.CompanyId,
                        CountryId = country.Name,
                        DepartmentId = department.Name,
                        Document = boss.Document,
                        FirstName = boss.FirstName,
                        LastName = boss.LastName,
                        Phone = boss.Phone,
                        UserName = boss.UserName,
                        VotingPlaceId = votingPlace.Name,
                        userId = 1,
                        PerfilId = 1,
                        ReferId = Refer.ReferId,
                        Fname = string.Format("{0} {1}", boss.FirstName, boss.LastName),
                        Profesion = boss.Profesion,
                        Barrio = boss.Barrio,
                        DateBorn = boss.DateBorn,

                    };

                    db.Voters.Add(Voter);
                    db.SaveChanges();


                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null &&
                                                            ex.InnerException.InnerException != null &&
                                                            ex.InnerException.InnerException.Message.Contains("_Index"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe un Registro con esa descripción");

                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.ToString());
                    }
                }
            }

            ViewBag.CountryId = new SelectList(
               CombosHelper.GetCountries(),
               "CountryId",
               "Name",
               boss.CountryId);

            ViewBag.CityId = new SelectList(
                CombosHelper.GetCities(),
                "CityId",
                "Name",
                boss.CityId);

            ViewBag.CompanyId = new SelectList(
                CombosHelper.GetCompanies(),
                "CompanyId",
                "Name",
                boss.CompanyId);

            ViewBag.DepartmentId = new SelectList(
                CombosHelper.GetDepartments(),
                "DepartmentId",
                "Name",
                boss.DepartmentId);

            ViewBag.CommuneId = new SelectList(
                CombosHelper.GetCommunes(),
                "CommuneId",
                "Name",
                boss.CommuneId);

            ViewBag.WorkPlaceId = new SelectList(
              CombosHelper.GetWorkPlaces(),
              "WorkPlaceId",
              "Name",
              boss.WorkPlaceId);

            ViewBag.VotingPlaceId = new SelectList(
                CombosHelper.GetVotingPlaces(),
                "VotingPlaceId",
                "Name",
                boss.VotingPlaceId);
            return View(boss);
        }

        // GET: Bosses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var boss = db.Bosses.Find(id);
            if (boss == null)
            {
                return HttpNotFound();
            }
            ViewBag.CountryId = new SelectList(
               CombosHelper.GetCountries(),
               "CountryId",
               "Name",
               boss.CountryId);

            ViewBag.CityId = new SelectList(
                CombosHelper.GetCities(),
                "CityId",
                "Name",
                boss.CityId);

            ViewBag.CompanyId = new SelectList(
                CombosHelper.GetCompanies(),
                "CompanyId",
                "Name",
                boss.CompanyId);

            ViewBag.DepartmentId = new SelectList(
                CombosHelper.GetDepartments(),
                "DepartmentId",
                "Name",
                boss.DepartmentId);

            ViewBag.CommuneId = new SelectList(
                CombosHelper.GetCommunes(),
                "CommuneId",
                "Name",
                boss.CommuneId);

            ViewBag.WorkPlaceId = new SelectList(
              CombosHelper.GetWorkPlaces(),
              "WorkPlaceId",
              "Name",
              boss.WorkPlaceId);

            ViewBag.VotingPlaceId = new SelectList(
                CombosHelper.GetVotingPlaces(),
                "VotingPlaceId",
                "Name",
                boss.VotingPlaceId);
            return View(boss);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Boss boss, string newWorkPlace)
        {
            if (ModelState.IsValid)
            {
                if (boss.WorkPlaceId == 9999)
                {
                    if (!string.IsNullOrEmpty(newWorkPlace))
                    {
                        var workPlace = new WorkPlace
                        {
                            Name = newWorkPlace,
                        };
                        db.WorkPlaces.Add(workPlace);
                        try
                        {
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            if (ex.InnerException != null &&
                                                                                        ex.InnerException.InnerException != null &&
                                                                                        ex.InnerException.InnerException.Message.Contains("_Index"))
                            {
                                ModelState.AddModelError(string.Empty, "Ya existe un lugar de trabajo con ese nombre");

                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, ex.ToString());
                            }
                            ViewBag.CountryId = new SelectList(
                               CombosHelper.GetCountries(),
                               "CountryId",
                               "Name",
                               boss.CountryId);

                            ViewBag.CityId = new SelectList(
                                CombosHelper.GetCities(),
                                "CityId",
                                "Name",
                                boss.CityId);

                            ViewBag.CompanyId = new SelectList(
                                CombosHelper.GetCompanies(),
                                "CompanyId",
                                "Name",
                                boss.CompanyId);

                            ViewBag.DepartmentId = new SelectList(
                                CombosHelper.GetDepartments(),
                                "DepartmentId",
                                "Name",
                                boss.DepartmentId);

                            ViewBag.CommuneId = new SelectList(
                                CombosHelper.GetCommunes(),
                                "CommuneId",
                                "Name",
                                boss.CommuneId);

                            ViewBag.WorkPlaceId = new SelectList(
                              CombosHelper.GetWorkPlaces(),
                              "WorkPlaceId",
                              "Name",
                              boss.WorkPlaceId);

                            ViewBag.VotingPlaceId = new SelectList(
                                CombosHelper.GetVotingPlaces(),
                                "VotingPlaceId",
                                "Name",
                                boss.VotingPlaceId);
                            return View(boss);
                        }
                        boss.WorkPlaceId = workPlace.WorkPlaceId;
                    }
                }
                if (boss.PhotoFile != null)
                {
                    var folder = "~/Content/Bosses";
                    var file = string.Format("{0}.jpg", boss.BossId);
                    var response = FilesHelper.UploadPhoto(boss.PhotoFile, folder, file);
                    if (response)
                    {
                        var pic = string.Format("{0}/{1}", folder, file);
                        boss.Photo = pic;
                        db.Entry(boss).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }


                db.Entry(boss).State = EntityState.Modified;
                try
                {
                    var refer = db.Refers.Where(r => r.ReferType == 1 && r.UserId == boss.BossId).FirstOrDefault();

                    var voter = db.Voters.Where(v => v.Document == boss.Document).FirstOrDefault();
                    var country = db.Countries.Find(boss.CountryId);
                    var department = db.Departments.Find(boss.DepartmentId);
                    var city = db.Cities.Find(boss.CityId);
                    var commune = db.Communes.Find(boss.CommuneId);
                    var voting = db.VotingPlaces.Find(boss.VotingPlaceId);

                    voter.Address = boss.Address;
                    voter.BossId = boss.BossId;
                    voter.CityId = city.Name;
                    voter.CommuneId = commune.Name;
                    voter.CompanyId = boss.CompanyId;
                    voter.CountryId = country.Name;
                    voter.DepartmentId = department.Name;
                    voter.Document = boss.Document;
                    voter.FirstName = boss.FirstName;
                    voter.LastName = boss.LastName;
                    voter.Phone = boss.Phone;
                    voter.UserName = boss.UserName;
                    voter.VotingPlaceId = voting.Name;
                    voter.userId = 1;
                    voter.ReferId = refer.ReferId;
                    voter.PerfilId = 1;
                    voter.Fname = string.Format("{0} {1}", boss.FirstName, boss.LastName);
                    voter.Barrio = boss.Barrio;
                    voter.DateBorn = boss.DateBorn;
                    voter.Profesion = boss.Profesion;
                    db.Entry(voter).State = EntityState.Modified;

                    refer.FullName = boss.FullName;
                    db.Entry(refer).State = EntityState.Modified;

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null &&
                                                            ex.InnerException.InnerException != null &&
                                                            ex.InnerException.InnerException.Message.Contains("_Index"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe un Registro con esa descripción");

                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.ToString());
                    }
                }
            }
            ViewBag.CountryId = new SelectList(
              CombosHelper.GetCountries(),
              "CountryId",
              "Name",
              boss.CountryId);

            ViewBag.CityId = new SelectList(
                CombosHelper.GetCities(),
                "CityId",
                "Name",
                boss.CityId);

            ViewBag.CompanyId = new SelectList(
                CombosHelper.GetCompanies(),
                "CompanyId",
                "Name",
                boss.CompanyId);

            ViewBag.DepartmentId = new SelectList(
                CombosHelper.GetDepartments(),
                "DepartmentId",
                "Name",
                boss.DepartmentId);

            ViewBag.CommuneId = new SelectList(
                CombosHelper.GetCommunes(),
                "CommuneId",
                "Name",
                boss.CommuneId);

            ViewBag.WorkPlaceId = new SelectList(
              CombosHelper.GetWorkPlaces(),
              "WorkPlaceId",
              "Name",
              boss.WorkPlaceId);

            ViewBag.VotingPlaceId = new SelectList(
                CombosHelper.GetVotingPlaces(),
                "VotingPlaceId",
                "Name",
                boss.VotingPlaceId);
            return View(boss);
        }

        // GET: Bosses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var boss = db.Bosses.Find(id);
            if (boss == null)
            {
                return HttpNotFound();
            }
            return View(boss);
        }

        // POST: Bosses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var boss = db.Bosses.Find(id);
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var response = FilesHelper.DeleteDocument(boss.Photo);
                    var Voter = db.Voters.Where(V => V.Document == boss.Document).FirstOrDefault();
                    if (Voter != null)
                    {
                        db.Voters.Remove(Voter);
                    }
                    //Elimino cualquier Enlace Coordinador o Lider asociado a ese Jefe
                    var Refer = db.Refers.Where(r => r.ReferType == 1 && r.UserId == boss.BossId).FirstOrDefault();
                    if (Refer != null)
                    {
                        var links = db.Links.Where(l => l.BossId == Refer.ReferId).ToList();
                        foreach (var link in links)
                        {
                            db.Links.Remove(link);
                        }

                        var coordinators = db.Coordinators.Where(c => c.ReferId == Refer.ReferId).ToList();
                        foreach (var coor in coordinators)
                        {
                            db.Coordinators.Remove(coor);
                        }
                        var leaders = db.Leaders.Where(l => l.ReferId == Refer.ReferId).ToList();
                        foreach (var leader in leaders)
                        {
                            db.Leaders.Remove(leader);
                        }
                        var voters = db.Voters.Where(v => v.ReferId == Refer.ReferId).ToList();
                        foreach (var voter in voters)
                        {
                            db.Voters.Remove(voter);
                        }
                        db.Refers.Remove(Refer);
                        db.SaveChanges();
                    }                    
                    
                    //Borro cualquier anotacion que tenga en la agenda
                    var dates = db.Dates.Where(d => d.ProfessionalId == boss.Document).ToList();
                    foreach(var item in dates)
                    {
                        db.Dates.Remove(item);
                    }

                    var dateItems = db.TimesDates.Where(di => di.ProfessionalId == boss.Document).ToList();
                    foreach(var it in dateItems)
                    {
                        db.TimesDates.Remove(it);
                    }
                    var HV = db.HojaVidas.Where(h => h.RolId == 1 && h.UserId == boss.BossId).FirstOrDefault();
                    if (HV != null)
                    {
                        db.HojaVidas.Remove(HV);
                    }
                    db.Bosses.Remove(boss);
                    db.SaveChanges();
                    transaction.Commit();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    if (ex.InnerException != null &&
                                                        ex.InnerException.InnerException != null &&
                                                        ex.InnerException.InnerException.Message.Contains("REFERENCE"))
                    {
                        ModelState.AddModelError(string.Empty, "El registro no se puede borrar porque tiene registros relacionados");

                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, ex.ToString());
                    }
                }
            }
            ViewBag.CountryId = new SelectList(
              CombosHelper.GetCountries(),
              "CountryId",
              "Name",
              boss.CountryId);

            ViewBag.CityId = new SelectList(
                CombosHelper.GetCities(),
                "CityId",
                "Name",
                boss.CityId);

            ViewBag.CompanyId = new SelectList(
                CombosHelper.GetCompanies(),
                "CompanyId",
                "Name",
                boss.CompanyId);

            ViewBag.DepartmentId = new SelectList(
                CombosHelper.GetDepartments(),
                "DepartmentId",
                "Name",
                boss.DepartmentId);

            ViewBag.CommuneId = new SelectList(
                CombosHelper.GetCommunes(),
                "CommuneId",
                "Name",
                boss.CommuneId);

            ViewBag.WorkPlaceId = new SelectList(
             CombosHelper.GetWorkPlaces(),
             "WorkPlaceId",
             "Name",
             boss.WorkPlaceId);

            ViewBag.VotingPlaceId = new SelectList(
                CombosHelper.GetVotingPlaces(),
                "VotingPlaceId",
                "Name",
                boss.VotingPlaceId);
            return View(boss);
        }

        public JsonResult GetDepartments(int countryId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var department = db.Departments.Where(m => m.CountryId == countryId);
            return Json(department);
        }

        public JsonResult GetCities(int departmentId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var cities = db.Cities.Where(m => m.DepartmentId == departmentId);
            return Json(cities);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult CargarHojaVida(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var boss = db.Bosses.Find(id);
            if (boss == null)
            {
                return HttpNotFound();
            }

            return View(boss);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CargarHojaVida(int BossId, HttpPostedFileBase File)
        {
            if (File != null)
            {
                var folder = "~/Content/HojasVida";
                var file = string.Format("{0}_{1}_{2}", 1, BossId, File.FileName);
                var response = FilesHelper.UploadPhoto(File, folder, file);
                if (response)
                {
                    var pic = string.Format("{0}/{1}", folder, file);

                    //Elimino la actual HV
                    var hoja = db.HojaVidas.Where(h => h.RolId == 1 && h.UserId == BossId).FirstOrDefault();
                    if (hoja != null)
                    {
                        var resp = FilesHelper.DeleteDocument(hoja.Path);
                        db.HojaVidas.Remove(hoja);
                        db.SaveChanges();
                    }
                    var HV = new HojaVida
                    {
                        RolId = 1,
                        UserId = BossId,
                        Path = pic,
                    };
                    db.HojaVidas.Add(HV);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }
    }
}
