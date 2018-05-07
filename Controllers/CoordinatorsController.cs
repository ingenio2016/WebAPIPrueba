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
    public class CoordinatorsController : Controller
    {
        private WebApiPruebaContext db = new WebApiPruebaContext();

        // GET: Coordinators
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
                    var coord = db.Coordinators.Where(v => v.Document == doc);
                    return View(coord.OrderBy(b => b.CompanyId).ThenBy(b => b.FirstName).ToPagedList((int)page, 5));
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
                    var coord = db.Coordinators.Where(b => b.CoordinatorId == doc);
                    return View(coord.OrderBy(b => b.CompanyId).ThenBy(b => b.FirstName).ToPagedList((int)page, 5));
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

                    var coorGeneral = db.Coordinators.Where(b => b.CompanyId == user.CompanyId);
                    return View(coorGeneral.OrderBy(b => b.CompanyId).ThenBy(b => b.FirstName).ToPagedList((int)page, 5));
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
                        ViewBag.WorkPlaceId = new SelectList(
                      CombosHelper.GetWorkPlaces(),
                      "WorkPlaceId",
                      "Name");

                        ViewBag.VotingPlaceId = new SelectList(
                        CombosHelper.GetVotingPlaces(),
                        "VotingPlaceId",
                        "Name");

                        var coorComuna = db.Coordinators.Where(b => b.CompanyId == user.CompanyId && b.CommuneId == Comuna);
                        return View(coorComuna.OrderBy(b => b.CompanyId).ThenBy(b => b.FirstName).ToPagedList((int)page, 10));
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

                        ViewBag.WorkPlaceId = new SelectList(
                       CombosHelper.GetWorkPlaces(),
                       "WorkPlaceId",
                       "Name");

                       ViewBag.VotingPlaceId = new SelectList(
                        CombosHelper.GetVotingPlaces(),
                        "VotingPlaceId",
                        "Name",
                        VotingPlaceId);

                        var coorVotacion = db.Coordinators.Where(b => b.CompanyId == user.CompanyId && b.VotingPlaceId == VotingPlaceId);
                        return View(coorVotacion.OrderBy(b => b.CompanyId).ThenBy(b => b.FirstName).ToPagedList((int)page, 10));
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

                        var coorWork = db.Coordinators.Where(b => b.CompanyId == user.CompanyId && b.WorkPlaceId == WorkPlaceId);
                        return View(coorWork.OrderBy(b => b.CompanyId).ThenBy(b => b.FirstName).ToPagedList((int)page, 10));
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
            var coor = db.Coordinators.Where(b => b.CompanyId == user.CompanyId);
            return View(coor.OrderBy(b => b.CompanyId).ThenBy(b => b.FirstName).ToPagedList((int)page, 10));
        }

        public ActionResult CoorReport(int? filtro, int? communa, int? votacion, int? work, int? type)
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Index", "Coordinators");
            }

            var inc = new object();
            inc = (from b in db.Coordinators
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
                    inc = (from b in db.Coordinators
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
                        inc = (from b in db.Coordinators
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
                        inc = (from b in db.Coordinators
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
                        inc = (from b in db.Coordinators
                               join c in db.Companies on b.CompanyId equals c.CompanyId
                               join v in db.VotingPlaces on b.VotingPlaceId equals v.VotingPlaceId
                               join r in db.Refers on b.ReferId equals r.ReferId
                               where b.CompanyId == user.CompanyId && b.WorkPlaceId == work
                               orderby b.FirstName
                               select new { c.Name, c.Logo, c.Phone, c.Address, b.FirstName, b.LastName, b.Document, BossAddress = b.Address, b.UserName, Telefono = b.Phone, b.CommuneId, b.Profesion, VotingName = v.Name, referido = r.FullName }).ToList();

                    }
                }
            }
            




            string path = Path.Combine(Server.MapPath("~/Reports"), "Coordinators.rdlc");

            LocalReport lr = new LocalReport();
            lr.EnableExternalImages = true;
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return RedirectToAction("Index", "Coordinators");
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


        // GET: Coordinators/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var coordinator = db.Coordinators.Find(id);
            if (coordinator == null)
            {
                return HttpNotFound();
            }
            return View(coordinator);
        }

        // GET: Coordinators/Create
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
                CombosHelper.GetTypeUser(),
                "userId",
                "name");

            ViewBag.ReferId = new SelectList(
               CombosHelper.GetRefer(),
               "ReferId",
               "FullName");

            var coordinator = new Coordinator
            {
                CompanyId = user.CompanyId,
                Date = DateTime.Now,
                DateBorn = DateTime.Now,

            };
            return View(coordinator);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Coordinator coordinator, string newWorkPlace)
        {
            if (ModelState.IsValid)
            {
                if (coordinator.WorkPlaceId == 9999)
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
                               coordinator.CountryId);

                            ViewBag.CityId = new SelectList(
                                CombosHelper.GetCities(),
                                "CityId",
                                "Name",
                                coordinator.CityId);

                            ViewBag.CompanyId = new SelectList(
                                CombosHelper.GetCompanies(),
                                "CompanyId",
                                "Name",
                                coordinator.CompanyId);

                            ViewBag.DepartmentId = new SelectList(
                                CombosHelper.GetDepartments(),
                                "DepartmentId",
                                "Name",
                                coordinator.DepartmentId);

                            ViewBag.CommuneId = new SelectList(
                                CombosHelper.GetCommunes(),
                                "CommuneId",
                                "Name",
                                coordinator.CommuneId);

                            ViewBag.WorkPlaceId = new SelectList(
                              CombosHelper.GetWorkPlaces(),
                              "WorkPlaceId",
                              "Name",
                              coordinator.WorkPlaceId);

                            ViewBag.VotingPlaceId = new SelectList(
                                CombosHelper.GetVotingPlaces(),
                                "VotingPlaceId",
                                "Name",
                                coordinator.VotingPlaceId);
                            return View(coordinator);
                        }
                        db.SaveChanges();
                        coordinator.WorkPlaceId = workPlace.WorkPlaceId;
                    }
                }
                var refer = db.Refers.Find(coordinator.ReferId);
                if (coordinator.userId == 1) coordinator.BossId = refer.UserId;
                if (coordinator.userId == 2) coordinator.LinkId = refer.UserId;

                db.Coordinators.Add(coordinator);
                try
                {
                    db.SaveChanges();
                    if (coordinator.PhotoFile != null)
                    {
                        var folder = "~/Content/Coordinators";
                        var file = string.Format("{0}.jpg", coordinator.CoordinatorId);
                        var response = FilesHelper.UploadPhoto(coordinator.PhotoFile, folder, file);
                        if (response)
                        {
                            var pic = string.Format("{0}/{1}", folder, file);
                            coordinator.Photo = pic;
                            db.Entry(coordinator).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }

                   
                    var city = db.Cities.Find(coordinator.CityId);
                    var department = db.Departments.Find(coordinator.DepartmentId);
                    var country = db.Countries.Find(coordinator.CountryId);
                    var comune = db.Communes.Find(coordinator.CommuneId);
                    var votingPlace = db.VotingPlaces.Find(coordinator.VotingPlaceId);


                    var Voter = new Voter
                    {
                        Address = coordinator.Address,
                        BossId = coordinator.BossId,
                        LinkId = coordinator.LinkId,
                        CityId = city.Name,
                        CommuneId = comune.Name,
                        CompanyId = coordinator.CompanyId,
                        CountryId = country.Name,
                        DepartmentId = department.Name,
                        Document = coordinator.Document,
                        FirstName = coordinator.FirstName,
                        LastName = coordinator.LastName,
                        Phone = coordinator.Phone,
                        UserName = coordinator.UserName,
                        VotingPlaceId = votingPlace.Name,
                        userId = 3,
                        ReferId = refer.ReferId,
                        PerfilId = coordinator.userId,
                        Fname = string.Format("{0} {1}", coordinator.FirstName, coordinator.LastName),
                        Barrio = coordinator.Barrio,
                        Profesion = coordinator.Profesion,
                        DateBorn = coordinator.DateBorn,
                    };
                    db.Voters.Add(Voter);
                    db.SaveChanges();

                    var Refer = new Refer
                    {
                        ReferType = 3,
                        UserId = coordinator.CoordinatorId,
                        FullName = coordinator.FullName,
                        Active = 1
                    };
                    db.Refers.Add(Refer);
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
               coordinator.CountryId);

            ViewBag.CityId = new SelectList(
                CombosHelper.GetCities(),
                "CityId",
                "Name",
                coordinator.CityId);

            ViewBag.CompanyId = new SelectList(
                CombosHelper.GetCompanies(),
                "CompanyId",
                "Name",
                coordinator.CompanyId);

            ViewBag.DepartmentId = new SelectList(
                CombosHelper.GetDepartments(),
                "DepartmentId",
                "Name",
                coordinator.DepartmentId);

            ViewBag.CommuneId = new SelectList(
                CombosHelper.GetCommunes(),
                "CommuneId",
                "Name",
                coordinator.CommuneId);

           
            ViewBag.VotingPlaceId = new SelectList(
                CombosHelper.GetVotingPlaces(),
                "VotingPlaceId",
                "Name",
                coordinator.VotingPlaceId);

            ViewBag.WorkPlaceId = new SelectList(
                              CombosHelper.GetWorkPlaces(),
                              "WorkPlaceId",
                              "Name",
                              coordinator.WorkPlaceId);

            ViewBag.userId = new SelectList(
                CombosHelper.GetTypeUser(),
                "userId",
                "name",
                coordinator.userId);

            ViewBag.ReferId = new SelectList(
               CombosHelper.GetRefer(),
               "ReferId",
               "FullName",
               coordinator.ReferId);

            return View(coordinator);
        }

        // GET: Coordinators/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var coordinator = db.Coordinators.Find(id);
            if (coordinator == null)
            {
                return HttpNotFound();
            }

            ViewBag.CountryId = new SelectList(
               CombosHelper.GetCountries(),
               "CountryId",
               "Name",
               coordinator.CountryId);

            ViewBag.CityId = new SelectList(
                CombosHelper.GetCities(),
                "CityId",
                "Name",
                coordinator.CityId);

            ViewBag.CompanyId = new SelectList(
                CombosHelper.GetCompanies(),
                "CompanyId",
                "Name",
                coordinator.CompanyId);

            ViewBag.DepartmentId = new SelectList(
                CombosHelper.GetDepartments(),
                "DepartmentId",
                "Name",
                coordinator.DepartmentId);

            ViewBag.CommuneId = new SelectList(
                CombosHelper.GetCommunes(),
                "CommuneId",
                "Name",
                coordinator.CommuneId);

            ViewBag.WorkPlaceId = new SelectList(
                             CombosHelper.GetWorkPlaces(),
                             "WorkPlaceId",
                             "Name",
                             coordinator.WorkPlaceId);


            ViewBag.VotingPlaceId = new SelectList(
                CombosHelper.GetVotingPlaces(),
                "VotingPlaceId",
                "Name",
                coordinator.VotingPlaceId);

            ViewBag.userId = new SelectList(
                CombosHelper.GetTypeUser(),
                "userId",
                "name",
                coordinator.userId);

            ViewBag.ReferId = new SelectList(
               CombosHelper.GetRefer(),
               "ReferId",
               "FullName",
               coordinator.ReferId);

            return View(coordinator);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Coordinator coordinator, string newWorkPlace)
        {
            if (ModelState.IsValid)
            {
                if (coordinator.WorkPlaceId == 9999)
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
                               coordinator.CountryId);

                            ViewBag.CityId = new SelectList(
                                CombosHelper.GetCities(),
                                "CityId",
                                "Name",
                                coordinator.CityId);

                            ViewBag.CompanyId = new SelectList(
                                CombosHelper.GetCompanies(),
                                "CompanyId",
                                "Name",
                                coordinator.CompanyId);

                            ViewBag.DepartmentId = new SelectList(
                                CombosHelper.GetDepartments(),
                                "DepartmentId",
                                "Name",
                                coordinator.DepartmentId);

                            ViewBag.CommuneId = new SelectList(
                                CombosHelper.GetCommunes(),
                                "CommuneId",
                                "Name",
                                coordinator.CommuneId);

                            ViewBag.WorkPlaceId = new SelectList(
                              CombosHelper.GetWorkPlaces(),
                              "WorkPlaceId",
                              "Name",
                              coordinator.WorkPlaceId);

                            ViewBag.VotingPlaceId = new SelectList(
                                CombosHelper.GetVotingPlaces(),
                                "VotingPlaceId",
                                "Name",
                                coordinator.VotingPlaceId);
                            return View(coordinator);
                        }
                        db.SaveChanges();
                        coordinator.WorkPlaceId = workPlace.WorkPlaceId;
                    }
                }
                var refe = db.Refers.Find(coordinator.ReferId);
                if (coordinator.userId == 1) coordinator.BossId = refe.UserId;
                if (coordinator.userId == 2) coordinator.LinkId = refe.UserId;

                if (coordinator.PhotoFile != null)
                {
                    var folder = "~/Content/Coordinators";
                    var file = string.Format("{0}.jpg", coordinator.CoordinatorId);
                    var response = FilesHelper.UploadPhoto(coordinator.PhotoFile, folder, file);
                    if (response)
                    {
                        var pic = string.Format("{0}/{1}", folder, file);
                        coordinator.Photo = pic;
                        db.Entry(coordinator).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }


                db.Entry(coordinator).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                    var voter = db.Voters.Where(v => v.Document == coordinator.Document).FirstOrDefault();
                    var country = db.Countries.Find(coordinator.CountryId);
                    var department = db.Departments.Find(coordinator.DepartmentId);
                    var city = db.Cities.Find(coordinator.CityId);
                    var commune = db.Communes.Find(coordinator.CommuneId);
                    var voting = db.VotingPlaces.Find(coordinator.VotingPlaceId);
                    if (voter != null)
                    {
                        voter.Address = coordinator.Address;
                        voter.BossId = coordinator.BossId;
                        voter.LinkId = coordinator.LinkId;
                        voter.CityId = city.Name;
                        voter.CommuneId = commune.Name;
                        voter.CompanyId = coordinator.CompanyId;
                        voter.CountryId = country.Name;
                        voter.DepartmentId = department.Name;
                        voter.Document = coordinator.Document;
                        voter.FirstName = coordinator.FirstName;
                        voter.LastName = coordinator.LastName;
                        voter.Phone = coordinator.Phone;
                        voter.UserName = coordinator.UserName;
                        voter.VotingPlaceId = voting.Name;
                        voter.userId = 3;
                        voter.ReferId = refe.ReferId;
                        voter.PerfilId = coordinator.userId;
                        voter.Fname = string.Format("{0} {1}", coordinator.FirstName, coordinator.LastName);
                        voter.Profesion = coordinator.Profesion;
                        voter.Barrio = coordinator.Barrio;
                        voter.DateBorn = coordinator.DateBorn;
                        db.Entry(voter).State = EntityState.Modified;
                    }
                    var refer = db.Refers.Where(r => r.ReferType == 3 && r.UserId == coordinator.CoordinatorId).FirstOrDefault();
                    refer.FullName = coordinator.FullName;
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
               coordinator.CountryId);

            ViewBag.CityId = new SelectList(
                CombosHelper.GetCities(),
                "CityId",
                "Name",
                coordinator.CityId);

            ViewBag.CompanyId = new SelectList(
                CombosHelper.GetCompanies(),
                "CompanyId",
                "Name",
                coordinator.CompanyId);

            ViewBag.DepartmentId = new SelectList(
                CombosHelper.GetDepartments(),
                "DepartmentId",
                "Name",
                coordinator.DepartmentId);

            ViewBag.CommuneId = new SelectList(
                CombosHelper.GetCommunes(),
                "CommuneId",
                "Name",
                coordinator.CommuneId);

            ViewBag.WorkPlaceId = new SelectList(
                             CombosHelper.GetWorkPlaces(),
                             "WorkPlaceId",
                             "Name",
                             coordinator.WorkPlaceId);

            ViewBag.VotingPlaceId = new SelectList(
                CombosHelper.GetVotingPlaces(),
                "VotingPlaceId",
                "Name",
                coordinator.VotingPlaceId);

            ViewBag.userId = new SelectList(
                CombosHelper.GetTypeUser(),
                "userId",
                "name",
                coordinator.userId);

            ViewBag.ReferId = new SelectList(
               CombosHelper.GetRefer(),
               "ReferId",
               "FullName",
               coordinator.ReferId);

            return View(coordinator);
        }

        // GET: Coordinators/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var coordinator = db.Coordinators.Find(id);
            if (coordinator == null)
            {
                return HttpNotFound();
            }
            return View(coordinator);
        }

        // POST: Coordinators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)

        {
            var coordinator = db.Coordinators.Find(id);
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var response = FilesHelper.DeleteDocument(coordinator.Photo);
                    var Voter = db.Voters.Where(V => V.Document == coordinator.Document).FirstOrDefault();
                    if (Voter != null)
                    {
                        db.Voters.Remove(Voter);
                    }
                    var Refer = db.Refers.Where(r => r.ReferType == 3 && r.UserId == coordinator.CoordinatorId).FirstOrDefault();
                    if (Refer != null)
                    {
                        //Borro todo lider asociado a este coordinador
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
                    var dates = db.Dates.Where(d => d.ProfessionalId == coordinator.Document).ToList();
                    foreach (var item in dates)
                    {
                        db.Dates.Remove(item);
                    }

                    var dateItems = db.TimesDates.Where(di => di.ProfessionalId == coordinator.Document).ToList();
                    foreach (var it in dateItems)
                    {
                        db.TimesDates.Remove(it);
                    }

                    var HV = db.HojaVidas.Where(h => h.RolId == 3 && h.UserId == coordinator.CoordinatorId).FirstOrDefault();
                    if (HV != null)
                    {
                        db.HojaVidas.Remove(HV);
                    }
                    db.Coordinators.Remove(coordinator);
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
               coordinator.CountryId);

            ViewBag.CityId = new SelectList(
                CombosHelper.GetCities(),
                "CityId",
                "Name",
                coordinator.CityId);

            ViewBag.CompanyId = new SelectList(
                CombosHelper.GetCompanies(),
                "CompanyId",
                "Name",
                coordinator.CompanyId);

            ViewBag.DepartmentId = new SelectList(
                CombosHelper.GetDepartments(),
                "DepartmentId",
                "Name",
                coordinator.DepartmentId);

            ViewBag.CommuneId = new SelectList(
                CombosHelper.GetCommunes(),
                "CommuneId",
                "Name",
                coordinator.CommuneId);

            ViewBag.WorkPlaceId = new SelectList(
                             CombosHelper.GetWorkPlaces(),
                             "WorkPlaceId",
                             "Name",
                             coordinator.WorkPlaceId);

            ViewBag.VotingPlaceId = new SelectList(
                CombosHelper.GetVotingPlaces(),
                "VotingPlaceId",
                "Name",
                coordinator.VotingPlaceId);

            ViewBag.userId = new SelectList(
                CombosHelper.GetTypeUser(),
                "userId",
                "name",
                coordinator.userId);

            ViewBag.ReferId = new SelectList(
               CombosHelper.GetRefer(),
               "ReferId",
               "FullName",
               coordinator.ReferId);

            return View(coordinator);
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

        public JsonResult GetPerfil(int userId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var refer = db.Refers.Where(r => r.ReferType == userId);
            return Json(refer);
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
            var coordinador = db.Coordinators.Find(id);
            if (coordinador == null)
            {
                return HttpNotFound();
            }

            return View(coordinador);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CargarHojaVida(int CoordinatorId, HttpPostedFileBase File)
        {
            if (File != null)
            {
                var folder = "~/Content/HojasVida";
                var file = string.Format("{0}_{1}_{2}", 1, CoordinatorId, File.FileName);
                var response = FilesHelper.UploadPhoto(File, folder, file);
                if (response)
                {
                    var pic = string.Format("{0}/{1}", folder, file);

                    //Elimino la actual HV
                    var hoja = db.HojaVidas.Where(h => h.RolId == 3 && h.UserId == CoordinatorId).FirstOrDefault();
                    if (hoja != null)
                    {
                        var resp = FilesHelper.DeleteDocument(hoja.Path);
                        db.HojaVidas.Remove(hoja);
                        db.SaveChanges();
                    }
                    var HV = new HojaVida
                    {
                        RolId = 3,
                        UserId = CoordinatorId,
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
