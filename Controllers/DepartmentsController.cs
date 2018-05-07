using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAPIPrueba.Models;
using WebAPIPrueba.Models;

namespace WebAPIPrueba.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DepartmentsController : Controller
    {
        private WebApiPruebaContext db = new WebApiPruebaContext();

        // GET: Departments
        public ActionResult Index(int? page = null)
        {
            page = (page ?? 1);
            return View(db.Departments.OrderBy(x=>x.Country.Name).ThenBy(x=>x.Name).ToPagedList((int)page, 6));
        }

        // GET: Departments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // GET: Departments/Create
        public ActionResult Create()
        {
            ViewBag.CountryId = new SelectList(
               CombosHelper.GetCountries(),
               "CountryId",
               "Name");
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Department department)
        {
            if (ModelState.IsValid)
            {
                db.Departments.Add(department);
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
                department.CountryId);
            return View(department);
        }

        // GET: Departments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            ViewBag.CountryId = new SelectList(
                CombosHelper.GetCountries(),
                "CountryId",
                "Name",
                department.CountryId);
            return View(department);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Department department)
        {
            if (ModelState.IsValid)
            {
                db.Entry(department).State = EntityState.Modified;
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
                department.CountryId);
            return View(department);
        }

        // GET: Departments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var department = db.Departments.Find(id);
            db.Departments.Remove(department);
            try
            {
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch(Exception ex)
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
                department.CountryId);
            return View(department);
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
