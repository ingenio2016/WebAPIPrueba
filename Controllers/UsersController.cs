using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebAPIPrueba.Models;

namespace WebAPIPrueba.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private WebApiPruebaContext db = new WebApiPruebaContext();
        private static ApplicationDbContext userContext = new ApplicationDbContext();

        // GET: Users
        public ActionResult Index(int? page = null)
        {
            page = (page ?? 1);
            var users = db.Users.Include(u => u.City).Include(u => u.Company).Include(u => u.Department).Include(u => u.Country);
            return View(users.OrderBy(x=>x.FirstName).ToPagedList((int)page, 6));
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
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

            List<SelectListItem> mn = new List<SelectListItem>();
            mn.Add(new SelectListItem { Text = "Hombre", Value = "1" });
            mn.Add(new SelectListItem { Text = "Mujer", Value = "2" });
            ViewData["genero"] = mn;

            List<SelectListItem> muser = new List<SelectListItem>();
            muser.Add(new SelectListItem { Text = "Seleccione un rol", Value = "0" });
            muser.Add(new SelectListItem { Text = "Administrador", Value = "1" });
            muser.Add(new SelectListItem { Text = "Digitador", Value = "2" });
            muser.Add(new SelectListItem { Text = "Reuniones", Value = "3" });
            muser.Add(new SelectListItem { Text = "Secretario", Value = "4" });
            ViewData["rol"] = muser;
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user, string rol)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                try
                {
                    var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
                    var UserASP = userManager.FindByEmail(user.UserName);
                    if (UserASP == null)
                    {
                        db.SaveChanges();
                        if (rol == "1") UsersHelper.CreateUserASP(user.UserName, "User");
                        if (rol == "2") UsersHelper.CreateUserASP(user.UserName, "Digitador");
                        if (rol == "3") UsersHelper.CreateUserASP(user.UserName, "Reunion");
                        if (rol == "4") UsersHelper.CreateUserASP(user.UserName, "Secretario");
                        if (user.PhotoFile != null)
                        {
                            var folder = "~/Content/Users";
                            var file = string.Format("{0}.jpg", user.UserId);
                            var response = FilesHelper.UploadPhoto(user.PhotoFile, folder, file);
                            if (response)
                            {
                                var pic = string.Format("{0}/{1}", folder, file);
                                user.Photo = pic;
                                db.Entry(user).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe un usuario con ese correo electrónico");
                        List<SelectListItem> mn2 = new List<SelectListItem>();
                        mn2.Add(new SelectListItem { Text = "Hombre", Value = "1" });
                        mn2.Add(new SelectListItem { Text = "Mujer", Value = "2" });
                        ViewData["genero"] = mn2;
                        ViewBag.CountryId = new SelectList(
                           CombosHelper.GetCountries(),
                           "CountryId",
                           "Name",
                           user.CountryId);

                        ViewBag.CityId = new SelectList(
                            CombosHelper.GetCities(),
                            "CityId",
                            "Name",
                            user.CityId);

                        ViewBag.CompanyId = new SelectList(
                            CombosHelper.GetCompanies(),
                            "CompanyId",
                            "Name",
                            user.CompanyId);

                        ViewBag.DepartmentId = new SelectList(
                            CombosHelper.GetDepartments(),
                            "DepartmentId",
                            "Name",
                            user.DepartmentId);

                        List<SelectListItem> muser2 = new List<SelectListItem>();
                        muser2.Add(new SelectListItem { Text = "Seleccione un rol", Value = "0" });
                        muser2.Add(new SelectListItem { Text = "Administrador", Value = "1" });
                        muser2.Add(new SelectListItem { Text = "Digitador", Value = "2" });
                        muser2.Add(new SelectListItem { Text = "Reuniones", Value = "3" });
                        muser2.Add(new SelectListItem { Text = "Secretario", Value = "4" });
                        ViewData["rol"] = muser2;

                        return View(user);
                    }                    
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
               user.CountryId);

            ViewBag.CityId = new SelectList(
                CombosHelper.GetCities(), 
                "CityId", 
                "Name", 
                user.CityId);

            ViewBag.CompanyId = new SelectList(
                CombosHelper.GetCompanies(), 
                "CompanyId", 
                "Name", 
                user.CompanyId);

            ViewBag.DepartmentId = new SelectList(
                CombosHelper.GetDepartments(), 
                "DepartmentId", 
                "Name", 
                user.DepartmentId);

            List<SelectListItem> muser = new List<SelectListItem>();
            muser.Add(new SelectListItem { Text = "Seleccione un rol", Value = "0" });
            muser.Add(new SelectListItem { Text = "Administrador", Value = "1" });
            muser.Add(new SelectListItem { Text = "Digitador", Value = "2" });
            muser.Add(new SelectListItem { Text = "Reuniones", Value = "3" });
            muser.Add(new SelectListItem { Text = "Secretario", Value = "4" });
            ViewData["rol"] = muser;

            List<SelectListItem> mn = new List<SelectListItem>();
            mn.Add(new SelectListItem { Text = "Hombre", Value = "1" });
            mn.Add(new SelectListItem { Text = "Mujer", Value = "2" });
            ViewData["genero"] = mn;

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.CountryId = new SelectList(
              CombosHelper.GetCountries(),
              "CountryId",
              "Name",
              user.CountryId);

            ViewBag.CityId = new SelectList(
                CombosHelper.GetCities(),
                "CityId",
                "Name",
                user.CityId);

            ViewBag.CompanyId = new SelectList(
                CombosHelper.GetCompanies(),
                "CompanyId",
                "Name",
                user.CompanyId);

            ViewBag.DepartmentId = new SelectList(
                CombosHelper.GetDepartments(),
                "DepartmentId",
                "Name",
                user.DepartmentId);

            List<SelectListItem> mn = new List<SelectListItem>();
            mn.Add(new SelectListItem { Text = "Hombre", Value = "1" });
            mn.Add(new SelectListItem { Text = "Mujer", Value = "2" });
            ViewData["genero"] = mn;

            List<SelectListItem> muser = new List<SelectListItem>();
            muser.Add(new SelectListItem { Text = "Seleccione un rol", Value = "0" });
            muser.Add(new SelectListItem { Text = "Administrador", Value = "1" });
            muser.Add(new SelectListItem { Text = "Digitador", Value = "2" });
            muser.Add(new SelectListItem { Text = "Reuniones", Value = "3" });
            muser.Add(new SelectListItem { Text = "Secretario", Value = "4" });
            ViewData["rol"] = muser;

            return View(user);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User user, string rol)
        {
            if (ModelState.IsValid)
            {
                if (user.PhotoFile != null)
                {
                    var folder = "~/Content/Users";
                    var file = string.Format("{0}.jpg", user.UserId);
                    var response = FilesHelper.UploadPhoto(user.PhotoFile, folder, file);
                    if (response)
                    {
                        var pic = string.Format("{0}/{1}", folder, file);
                        user.Photo = pic;
                        db.Entry(user).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }

                var db2 = new WebApiPruebaContext();
                var currentUser = db2.Users.Find(user.UserId);

                UsersHelper.DeleteUser(currentUser.UserName);
                if (rol == "1") UsersHelper.CreateUserASP(user.UserName, "User");
                if (rol == "2") UsersHelper.CreateUserASP(user.UserName, "Digitador");
                if (rol == "3") UsersHelper.CreateUserASP(user.UserName, "Reunion");
                if (rol == "4") UsersHelper.CreateUserASP(user.UserName, "Secretario");

                db.Entry(user).State = EntityState.Modified;
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
            ViewBag.CountryId = new SelectList(
              CombosHelper.GetCountries(),
              "CountryId",
              "Name",
              user.CountryId);

            ViewBag.CityId = new SelectList(
                CombosHelper.GetCities(),
                "CityId",
                "Name",
                user.CityId);

            ViewBag.CompanyId = new SelectList(
                CombosHelper.GetCompanies(),
                "CompanyId",
                "Name",
                user.CompanyId);

            ViewBag.DepartmentId = new SelectList(
                CombosHelper.GetDepartments(),
                "DepartmentId",
                "Name",
                user.DepartmentId);

            List<SelectListItem> muser = new List<SelectListItem>();
            muser.Add(new SelectListItem { Text = "Seleccione un rol", Value = "0" });
            muser.Add(new SelectListItem { Text = "Administrador", Value = "1" });
            muser.Add(new SelectListItem { Text = "Digitador", Value = "2" });
            muser.Add(new SelectListItem { Text = "Reuniones", Value = "3" });
            muser.Add(new SelectListItem { Text = "Secretario", Value = "4" });
            ViewData["rol"] = muser;
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var user = db.Users.Find(id);
            db.Users.Remove(user);
            try
            {
                var response = FilesHelper.DeleteDocument(user.Photo);
                db.SaveChanges();
                UsersHelper.DeleteUser(user.UserName);
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
            ViewBag.CountryId = new SelectList(
              CombosHelper.GetCountries(),
              "CountryId",
              "Name",
              user.CountryId);

            ViewBag.CityId = new SelectList(
                CombosHelper.GetCities(),
                "CityId",
                "Name",
                user.CityId);

            ViewBag.CompanyId = new SelectList(
                CombosHelper.GetCompanies(),
                "CompanyId",
                "Name",
                user.CompanyId);

            ViewBag.DepartmentId = new SelectList(
                CombosHelper.GetDepartments(),
                "DepartmentId",
                "Name",
                user.DepartmentId);
            return View(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
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
    }
}
