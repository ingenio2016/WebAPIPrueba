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
    public class LinksController : Controller
    {
        private WebApiPruebaContext db = new WebApiPruebaContext();

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
                    var link = db.Links.Where(v => v.Document == doc);
                    return View(link.OrderBy(b => b.CompanyId).ThenBy(b => b.FirstName).ToPagedList((int)page, 5));
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
                    var link = db.Links.Where(b => b.LinkId == doc);
                    return View(link.OrderBy(b => b.CompanyId).ThenBy(b => b.FirstName).ToPagedList((int)page, 5));
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

                    var linkGeneral = db.Links.Where(b => b.CompanyId == user.CompanyId);
                    return View(linkGeneral.OrderBy(b => b.CompanyId).ThenBy(b => b.FirstName).ToPagedList((int)page, 5));
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

                        var linkComuna = db.Links.Where(b => b.CompanyId == user.CompanyId && b.CommuneId == Comuna);
                        return View(linkComuna.OrderBy(b => b.CompanyId).ThenBy(b => b.FirstName).ToPagedList((int)page, 10));
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

                        var linkVotacion = db.Links.Where(b => b.CompanyId == user.CompanyId && b.VotingPlaceId == VotingPlaceId);
                        return View(linkVotacion.OrderBy(b => b.CompanyId).ThenBy(b => b.FirstName).ToPagedList((int)page, 10));
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

                        var linkWork = db.Links.Where(b => b.CompanyId == user.CompanyId && b.WorkPlaceId == WorkPlaceId);
                        return View(linkWork.OrderBy(b => b.CompanyId).ThenBy(b => b.FirstName).ToPagedList((int)page, 10));
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
            var links = db.Links.Where(b => b.CompanyId == user.CompanyId);
            return View(links.OrderBy(b => b.CompanyId).ThenBy(b => b.FirstName).ToPagedList((int)page, 10));
        }

        public ActionResult LinksReport(int? filtro, int? communa, int? votacion, int? work, int? type)
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Index", "Links");
            }

            var inc = new object();
            inc = (from b in db.Links
                   join c in db.Companies on b.CompanyId equals c.CompanyId
                   join v in db.VotingPlaces on b.VotingPlaceId equals v.VotingPlaceId
                   where b.CompanyId == user.CompanyId
                   orderby b.FirstName
                   select new { c.Name, c.Logo, c.Phone, c.Address, b.FirstName, b.LastName, b.Document, BossAddress = b.Address, b.UserName, b.CommuneId, Telefono = b.Phone, b.Profesion, VotingName = v.Name }).ToList();

            if (filtro > 0)
            {
                if (filtro == 1)//GENERAL
                {
                    inc = (from b in db.Links
                           join c in db.Companies on b.CompanyId equals c.CompanyId
                           join v in db.VotingPlaces on b.VotingPlaceId equals v.VotingPlaceId
                           where b.CompanyId == user.CompanyId
                           orderby b.FirstName
                           select new { c.Name, c.Logo, c.Phone, c.Address, b.FirstName, b.LastName, b.Document, BossAddress = b.Address, b.UserName, Telefono = b.Phone, b.CommuneId, b.Profesion, VotingName = v.Name }).ToList();

                }
                if (filtro == 2)//COMUNA
                {
                    if (communa > 0)
                    {
                        inc = (from b in db.Links
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
                        inc = (from b in db.Links
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
                        inc = (from b in db.Links
                               join c in db.Companies on b.CompanyId equals c.CompanyId
                               join v in db.VotingPlaces on b.VotingPlaceId equals v.VotingPlaceId
                               where b.CompanyId == user.CompanyId && b.WorkPlaceId == work
                               orderby b.FirstName
                               select new { c.Name, c.Logo, c.Phone, c.Address, b.FirstName, b.LastName, b.Document, BossAddress = b.Address, b.UserName, Telefono = b.Phone, b.CommuneId, b.Profesion, VotingName = v.Name }).ToList();

                    }
                }
            }
           




            string path = Path.Combine(Server.MapPath("~/Reports"), "Links.rdlc");

            LocalReport lr = new LocalReport();
            lr.EnableExternalImages = true;
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return RedirectToAction("Index", "Links");
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


        // GET: Bosses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var link = db.Links.Find(id);
            if (link == null)
            {
                return HttpNotFound();
            }
            return View(link);
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

            ViewBag.BossId = new SelectList(
                CombosHelper.GetBosses(user.CompanyId),
                "BossId",
                "FullName");

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

            var link = new Link
            {
                CompanyId = user.CompanyId,
                Date = DateTime.Now,
                DateBorn = DateTime.Now,

            };
            return View(link);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Link link, string newWorkPlace)
        {
            if (ModelState.IsValid)
            {
                if (link.WorkPlaceId == 9999)
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
                               link.CountryId);

                            ViewBag.CityId = new SelectList(
                                CombosHelper.GetCities(),
                                "CityId",
                                "Name",
                                link.CityId);

                            ViewBag.CompanyId = new SelectList(
                                CombosHelper.GetCompanies(),
                                "CompanyId",
                                "Name",
                                link.CompanyId);

                            ViewBag.DepartmentId = new SelectList(
                                CombosHelper.GetDepartments(),
                                "DepartmentId",
                                "Name",
                                link.DepartmentId);

                            ViewBag.CommuneId = new SelectList(
                                CombosHelper.GetCommunes(),
                                "CommuneId",
                                "Name",
                                link.CommuneId);

                            ViewBag.WorkPlaceId = new SelectList(
                              CombosHelper.GetWorkPlaces(),
                              "WorkPlaceId",
                              "Name",
                              link.WorkPlaceId);

                            ViewBag.VotingPlaceId = new SelectList(
                                CombosHelper.GetVotingPlaces(),
                                "VotingPlaceId",
                                "Name",
                                link.VotingPlaceId);
                            return View(link);
                        }
                        db.SaveChanges();
                        link.WorkPlaceId = workPlace.WorkPlaceId;
                    }
                }
                db.Links.Add(link);
                try
                {
                    db.SaveChanges();
                    //UsersHelper.CreateUserASP(link.UserName, "Link");
                    if (link.PhotoFile != null)
                    {
                        var folder = "~/Content/Links";
                        var file = string.Format("{0}.jpg", link.LinkId);
                        var response = FilesHelper.UploadPhoto(link.PhotoFile, folder, file);
                        if (response)
                        {
                            var pic = string.Format("{0}/{1}", folder, file);
                            link.Photo = pic;
                            db.Entry(link).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }

                    var city = db.Cities.Find(link.CityId);
                    var department = db.Departments.Find(link.DepartmentId);
                    var country = db.Countries.Find(link.CountryId);
                    var comune = db.Communes.Find(link.CommuneId);
                    var votingPlace = db.VotingPlaces.Find(link.VotingPlaceId);

                    var refer = db.Refers.Where(r => r.ReferType == 1 && r.UserId == link.BossId).FirstOrDefault();
                    var Voter = new Voter
                    {
                        Address = link.Address,
                        BossId = link.BossId,
                        CityId = city.Name,
                        CommuneId = comune.Name,
                        CompanyId = link.CompanyId,
                        CountryId = country.Name,
                        DepartmentId = department.Name,
                        Document = link.Document,
                        FirstName = link.FirstName,
                        LastName = link.LastName,
                        Phone = link.Phone,
                        UserName = link.UserName,
                        VotingPlaceId = votingPlace.Name,
                        userId = 2,
                        ReferId = refer.ReferId,
                        PerfilId = 1,
                        Fname = string.Format("{0} {1}", link.FirstName, link.LastName),
                        Barrio = link.Barrio,
                        DateBorn = link.DateBorn,
                        Profesion = link.Profesion,
                    };
                    db.Voters.Add(Voter);
                    db.SaveChanges();

                    var Refer = new Refer
                    {
                        ReferType = 2,
                        UserId = link.LinkId,
                        FullName = link.FullName,
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
               link.CountryId);

            ViewBag.CityId = new SelectList(
                CombosHelper.GetCities(),
                "CityId",
                "Name",
                link.CityId);

            ViewBag.CompanyId = new SelectList(
                CombosHelper.GetCompanies(),
                "CompanyId",
                "Name",
                link.CompanyId);

            ViewBag.DepartmentId = new SelectList(
                CombosHelper.GetDepartments(),
                "DepartmentId",
                "Name",
                link.DepartmentId);

            ViewBag.BossId = new SelectList(
                CombosHelper.GetBosses(link.CompanyId),
                "BossId",
                "FullName",
                link.BossId);

            ViewBag.CommuneId = new SelectList(
                CombosHelper.GetCommunes(),
                "CommuneId",
                "Name",
                link.CommuneId);

            ViewBag.WorkPlaceId = new SelectList(
                             CombosHelper.GetWorkPlaces(),
                             "WorkPlaceId",
                             "Name",
                             link.WorkPlaceId);

            ViewBag.VotingPlaceId = new SelectList(
                CombosHelper.GetVotingPlaces(),
                "VotingPlaceId",
                "Name",
                link.VotingPlaceId);

            return View(link);
        }

        // GET: Bosses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var link = db.Links.Find(id);
            if (link == null)
            {
                return HttpNotFound();
            }
            ViewBag.CountryId = new SelectList(
               CombosHelper.GetCountries(),
               "CountryId",
               "Name",
               link.CountryId);

            ViewBag.CityId = new SelectList(
                CombosHelper.GetCities(),
                "CityId",
                "Name",
                link.CityId);

            ViewBag.CompanyId = new SelectList(
                CombosHelper.GetCompanies(),
                "CompanyId",
                "Name",
                link.CompanyId);

            ViewBag.DepartmentId = new SelectList(
                CombosHelper.GetDepartments(),
                "DepartmentId",
                "Name",
                link.DepartmentId);

            ViewBag.BossId = new SelectList(
                CombosHelper.GetBosses(link.CompanyId),
                "BossId",
                "FullName",
                link.BossId);

            ViewBag.CommuneId = new SelectList(
                CombosHelper.GetCommunes(),
                "CommuneId",
                "Name",
                link.CommuneId);

            ViewBag.WorkPlaceId = new SelectList(
                              CombosHelper.GetWorkPlaces(),
                              "WorkPlaceId",
                              "Name",
                              link.WorkPlaceId);

            ViewBag.VotingPlaceId = new SelectList(
                CombosHelper.GetVotingPlaces(),
                "VotingPlaceId",
                "Name",
                link.VotingPlaceId);

            return View(link);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Link link, string newWorkPlace)
        {
            if (ModelState.IsValid)
            {
                if (link.WorkPlaceId == 9999)
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
                               link.CountryId);

                            ViewBag.CityId = new SelectList(
                                CombosHelper.GetCities(),
                                "CityId",
                                "Name",
                                link.CityId);

                            ViewBag.CompanyId = new SelectList(
                                CombosHelper.GetCompanies(),
                                "CompanyId",
                                "Name",
                                link.CompanyId);

                            ViewBag.DepartmentId = new SelectList(
                                CombosHelper.GetDepartments(),
                                "DepartmentId",
                                "Name",
                                link.DepartmentId);

                            ViewBag.CommuneId = new SelectList(
                                CombosHelper.GetCommunes(),
                                "CommuneId",
                                "Name",
                                link.CommuneId);

                            ViewBag.WorkPlaceId = new SelectList(
                              CombosHelper.GetWorkPlaces(),
                              "WorkPlaceId",
                              "Name",
                              link.WorkPlaceId);

                            ViewBag.VotingPlaceId = new SelectList(
                                CombosHelper.GetVotingPlaces(),
                                "VotingPlaceId",
                                "Name",
                                link.VotingPlaceId);
                            return View(link);
                        }
                        db.SaveChanges();
                        link.WorkPlaceId = workPlace.WorkPlaceId;
                    }
                }
                if (link.PhotoFile != null)
                {
                    var folder = "~/Content/Links";
                    var file = string.Format("{0}.jpg", link.LinkId);
                    var response = FilesHelper.UploadPhoto(link.PhotoFile, folder, file);
                    if (response)
                    {
                        var pic = string.Format("{0}/{1}", folder, file);
                        link.Photo = pic;
                        db.Entry(link).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }

                db.Entry(link).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();

                    var referr = db.Refers.Where(r => r.ReferType == 1 && r.UserId == link.BossId).FirstOrDefault();

                    var voter = db.Voters.Where(v => v.Document == link.Document && v.UserName == link.UserName).FirstOrDefault();
                    var country = db.Countries.Find(link.CountryId);
                    var department = db.Departments.Find(link.DepartmentId);
                    var city = db.Cities.Find(link.CityId);
                    var commune = db.Communes.Find(link.CommuneId);
                    var voting = db.VotingPlaces.Find(link.VotingPlaceId);

                    voter.Address = link.Address;
                    voter.BossId = link.BossId;
                    voter.CityId = city.Name;
                    voter.CommuneId = commune.Name;
                    voter.CompanyId = link.CompanyId;
                    voter.CountryId = country.Name;
                    voter.DepartmentId = department.Name;
                    voter.Document = link.Document;
                    voter.FirstName = link.FirstName;
                    voter.LastName = link.LastName;
                    voter.Phone = link.Phone;
                    voter.UserName = link.UserName;
                    voter.VotingPlaceId = voting.Name;
                    voter.userId = 2;
                    voter.ReferId = referr.ReferId;
                    voter.PerfilId = 1;
                    voter.Fname = string.Format("{0} {1}", link.FirstName, link.LastName);
                    voter.Barrio = link.Barrio;
                    voter.Profesion = link.Profesion;
                    voter.DateBorn = link.DateBorn;
                    db.Entry(voter).State = EntityState.Modified;

                    var refer = db.Refers.Where(r => r.ReferType == 2 && r.UserId == link.LinkId).FirstOrDefault();
                    refer.FullName = link.FullName;
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
              link.CountryId);

            ViewBag.CityId = new SelectList(
                CombosHelper.GetCities(),
                "CityId",
                "Name",
                link.CityId);

            ViewBag.CompanyId = new SelectList(
                CombosHelper.GetCompanies(),
                "CompanyId",
                "Name",
                link.CompanyId);

            ViewBag.DepartmentId = new SelectList(
                CombosHelper.GetDepartments(),
                "DepartmentId",
                "Name",
                link.DepartmentId);

            ViewBag.BossId = new SelectList(
                CombosHelper.GetBosses(link.CompanyId),
                "BossId",
                "FullName",
                link.BossId);

            ViewBag.CommuneId = new SelectList(
                CombosHelper.GetCommunes(),
                "CommuneId",
                "Name",
                link.CommuneId);

            ViewBag.WorkPlaceId = new SelectList(
                             CombosHelper.GetWorkPlaces(),
                             "WorkPlaceId",
                             "Name",
                             link.WorkPlaceId);

            ViewBag.VotingPlaceId = new SelectList(
                CombosHelper.GetVotingPlaces(),
                "VotingPlaceId",
                "Name",
                link.VotingPlaceId);

            return View(link);
        }

        // GET: Bosses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var link = db.Links.Find(id);
            if (link == null)
            {
                return HttpNotFound();
            }
            return View(link);
        }

        // POST: Bosses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var link = db.Links.Find(id);
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var response = FilesHelper.DeleteDocument(link.Photo);
                    var Voter = db.Voters.Where(V => V.Document == link.Document).FirstOrDefault();
                    if (Voter != null)
                    {
                        db.Voters.Remove(Voter);
                    }
                    var Refer = db.Refers.Where(r => r.ReferType == 2 && r.UserId == link.LinkId).FirstOrDefault();
                    if (Refer != null)
                    {
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

                    //Elimino todo Coordinador y Lider Asociado a este enlace
                    
                    //Borro cualquier anotacion que tenga en la agenda
                    var dates = db.Dates.Where(d => d.ProfessionalId == link.Document).ToList();
                    foreach (var item in dates)
                    {
                        db.Dates.Remove(item);
                    }

                    var dateItems = db.TimesDates.Where(di => di.ProfessionalId == link.Document).ToList();
                    foreach (var it in dateItems)
                    {
                        db.TimesDates.Remove(it);
                    }


                    var HV = db.HojaVidas.Where(h => h.RolId == 2 && h.UserId == link.LinkId).FirstOrDefault();
                    if (HV != null)
                    {
                        db.HojaVidas.Remove(HV);
                    }
                    db.Links.Remove(link);
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
              link.CountryId);

            ViewBag.CityId = new SelectList(
                CombosHelper.GetCities(),
                "CityId",
                "Name",
                link.CityId);

            ViewBag.CompanyId = new SelectList(
                CombosHelper.GetCompanies(),
                "CompanyId",
                "Name",
                link.CompanyId);

            ViewBag.DepartmentId = new SelectList(
                CombosHelper.GetDepartments(),
                "DepartmentId",
                "Name",
                link.DepartmentId);

            ViewBag.BossId = new SelectList(
               CombosHelper.GetBosses(link.CompanyId),
               "BossId",
               "FullName",
               link.BossId);

            ViewBag.CommuneId = new SelectList(
                CombosHelper.GetCommunes(),
                "CommuneId",
                "Name",
                link.CommuneId);

            ViewBag.WorkPlaceId = new SelectList(
                             CombosHelper.GetWorkPlaces(),
                             "WorkPlaceId",
                             "Name",
                             link.WorkPlaceId);

            ViewBag.VotingPlaceId = new SelectList(
                CombosHelper.GetVotingPlaces(),
                "VotingPlaceId",
                "Name",
                link.VotingPlaceId);

            return View(link);
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
            var link = db.Links.Find(id);
            if (link == null)
            {
                return HttpNotFound();
            }

            return View(link);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CargarHojaVida(int LinkId, HttpPostedFileBase File)
        {
            if (File != null)
            {
                var folder = "~/Content/HojasVida";
                var file = string.Format("{0}_{1}_{2}", 1, LinkId, File.FileName);
                var response = FilesHelper.UploadPhoto(File, folder, file);
                if (response)
                {
                    var pic = string.Format("{0}/{1}", folder, file);

                    //Elimino la actual HV
                    var hoja = db.HojaVidas.Where(h => h.RolId == 2 && h.UserId == LinkId).FirstOrDefault();
                    if (hoja != null)
                    {
                        var resp = FilesHelper.DeleteDocument(hoja.Path);
                        db.HojaVidas.Remove(hoja);
                        db.SaveChanges();
                    }
                    var HV = new HojaVida
                    {
                        RolId = 2,
                        UserId = LinkId,
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
