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
    public class LeadersController : Controller
    {
        private WebApiPruebaContext db = new WebApiPruebaContext();

        // GET: Leaders
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
                    var lid = db.Leaders.Where(v => v.Document == doc);
                    return View(lid.OrderBy(b => b.CompanyId).ThenBy(b => b.FirstName).ToPagedList((int)page, 5));
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
                    var lid = db.Leaders.Where(b => b.LeaderId == doc);
                    return View(lid.OrderBy(b => b.CompanyId).ThenBy(b => b.FirstName).ToPagedList((int)page, 5));
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

                    ViewBag.Comuna = new SelectList(
                            CombosHelper.GetCommunes(),
                            "CommuneId",
                            "Name");

                    ViewBag.WorkPlaceId = new SelectList(
                      CombosHelper.GetWorkPlaces(),
                      "WorkPlaceId",
                      "Name");

                    var liderGeneral = db.Leaders.Where(b => b.CompanyId == user.CompanyId);
                    return View(liderGeneral.OrderBy(b => b.CompanyId).ThenBy(b => b.FirstName).ToPagedList((int)page, 5));
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

                        var liderComuna = db.Leaders.Where(b => b.CompanyId == user.CompanyId && b.CommuneId == Comuna);
                        return View(liderComuna.OrderBy(b => b.CompanyId).ThenBy(b => b.FirstName).ToPagedList((int)page, 10));
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

                        var liderVotacion = db.Leaders.Where(b => b.CompanyId == user.CompanyId && b.VotingPlaceId == VotingPlaceId);
                        return View(liderVotacion.OrderBy(b => b.CompanyId).ThenBy(b => b.FirstName).ToPagedList((int)page, 10));
                    }
                }

                if (FilterId == 4)
                {
                    if (WorkPlaceId > 0)
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

                        var liderWork = db.Leaders.Where(b => b.CompanyId == user.CompanyId && b.WorkPlaceId == WorkPlaceId);
                        return View(liderWork.OrderBy(b => b.CompanyId).ThenBy(b => b.FirstName).ToPagedList((int)page, 10));
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
            var lider = db.Leaders.Where(b => b.CompanyId == user.CompanyId);
            return View(lider.OrderBy(b => b.CompanyId).ThenBy(b => b.FirstName).ToPagedList((int)page, 10));
        }

        public ActionResult LeaderReport(int? filtro, int? communa, int? votacion, int? work, int? type)
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Index", "Leaders");
            }

            var inc = new object();
            inc = (from b in db.Leaders
                   join c in db.Companies on b.CompanyId equals c.CompanyId
                   join v in db.VotingPlaces on b.VotingPlaceId equals v.VotingPlaceId
                   join r in db.Refers on b.ReferId equals r.ReferId
                   where b.CompanyId == user.CompanyId
                   orderby b.FirstName
                   select new { c.Name, c.Logo, c.Phone, c.Address, b.FirstName, b.LastName, b.Document, BossAddress = b.Address, Telefono = b.Phone, b.UserName, b.CommuneId, b.Profesion, VotingName = v.Name, referido = r.FullName }).ToList();

            if (filtro > 0)
            {
                if (filtro == 1)//GENERAL
                {
                    inc = (from b in db.Leaders
                           join c in db.Companies on b.CompanyId equals c.CompanyId
                           join v in db.VotingPlaces on b.VotingPlaceId equals v.VotingPlaceId
                           join r in db.Refers on b.ReferId equals r.ReferId
                           where b.CompanyId == user.CompanyId
                           orderby b.FirstName
                           select new { c.Name, c.Logo, c.Phone, c.Address, b.FirstName, b.LastName, b.Document, Telefono = b.Phone, BossAddress = b.Address, b.UserName, b.CommuneId, b.Profesion, VotingName = v.Name, referido = r.FullName }).ToList();

                }
                if (filtro == 2)//COMUNA
                {
                    if (communa > 0)
                    {
                        inc = (from b in db.Leaders
                               join c in db.Companies on b.CompanyId equals c.CompanyId
                               join v in db.VotingPlaces on b.VotingPlaceId equals v.VotingPlaceId
                               join r in db.Refers on b.ReferId equals r.ReferId
                               where b.CompanyId == user.CompanyId && b.CommuneId == communa
                               orderby b.FirstName
                               select new { c.Name, c.Logo, c.Phone, c.Address, b.FirstName, b.LastName, b.Document, Telefono = b.Phone, BossAddress = b.Address, b.UserName, b.CommuneId, b.Profesion, VotingName = v.Name, referido = r.FullName }).ToList();

                    }
                }
                if (filtro == 3)//LUGAR DE VOTACION
                {
                    if (votacion > 0)
                    {
                        inc = (from b in db.Leaders
                               join c in db.Companies on b.CompanyId equals c.CompanyId
                               join v in db.VotingPlaces on b.VotingPlaceId equals v.VotingPlaceId
                               join r in db.Refers on b.ReferId equals r.ReferId
                               where b.CompanyId == user.CompanyId && b.VotingPlaceId == votacion
                               orderby b.FirstName
                               select new { c.Name, c.Logo, c.Phone, c.Address, b.FirstName, b.LastName, b.Document, Telefono = b.Phone, BossAddress = b.Address, b.UserName, b.CommuneId, b.Profesion, VotingName = v.Name, referido = r.FullName }).ToList();

                    }
                }

                if (filtro == 4)//LUGAR DE TRABAJO
                {
                    if (work > 0)
                    {
                        inc = (from b in db.Leaders
                               join c in db.Companies on b.CompanyId equals c.CompanyId
                               join v in db.VotingPlaces on b.VotingPlaceId equals v.VotingPlaceId
                               join r in db.Refers on b.ReferId equals r.ReferId
                               where b.CompanyId == user.CompanyId && b.WorkPlaceId == work
                               orderby b.FirstName
                               select new { c.Name, c.Logo, c.Phone, c.Address, b.FirstName, b.LastName, b.Document, BossAddress = b.Address, b.UserName, Telefono = b.Phone, b.CommuneId, b.Profesion, VotingName = v.Name, referido = r.FullName }).ToList();

                    }
                }
            }
           




            string path = Path.Combine(Server.MapPath("~/Reports"), "Leaders.rdlc");

            LocalReport lr = new LocalReport();
            lr.EnableExternalImages = true;
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return RedirectToAction("Index", "Leaders");
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
            if (type == 1)
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


        // GET: Leaders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var leader = db.Leaders.Find(id);
            if (leader == null)
            {
                return HttpNotFound();
            }
            return View(leader);
        }

        // GET: Leaders/Create
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

            ViewBag.AssociationId = new SelectList(
                CombosHelper.GetAssociations(),
                "AssociationId",
                "Name");

            ViewBag.WorkPlaceId = new SelectList(
               CombosHelper.GetWorkPlaces(),
               "WorkPlaceId",
               "Name");

            ViewBag.VotingPlaceId = new SelectList(
                CombosHelper.GetVotingPlaces(),
                "VotingPlaceId",
                "Name");

            ViewBag.userId = new SelectList(
                CombosHelper.GetTypeUserCoordinator(),
                "userId",
                "name");

            ViewBag.ReferId = new SelectList(
               CombosHelper.GetRefer(),
               "ReferId",
               "FullName");

            var leader = new Leader
            {
                CompanyId = user.CompanyId,
                Date = DateTime.Now,
                DateBorn = DateTime.Now,

            };
            return View(leader);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Leader leader, string newWorkPlace)
        {
            if (ModelState.IsValid)
            {
                if (leader.WorkPlaceId == 9999)
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
                               leader.CountryId);

                            ViewBag.CityId = new SelectList(
                                CombosHelper.GetCities(),
                                "CityId",
                                "Name",
                                leader.CityId);

                            ViewBag.CompanyId = new SelectList(
                                CombosHelper.GetCompanies(),
                                "CompanyId",
                                "Name",
                                leader.CompanyId);

                            ViewBag.DepartmentId = new SelectList(
                                CombosHelper.GetDepartments(),
                                "DepartmentId",
                                "Name",
                                leader.DepartmentId);

                            ViewBag.CommuneId = new SelectList(
                                CombosHelper.GetCommunes(),
                                "CommuneId",
                                "Name",
                                leader.CommuneId);

                            ViewBag.WorkPlaceId = new SelectList(
                              CombosHelper.GetWorkPlaces(),
                              "WorkPlaceId",
                              "Name",
                              leader.WorkPlaceId);

                            ViewBag.VotingPlaceId = new SelectList(
                                CombosHelper.GetVotingPlaces(),
                                "VotingPlaceId",
                                "Name",
                                leader.VotingPlaceId);
                            return View(leader);
                        }
                        db.SaveChanges();
                        leader.WorkPlaceId = workPlace.WorkPlaceId;
                    }
                }
                var refer = db.Refers.Find(leader.ReferId);
                if (leader.userId == 1) leader.BossId = refer.UserId;
                if (leader.userId == 2) leader.LinkId = refer.UserId;
                if (leader.userId == 3) leader.CoordinatorId = refer.UserId;

                db.Leaders.Add(leader);
                try
                {
                    db.SaveChanges();
                    //UsersHelper.CreateUserASP(leader.UserName, "Leader");
                    if (leader.PhotoFile != null)
                    {
                        var folder = "~/Content/Leader";
                        var file = string.Format("{0}.jpg", leader.LeaderId);
                        var response = FilesHelper.UploadPhoto(leader.PhotoFile, folder, file);
                        if (response)
                        {
                            var pic = string.Format("{0}/{1}", folder, file);
                            leader.Photo = pic;
                            db.Entry(leader).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }

                    var Refer = new Refer
                    {
                        ReferType = 4,
                        UserId = leader.LeaderId,
                        FullName = leader.FullName,
                        Active = 1
                    };
                    db.Refers.Add(Refer);
                    db.SaveChanges();


                    var city = db.Cities.Find(leader.CityId);
                    var department = db.Departments.Find(leader.DepartmentId);
                    var country = db.Countries.Find(leader.CountryId);
                    var comune = db.Communes.Find(leader.CommuneId);
                    var votingPlace = db.VotingPlaces.Find(leader.VotingPlaceId);
                    var Voter = new Voter
                    {
                        Address = leader.Address,
                        BossId = leader.BossId,
                        LinkId = leader.LinkId,
                        CoordinatorId = leader.CoordinatorId,                        
                        CityId = city.Name,
                        CommuneId = comune.Name,
                        CompanyId = leader.CompanyId,
                        CountryId = leader.Country.Name,
                        DepartmentId = department.Name,
                        Document = leader.Document,
                        FirstName = leader.FirstName,
                        LastName = leader.LastName,
                        Phone = leader.Phone,
                        UserName = leader.UserName,
                        VotingPlaceId = votingPlace.Name,
                        userId = 4,
                        ReferId = refer.ReferId,
                        PerfilId = leader.userId,
                        Fname = string.Format("{0} {1}", leader.FirstName, leader.LastName),
                        Barrio = leader.Barrio,
                        Profesion = leader.Profesion,
                        DateBorn = leader.DateBorn,
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
               leader.CountryId);

            ViewBag.CityId = new SelectList(
                CombosHelper.GetCities(),
                "CityId",
                "Name",
                leader.CityId);

            ViewBag.CompanyId = new SelectList(
                CombosHelper.GetCompanies(),
                "CompanyId",
                "Name",
                leader.CompanyId);

            ViewBag.DepartmentId = new SelectList(
                CombosHelper.GetDepartments(),
                "DepartmentId",
                "Name",
                leader.DepartmentId);

            ViewBag.CommuneId = new SelectList(
                CombosHelper.GetCommunes(),
                "CommuneId",
                "Name",
                leader.CommuneId);

            ViewBag.VotingPlaceId = new SelectList(
                CombosHelper.GetVotingPlaces(),
                "VotingPlaceId",
                "Name",
                leader.VotingPlaceId);

            ViewBag.userId = new SelectList(
                CombosHelper.GetTypeUserCoordinator(),
                "userId",
                "name",
                leader.userId);

            ViewBag.WorkPlaceId = new SelectList(
                             CombosHelper.GetWorkPlaces(),
                             "WorkPlaceId",
                             "Name",
                             leader.WorkPlaceId);

            ViewBag.ReferId = new SelectList(
               CombosHelper.GetRefer(),
               "ReferId",
               "FullName",
               leader.ReferId);

            return View(leader);
        }

        // GET: Leaders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var leader = db.Leaders.Find(id);
            if (leader == null)
            {
                return HttpNotFound();
            }
            ViewBag.CountryId = new SelectList(
               CombosHelper.GetCountries(),
               "CountryId",
               "Name",
               leader.CountryId);

            ViewBag.CityId = new SelectList(
                CombosHelper.GetCities(),
                "CityId",
                "Name",
                leader.CityId);

            ViewBag.CompanyId = new SelectList(
                CombosHelper.GetCompanies(),
                "CompanyId",
                "Name",
                leader.CompanyId);

            ViewBag.DepartmentId = new SelectList(
                CombosHelper.GetDepartments(),
                "DepartmentId",
                "Name",
                leader.DepartmentId);

            ViewBag.CommuneId = new SelectList(
                CombosHelper.GetCommunes(),
                "CommuneId",
                "Name",
                leader.CommuneId);

            ViewBag.VotingPlaceId = new SelectList(
                CombosHelper.GetVotingPlaces(),
                "VotingPlaceId",
                "Name",
                leader.VotingPlaceId);

            ViewBag.userId = new SelectList(
                CombosHelper.GetTypeUserCoordinator(),
                "userId",
                "name",
                leader.userId);

            ViewBag.WorkPlaceId = new SelectList(
                             CombosHelper.GetWorkPlaces(),
                             "WorkPlaceId",
                             "Name",
                             leader.WorkPlaceId);

            ViewBag.ReferId = new SelectList(
              CombosHelper.GetRefer(),
              "ReferId",
              "FullName",
              leader.ReferId);

            return View(leader);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Leader leader, string newWorkPlace)
        {
            if (ModelState.IsValid)
            {
                if (leader.WorkPlaceId == 9999)
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
                               leader.CountryId);

                            ViewBag.CityId = new SelectList(
                                CombosHelper.GetCities(),
                                "CityId",
                                "Name",
                                leader.CityId);

                            ViewBag.CompanyId = new SelectList(
                                CombosHelper.GetCompanies(),
                                "CompanyId",
                                "Name",
                                leader.CompanyId);

                            ViewBag.DepartmentId = new SelectList(
                                CombosHelper.GetDepartments(),
                                "DepartmentId",
                                "Name",
                                leader.DepartmentId);

                            ViewBag.CommuneId = new SelectList(
                                CombosHelper.GetCommunes(),
                                "CommuneId",
                                "Name",
                                leader.CommuneId);

                            ViewBag.WorkPlaceId = new SelectList(
                              CombosHelper.GetWorkPlaces(),
                              "WorkPlaceId",
                              "Name",
                              leader.WorkPlaceId);

                            ViewBag.VotingPlaceId = new SelectList(
                                CombosHelper.GetVotingPlaces(),
                                "VotingPlaceId",
                                "Name",
                                leader.VotingPlaceId);
                            return View(leader);
                        }
                        db.SaveChanges();
                        leader.WorkPlaceId = workPlace.WorkPlaceId;
                    }
                }
                if (leader.PhotoFile != null)
                {
                    var folder = "~/Content/Leader";
                    var file = string.Format("{0}.jpg", leader.LeaderId);
                    var response = FilesHelper.UploadPhoto(leader.PhotoFile, folder, file);
                    if (response)
                    {
                        var pic = string.Format("{0}/{1}", folder, file);
                        leader.Photo = pic;
                        db.Entry(leader).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }

                var refer = db.Refers.Find(leader.ReferId);

                if (leader.userId == 1) leader.BossId = refer.UserId;
                if (leader.userId == 2) leader.LinkId = refer.UserId;
                if (leader.userId == 3) leader.CoordinatorId = refer.UserId;
                db.Entry(leader).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                    var voter = db.Voters.Where(v => v.Document == leader.Document).FirstOrDefault();
                    var country = db.Countries.Find(leader.CountryId);
                    var department = db.Departments.Find(leader.DepartmentId);
                    var city = db.Cities.Find(leader.CityId);
                    var commune = db.Communes.Find(leader.CommuneId);
                    var voting = db.VotingPlaces.Find(leader.VotingPlaceId);
                    if (voter != null)
                    {
                        voter.Address = leader.Address;
                        voter.BossId = leader.BossId;
                        voter.LinkId = leader.LinkId;
                        voter.CoordinatorId = leader.CoordinatorId;
                        voter.CityId = city.Name;
                        voter.CommuneId = commune.Name;
                        voter.CompanyId = leader.CompanyId;
                        voter.CountryId = country.Name;
                        voter.DepartmentId = department.Name;
                        voter.Document = leader.Document;
                        voter.FirstName = leader.FirstName;
                        voter.LastName = leader.LastName;
                        voter.Phone = leader.Phone;
                        voter.UserName = leader.UserName;
                        voter.VotingPlaceId = voting.Name;
                        voter.userId = 4;
                        voter.ReferId = refer.ReferId;
                        voter.PerfilId = leader.userId;
                        voter.Fname = string.Format("{0} {1}", leader.FirstName, leader.LastName);
                        voter.Profesion = leader.Profesion;
                        voter.Barrio = leader.Barrio;
                        voter.DateBorn = leader.DateBorn;
                        db.Entry(voter).State = EntityState.Modified;
                    }

                    var refer2 = db.Refers.Where(r => r.ReferType == 4 && r.UserId == leader.LeaderId).FirstOrDefault();
                    refer2.FullName = leader.FullName;
                    db.Entry(refer2).State = EntityState.Modified;

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
              leader.CountryId);

            ViewBag.CityId = new SelectList(
                CombosHelper.GetCities(),
                "CityId",
                "Name",
                leader.CityId);

            ViewBag.CompanyId = new SelectList(
                CombosHelper.GetCompanies(),
                "CompanyId",
                "Name",
                leader.CompanyId);

            ViewBag.DepartmentId = new SelectList(
                CombosHelper.GetDepartments(),
                "DepartmentId",
                "Name",
                leader.DepartmentId);

            ViewBag.CommuneId = new SelectList(
                CombosHelper.GetCommunes(),
                "CommuneId",
                "Name",
                leader.CommuneId);

            ViewBag.VotingPlaceId = new SelectList(
                CombosHelper.GetVotingPlaces(),
                "VotingPlaceId",
                "Name",
                leader.VotingPlaceId);

            ViewBag.WorkPlaceId = new SelectList(
                             CombosHelper.GetWorkPlaces(),
                             "WorkPlaceId",
                             "Name",
                             leader.WorkPlaceId);

            ViewBag.userId = new SelectList(
                CombosHelper.GetTypeUserCoordinator(),
                "userId",
                "name",
                leader.userId);

            ViewBag.ReferId = new SelectList(
               CombosHelper.GetRefer(),
               "ReferId",
               "FullName",
               leader.ReferId);

            return View(leader);
        }

        // GET: Leaders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var leader = db.Leaders.Find(id);
            if (leader == null)
            {
                return HttpNotFound();
            }
            return View(leader);
        }

        // POST: Leaders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var leader = db.Leaders.Find(id);
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var response = FilesHelper.DeleteDocument(leader.Photo);
                    var Voter = db.Voters.Where(V => V.Document == leader.Document).FirstOrDefault();
                    if (Voter != null)
                    {
                        db.Voters.Remove(Voter);
                    }
                    var Refer = db.Refers.Where(r => r.ReferType == 4 && r.UserId == leader.LeaderId).FirstOrDefault();
                    if (Refer != null)
                    {
                        var voters = db.Voters.Where(v => v.ReferId == Refer.ReferId);
                        foreach (var voter in voters)
                        {
                            db.Voters.Remove(voter);
                        }
                        db.Refers.Remove(Refer);
                        db.SaveChanges();
                    }

                    

                    //Borro cualquier anotacion que tenga en la agenda
                    var dates = db.Dates.Where(d => d.ProfessionalId == leader.Document).ToList();
                    foreach (var item in dates)
                    {
                        db.Dates.Remove(item);
                    }

                    var dateItems = db.TimesDates.Where(di => di.ProfessionalId == leader.Document).ToList();
                    foreach (var it in dateItems)
                    {
                        db.TimesDates.Remove(it);
                    }

                    var HV = db.HojaVidas.Where(h => h.RolId == 4 && h.UserId == leader.LeaderId).FirstOrDefault();
                    if (HV != null)
                    {
                        db.HojaVidas.Remove(HV);
                    }
                    db.Leaders.Remove(leader);
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
              leader.CountryId);

            ViewBag.CityId = new SelectList(
                CombosHelper.GetCities(),
                "CityId",
                "Name",
                leader.CityId);

            ViewBag.CompanyId = new SelectList(
                CombosHelper.GetCompanies(),
                "CompanyId",
                "Name",
                leader.CompanyId);

            ViewBag.DepartmentId = new SelectList(
                CombosHelper.GetDepartments(),
                "DepartmentId",
                "Name",
                leader.DepartmentId);

            ViewBag.CommuneId = new SelectList(
                CombosHelper.GetCommunes(),
                "CommuneId",
                "Name",
                leader.CommuneId);

            ViewBag.WorkPlaceId = new SelectList(
                             CombosHelper.GetWorkPlaces(),
                             "WorkPlaceId",
                             "Name",
                             leader.WorkPlaceId);


            ViewBag.VotingPlaceId = new SelectList(
                CombosHelper.GetVotingPlaces(),
                "VotingPlaceId",
                "Name",
                leader.VotingPlaceId);

            ViewBag.userId = new SelectList(
                CombosHelper.GetTypeUserCoordinator(),
                "userId",
                "name",
                leader.userId);

            ViewBag.ReferId = new SelectList(
               CombosHelper.GetRefer(),
               "ReferId",
               "FullName",
               leader.ReferId);

            return View(leader);
        }

        public JsonResult GetDepartments(int countryId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var department = db.Departments.Where(m => m.CountryId == countryId);
            return Json(department);
        }

        public JsonResult GetPerfil(int userId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var refer = db.Refers.Where(r => r.ReferType == userId);
            return Json(refer);
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
            var leader = db.Leaders.Find(id);
            if (leader == null)
            {
                return HttpNotFound();
            }

            return View(leader);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CargarHojaVida(int LeaderId, HttpPostedFileBase File)
        {
            if (File != null)
            {
                var folder = "~/Content/HojasVida";
                var file = string.Format("{0}_{1}_{2}", 1, LeaderId, File.FileName);
                var response = FilesHelper.UploadPhoto(File, folder, file);
                if (response)
                {
                    var pic = string.Format("{0}/{1}", folder, file);

                    //Elimino la actual HV
                    var hoja = db.HojaVidas.Where(h => h.RolId == 4 && h.UserId == LeaderId).FirstOrDefault();
                    if (hoja != null)
                    {
                        var resp = FilesHelper.DeleteDocument(hoja.Path);
                        db.HojaVidas.Remove(hoja);
                        db.SaveChanges();
                    }
                    var HV = new HojaVida
                    {
                        RolId = 4,
                        UserId = LeaderId,
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
