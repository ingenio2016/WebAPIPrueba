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
    public class VotersController : Controller
    {
        private WebApiPruebaContext db = new WebApiPruebaContext();

        // GET: Voters
        public ActionResult Index(string Document, string CustomerId, int? FilterId, int? Comuna, int? VotingPlaceId, int? page = null)
        {
            ViewBag.uno = FilterId;
            ViewBag.dos = Comuna;
            ViewBag.tres = VotingPlaceId;
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
                    double doc = Convert.ToDouble(Document);
                    var vot = db.Voters.Where(v => v.Document == doc);
                    return View(vot.OrderBy(b => b.CompanyId).ThenBy(b => b.FirstName).ToPagedList((int)page, 5));
                }
            }

            if (!string.IsNullOrEmpty(CustomerId))
            {
                if (Utilidades.isNumeric(CustomerId))
                {
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
                    int doc = Convert.ToInt32(CustomerId);
                    var vot = db.Voters.Where(b => b.VoterId == doc);
                    return View(vot.OrderBy(b => b.CompanyId).ThenBy(b => b.FirstName).ToPagedList((int)page, 5));
                }
            }
            if (FilterId > 0)
            {
                if (FilterId == 1)//GENERAL
                {
                    ViewBag.FilterId = new SelectList(
                    CombosHelper.GetVoterFilters(),
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

                    var voterGeneral = db.Voters.Where(b => b.CompanyId == user.CompanyId);
                    return View(voterGeneral.OrderBy(b => b.CompanyId).ThenBy(b => b.FirstName).ToPagedList((int)page, 5));
                }
                if (FilterId == 2)//COMUNA
                {
                    if (Comuna > 0)
                    {
                        ViewBag.FilterId = new SelectList(
                        CombosHelper.GetVoterFilters(),
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

                        var voterComuna = db.Voters.Where(b => b.CompanyId == user.CompanyId && b.CommuneId == Comuna.ToString());
                        return View(voterComuna.OrderBy(b => b.CompanyId).ThenBy(b => b.FirstName).ToPagedList((int)page, 10));
                    }
                }
                if (FilterId == 3)//LUGAR DE VOTACION
                {
                    if (VotingPlaceId > 0)
                    {
                        var votingplace = db.VotingPlaces.Find(VotingPlaceId);

                        ViewBag.FilterId = new SelectList(
                        CombosHelper.GetVoterFilters(),
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

                        var voterVotacion = db.Voters.Where(b => b.CompanyId == user.CompanyId && b.VotingPlaceId == votingplace.Name);
                        return View(voterVotacion.OrderBy(b => b.CompanyId).ThenBy(b => b.FirstName).ToPagedList((int)page, 10));
                    }
                }
            }
            ViewBag.FilterId = new SelectList(
                    CombosHelper.GetVoterFilters(),
                    "FiltersId",
                    "name");

            ViewBag.VotingPlaceId = new SelectList(
                CombosHelper.GetVotingPlaces(),
                "VotingPlaceId",
                "Name");

            ViewBag.Comuna = new SelectList(
                    CombosHelper.GetCommunes(),
                    "CommuneId",
                    "Name");
            var voter = db.Voters.Where(b => b.CompanyId == user.CompanyId);
            return View(voter.OrderBy(b => b.CompanyId).ThenBy(b => b.FirstName).ToPagedList((int)page, 10));
        }

        public ActionResult VoterReport(int? filtro, int? communa, int? votacion, int? type)
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Index", "Voters");
            }

            var inc = new object();
            inc = (from b in db.Voters
                   join c in db.Companies on b.CompanyId equals c.CompanyId
                   join r in db.Refers on b.ReferId equals r.ReferId
                   where b.CompanyId == user.CompanyId
                   orderby b.userId, b.FirstName
                   select new { c.Name, c.Logo, c.Phone, c.Address, b.FirstName, b.LastName, b.Document, BossAddress = b.Address, Telefono = b.Phone, b.UserName, b.CommuneId, b.Profesion, VotingName = b.VotingPlaceId, b.userId, referido = r.FullName }).ToList();

            if (filtro > 0)
            {
                if (filtro == 1)//GENERAL
                {
                    inc = (from b in db.Voters
                           join c in db.Companies on b.CompanyId equals c.CompanyId
                           join r in db.Refers on b.ReferId equals r.ReferId
                           where b.CompanyId == user.CompanyId
                           orderby b.userId, b.FirstName
                           select new { c.Name, c.Logo, c.Phone, c.Address, b.FirstName, b.LastName, b.Document, Telefono = b.Phone, BossAddress = b.Address, b.UserName, b.CommuneId, b.Profesion, VotingName = b.VotingPlaceId, b.userId, referido = r.FullName }).ToList();

                }
                if (filtro == 2)//COMUNA
                {
                    if (communa > 0)
                    {
                        inc = (from b in db.Voters
                               join c in db.Companies on b.CompanyId equals c.CompanyId
                               join r in db.Refers on b.ReferId equals r.ReferId
                               where b.CompanyId == user.CompanyId && b.CommuneId == communa.ToString()
                               orderby b.userId, b.FirstName
                               select new { c.Name, c.Logo, c.Phone, c.Address, b.FirstName, b.LastName, b.Document, Telefono = b.Phone, BossAddress = b.Address, b.UserName, b.CommuneId, b.Profesion, VotingName = b.VotingPlaceId, b.userId, referido = r.FullName }).ToList();

                    }
                }
                if (filtro == 3)//LUGAR DE VOTACION
                {
                    if (votacion > 0)
                    {
                        var votingplace = db.VotingPlaces.Find(votacion);
                        inc = (from b in db.Voters
                               join c in db.Companies on b.CompanyId equals c.CompanyId
                               join r in db.Refers on b.ReferId equals r.ReferId
                               where b.CompanyId == user.CompanyId && b.VotingPlaceId == votingplace.Name
                               orderby b.userId, b.FirstName
                               select new { c.Name, c.Logo, c.Phone, c.Address, b.FirstName, b.LastName, b.Document, Telefono = b.Phone, BossAddress = b.Address, b.UserName, b.CommuneId, b.Profesion, VotingName = b.VotingPlaceId, b.userId, referido = r.FullName }).ToList();

                    }
                }
            }

            

            string path = Path.Combine(Server.MapPath("~/Reports"), "Voters.rdlc");

            LocalReport lr = new LocalReport();
            lr.EnableExternalImages = true;
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return RedirectToAction("Index", "Voters");
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


        // GET: Voters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var voter = db.Voters.Find(id);
            if (voter == null)
            {
                return HttpNotFound();
            }
            return View(voter);
        }

        // GET: Voters/Create
        public ActionResult Create()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.CityId = new SelectList(CombosHelper.GetCities(), "CityId", "Name");
            ViewBag.CommuneId = new SelectList(CombosHelper.GetCommunes(), "CommuneId", "Name");
            ViewBag.CountryId = new SelectList(CombosHelper.GetCountries(), "CountryId", "Name");
            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name");
            ViewBag.VotingPlaceId = new SelectList(CombosHelper.GetVotingPlaces(), "VotingPlaceId", "Name");
            ViewBag.PerfilId = new SelectList(
                CombosHelper.GetUser(),
                "userId",
                "name");

            ViewBag.ReferId = new SelectList(
               CombosHelper.GetRefer(),
               "ReferId",
               "FullName");
            var voter = new Voter
            {
                CompanyId = user.CompanyId,
                DateBorn = DateTime.Now,
            };
            return View(voter);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Voter voter)
        {
            if (ModelState.IsValid)
            {

                var refer = db.Refers.Find(voter.ReferId);
                var city = db.Cities.Find(Convert.ToInt32(voter.CityId));
                var department = db.Departments.Find(Convert.ToInt32(voter.DepartmentId));
                var country = db.Countries.Find(Convert.ToInt32(voter.CountryId));
                var comune = db.Communes.Find(Convert.ToInt32(voter.CommuneId));
                var votingPlace = db.VotingPlaces.Find(Convert.ToInt32(voter.VotingPlaceId));
                if (voter.PerfilId == 1) voter.BossId = refer.UserId;
                if (voter.PerfilId == 2) voter.LinkId = refer.UserId;
                if (voter.PerfilId == 3) voter.CoordinatorId = refer.UserId;
                if (voter.PerfilId == 4) voter.LeaderId = refer.UserId;
                voter.userId = 5;
                voter.ReferId = refer.ReferId;
                voter.Fname = string.Format("{0} {1}", voter.FirstName, voter.LastName);
                voter.CityId = city.Name;
                voter.DepartmentId = department.Name;
                voter.CountryId = country.Name;
                voter.CommuneId = comune.Name;
                voter.VotingPlaceId = votingPlace.Name;
                db.Voters.Add(voter);
                try
                {
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

            ViewBag.CityId = new SelectList(CombosHelper.GetCities(), "CityId", "Name", voter.CityId);
            ViewBag.CommuneId = new SelectList(CombosHelper.GetCommunes(), "CommuneId", "Name", voter.CommuneId);
            ViewBag.CountryId = new SelectList(CombosHelper.GetCountries(), "CountryId", "Name", voter.CountryId);
            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name", voter.DepartmentId);
            ViewBag.VotingPlaceId = new SelectList(CombosHelper.GetVotingPlaces(), "VotingPlaceId", "Name", voter.VotingPlaceId);
            ViewBag.PerfilId = new SelectList(
                CombosHelper.GetUser(),
                "userId",
                "name",
                voter.PerfilId);

            ViewBag.ReferId = new SelectList(
               CombosHelper.GetRefer(),
               "ReferId",
               "FullName",
               voter.ReferId);
            return View(voter);
        }

        // GET: Voters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var voter = db.Voters.Find(id);
            if (voter == null)
            {
                return HttpNotFound();
            }
            var city = db.Cities.Where(c => c.Name.Contains(voter.CityId)).FirstOrDefault();
            var comune = db.Communes.Where(c => c.Name.Contains(voter.CommuneId)).FirstOrDefault();
            var country = db.Countries.Where(c => c.Name.Contains(voter.CountryId)).FirstOrDefault();
            var department = db.Departments.Where(c => c.Name.Contains(voter.DepartmentId)).FirstOrDefault();
            var votingplace = db.VotingPlaces.Where(c => c.Name.Contains(voter.VotingPlaceId)).FirstOrDefault();

            ViewBag.CityId = new SelectList(CombosHelper.GetCities(), "CityId", "Name", city.CityId);
            ViewBag.CommuneId = new SelectList(CombosHelper.GetCommunes(), "CommuneId", "Name", comune.CommuneId);
            ViewBag.CountryId = new SelectList(CombosHelper.GetCountries(), "CountryId", "Name", country.CountryId);
            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name", department.DepartmentId);
            ViewBag.VotingPlaceId = new SelectList(CombosHelper.GetVotingPlaces(), "VotingPlaceId", "Name", votingplace.VotingPlaceId);
            ViewBag.PerfilId = new SelectList(CombosHelper.GetUser(), "userId", "name", voter.PerfilId);
            ViewBag.ReferId = new SelectList(CombosHelper.GetRefer(), "ReferId", "FullName", voter.ReferId);
            
            return View(voter);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Voter voter)
        {
            if (ModelState.IsValid)
            {
                var refer = db.Refers.Find(voter.ReferId);
                var city = db.Cities.Find(Convert.ToInt32(voter.CityId));
                var department = db.Departments.Find(Convert.ToInt32(voter.DepartmentId));
                var country = db.Countries.Find(Convert.ToInt32(voter.CountryId));
                var comune = db.Communes.Find(Convert.ToInt32(voter.CommuneId));
                var votingPlace = db.VotingPlaces.Find(Convert.ToInt32(voter.VotingPlaceId));
                if (voter.PerfilId == 1) voter.BossId = refer.UserId;
                if (voter.PerfilId == 2) voter.LinkId = refer.UserId;
                if (voter.PerfilId == 3) voter.CoordinatorId = refer.UserId;
                if (voter.PerfilId == 4) voter.LeaderId = refer.UserId;
                voter.ReferId = refer.ReferId;
                voter.Fname = string.Format("{0} {1}", voter.FirstName, voter.LastName);
                voter.CityId = city.Name;
                voter.DepartmentId = department.Name;
                voter.CountryId = country.Name;
                voter.CommuneId = comune.Name;
                voter.VotingPlaceId = votingPlace.Name;
                db.Entry(voter).State = EntityState.Modified;
                try
                {
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
            ViewBag.CityId = new SelectList(CombosHelper.GetCities(), "CityId", "Name", voter.CityId);
            ViewBag.CommuneId = new SelectList(CombosHelper.GetCommunes(), "CommuneId", "Name", voter.CommuneId);
            ViewBag.CountryId = new SelectList(CombosHelper.GetCountries(), "CountryId", "Name", voter.CountryId);
            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name", voter.DepartmentId);
            ViewBag.VotingPlaceId = new SelectList(CombosHelper.GetVotingPlaces(), "VotingPlaceId", "Name", voter.VotingPlaceId);
            ViewBag.PerfilId = new SelectList(
                CombosHelper.GetUser(),
                "userId",
                "name",
                voter.PerfilId);

            ViewBag.ReferId = new SelectList(
               CombosHelper.GetRefer(),
               "ReferId",
               "FullName",
               voter.ReferId);
            return View(voter);
        }

        // GET: Voters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var voter = db.Voters.Find(id);
            if (voter == null)
            {
                return HttpNotFound();
            }
            return View(voter);
        }

        // POST: Voters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var voter = db.Voters.Find(id);
            db.Voters.Remove(voter);
            try
            {
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
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
            ViewBag.CityId = new SelectList(CombosHelper.GetCities(), "CityId", "Name", voter.CityId);
            ViewBag.CommuneId = new SelectList(CombosHelper.GetCommunes(), "CommuneId", "Name", voter.CommuneId);
            ViewBag.CountryId = new SelectList(CombosHelper.GetCountries(), "CountryId", "Name", voter.CountryId);
            ViewBag.DepartmentId = new SelectList(CombosHelper.GetDepartments(), "DepartmentId", "Name", voter.DepartmentId);
            ViewBag.VotingPlaceId = new SelectList(CombosHelper.GetVotingPlaces(), "VotingPlaceId", "Name", voter.VotingPlaceId);
            ViewBag.PerfilId = new SelectList(
                CombosHelper.GetUser(),
                "userId",
                "name",
                voter.PerfilId);

            ViewBag.ReferId = new SelectList(
               CombosHelper.GetRefer(),
               "ReferId",
               "FullName",
               voter.ReferId);
            return View(voter);
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
            var refer = db.Refers.Where(r => r.ReferType == userId && r.Active == 1);
            return Json(refer);
        }

        public ActionResult ImportExcel()
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.userId = new SelectList(
                CombosHelper.GetUser(),
                "userId",
                "name");

            ViewBag.ReferId = new SelectList(
               CombosHelper.GetRefer(),
               "ReferId",
               "FullName");
            return View();
        }

        [ActionName("Importexcel")]
        [HttpPost]
        public ActionResult Importexcel1(int userId, int ReferId)
        {
            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            int boss = 0;
            int link = 0;
            int coordinator = 0;
            int leader = 0;

            if (Request.Files["FileUpload1"].ContentLength > 0)
            {
                var refer = db.Refers.Find(ReferId);
                if (userId == 1) boss = refer.UserId;
                if (userId == 2) link = refer.UserId;
                if (userId == 3) coordinator = refer.UserId;
                if (userId == 4) leader = refer.UserId;

                string extension = System.IO.Path.GetExtension(Request.Files["FileUpload1"].FileName).ToLower();
                string query = null;
                string connString = "";
                string[] validFileTypes = { ".xlsx"};

                string path1 = string.Format("{0}/{1}", Server.MapPath("~/Content/Uploads"), Request.Files["FileUpload1"].FileName);
                if (!Directory.Exists(path1))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Content/Uploads"));
                }
                if (validFileTypes.Contains(extension))
                {
                    if (System.IO.File.Exists(path1))
                    { System.IO.File.Delete(path1); }
                    Request.Files["FileUpload1"].SaveAs(path1);
                    
                    List<Mistakes> errores = new List<Mistakes>();
                    
                    if (extension.Trim() == ".xlsx")
                    {
                        connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path1 + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                        DataTable dt = Utility.ConvertXSLXtoDataTable(path1, connString);
                        int contador = 0;
                        int mistakes = 0;
                        int existentes = 0;
                        ViewBag.Message = string.Empty;
                        ViewBag.Message1 = string.Empty;
                        ViewBag.Message2 = string.Empty;
                        foreach (DataRow row in dt.Rows)
                        {
                            //ACA VAN LAS VALIDACIONES
                            if (Utilidades.isNumeric(row[2].ToString()))//valido que el documento sea un campo numerico
                            {
                                double document = Convert.ToDouble(row[2].ToString());//documento de identidad
                                var voter = db.Voters.Where(v => v.Document == document).ToList();
                                if (voter.Count == 0)
                                {
                                    string pais = "Colombia";
                                    string departmento = "Norte de Santander";
                                    string ciudad = "Cúcuta";
                                    var Country = db.Countries.Where(c => c.Name == pais).ToList();
                                    var Department = db.Departments.Where(c => c.Name == departmento).ToList();
                                    var City = db.Cities.Where(c => c.Name == ciudad).ToList();
                                    if (Country.Count > 0)
                                    {
                                        if (Department.Count > 0)
                                        {
                                            if (City.Count > 0)
                                            {
                                                string comune = row[8].ToString();
                                                string votacion = row[6].ToString();
                                                var comuna = db.Communes.Where(c => c.Name == comune).ToList();
                                                var votingplace = db.VotingPlaces.Where(v => v.Name == votacion).ToList();
                                                if (comuna.Count > 0)
                                                {
                                                    if (votingplace.Count > 0)
                                                    {
                                                        using (var transaction = db.Database.BeginTransaction())
                                                        {
                                                            try
                                                            {
                                                                DateTime value;
                                                                DateTime dateBorn;
                                                                if (!DateTime.TryParse(row[3].ToString(), out value))
                                                                {
                                                                    dateBorn = DateTime.Now;
                                                                }
                                                                else
                                                                {
                                                                    dateBorn = Convert.ToDateTime(row[3].ToString());
                                                                }
                                                                var Voter = new Voter
                                                                {
                                                                        FirstName = row[0].ToString(),
                                                                        LastName = row[1].ToString(),
                                                                        Document = Convert.ToDouble(row[2].ToString()),
                                                                        DateBorn = dateBorn,
                                                                        CountryId = pais,
                                                                        DepartmentId = departmento,
                                                                        CityId = ciudad,
                                                                        Address = row[4].ToString(),
                                                                        UserName = row[10].ToString(),
                                                                        Phone = row[5].ToString(),
                                                                        CommuneId = row[8].ToString(),
                                                                        Barrio = row[7].ToString(),
                                                                        Profesion = row[9].ToString(),
                                                                        VotingPlaceId = row[6].ToString(),
                                                                        BossId = boss,
                                                                        LinkId = link,
                                                                        CoordinatorId = coordinator,
                                                                        LeaderId = leader,
                                                                        CompanyId = user.CompanyId,
                                                                        Fname = string.Format("{0} {1}", row[0].ToString(), row[1].ToString()),
                                                                        ReferId = ReferId,
                                                                        PerfilId = userId,
                                                                        userId = 5,

                                                                    };
                                                                    db.Voters.Add(Voter);
                                                                    db.SaveChanges();
                                                                    contador += 1;
                                                                    transaction.Commit();
                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                    transaction.Rollback();
                                                                    ViewBag.Error = "Se ha presentado un error al subir la información. No se han afectado los registros";
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            mistakes += 1;
                                                            errores.Add(new Mistakes
                                                            {
                                                                document = row[2].ToString(),
                                                                mistakeDescription = "No se suministro Lugar de Votacion"
                                                            });
                                                        }
                                                    }
                                                    else
                                                    {
                                                        mistakes += 1;
                                                        errores.Add(new Mistakes
                                                        {
                                                            document = row[2].ToString(),
                                                            mistakeDescription = "No se suministro Comuna"
                                                        });
                                                    }
                                            }
                                            else
                                            {
                                                mistakes += 1;
                                                errores.Add(new Mistakes
                                                {
                                                    document = row[2].ToString(),
                                                    mistakeDescription = "No se suministro Ciudad"
                                                });
                                            }
                                        }
                                        else
                                        {
                                            mistakes += 1;
                                            errores.Add(new Mistakes
                                            {
                                                document = row[2].ToString(),
                                                mistakeDescription = "No se suministro Departamento"
                                            });
                                        }
                                    }
                                    else
                                    {
                                        mistakes += 1;
                                        errores.Add(new Mistakes
                                        {
                                            document = row[2].ToString(),
                                            mistakeDescription = "No se suministro País"
                                        });
                                    }
                                }
                                else
                                {
                                    existentes += 1;
                                    errores.Add(new Mistakes
                                    {
                                        document = row[2].ToString(),
                                        mistakeDescription = "Este registro ya existe"
                                    });
                                }
                            }
                            else
                            {
                                mistakes += 1;
                                errores.Add(new Mistakes
                                {
                                    document = row[2].ToString(),
                                    mistakeDescription = "El documento de identidad es inválido"
                                });
                            }


                        }
                        if (contador > 0)
                        {
                            ViewBag.Message = "Se han ingresado correctamente " + contador + " Registros";
                        }
                        if (errores.Count > 0)
                        {
                            ViewBag.Message1 = errores;
                        }

                        contador = 0;
                        mistakes = 0;
                        existentes = 0;
                    }

                }
                else
                {
                    ViewBag.Error = "Solo se permiten archivos con formato .xls, .xlsx";

                }

            }
            ViewBag.userId = new SelectList(
                CombosHelper.GetUser(),
                "userId",
                "name");

            ViewBag.ReferId = new SelectList(
               CombosHelper.GetRefer(),
               "ReferId",
               "FullName");
            return View();
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
            var voter = db.Voters.Find(id);
            if (voter == null)
            {
                return HttpNotFound();
            }

            return View(voter);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CargarHojaVida(int VoterId, HttpPostedFileBase File)
        {
            int roluserId = 0;
            if (File != null)
            {
                var folder = "~/Content/HojasVida";
                var file = string.Format("{0}_{1}_{2}", 1, VoterId, File.FileName);
                var response = FilesHelper.UploadPhoto(File, folder, file);
                if (response)
                {
                    var pic = string.Format("{0}/{1}", folder, file);

                    //Elimino la actual HV

                    var voter = db.Voters.Find(VoterId);
                    if (voter.userId == 1)
                    {
                        var boss = db.Bosses.Where(b => b.Document == voter.Document).FirstOrDefault();
                        if (boss != null)
                        {
                            var hoja = db.HojaVidas.Where(h => h.RolId == 1 && h.UserId == boss.BossId).FirstOrDefault();
                            if (hoja != null)
                            {
                                var resp = FilesHelper.DeleteDocument(hoja.Path);
                                db.HojaVidas.Remove(hoja);
                                db.SaveChanges();
                            }
                            roluserId = boss.BossId;
                        }
                    }
                    if (voter.userId == 2)
                    {
                        var link = db.Links.Where(b => b.Document == voter.Document).FirstOrDefault();
                        if (link != null)
                        {
                            var hoja = db.HojaVidas.Where(h => h.RolId == 2 && h.UserId == link.LinkId).FirstOrDefault();
                            if (hoja != null)
                            {
                                var resp = FilesHelper.DeleteDocument(hoja.Path);
                                db.HojaVidas.Remove(hoja);
                                db.SaveChanges();
                            }
                            roluserId = link.LinkId;
                        }
                    }
                    if (voter.userId == 3)
                    {
                        var coor = db.Coordinators.Where(b => b.Document == voter.Document).FirstOrDefault();
                        if (coor != null)
                        {
                            var hoja = db.HojaVidas.Where(h => h.RolId == 3 && h.UserId == coor.CoordinatorId).FirstOrDefault();
                            if (hoja != null)
                            {
                                var resp = FilesHelper.DeleteDocument(hoja.Path);
                                db.HojaVidas.Remove(hoja);
                                db.SaveChanges();
                            }
                            roluserId = coor.CoordinatorId;
                        }
                    }
                    if (voter.userId == 4)
                    {
                        var leader = db.Leaders.Where(b => b.Document == voter.Document).FirstOrDefault();
                        if (leader != null)
                        {
                            var hoja = db.HojaVidas.Where(h => h.RolId == 4 && h.UserId == leader.LeaderId).FirstOrDefault();
                            if (hoja != null)
                            {
                                var resp = FilesHelper.DeleteDocument(hoja.Path);
                                db.HojaVidas.Remove(hoja);
                                db.SaveChanges();
                            }
                            roluserId = leader.LeaderId;
                        }
                    }
                    if (voter.userId == 5)
                    {
                        var hoja = db.HojaVidas.Where(h => h.RolId == 5 && h.UserId == voter.VoterId).FirstOrDefault();
                        if (hoja != null)
                        {
                            var resp = FilesHelper.DeleteDocument(hoja.Path);
                            db.HojaVidas.Remove(hoja);
                            db.SaveChanges();
                        }
                        roluserId = voter.VoterId;
                    }

                    var HV = new HojaVida
                    {
                        RolId = voter.userId,
                        UserId = roluserId,
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
