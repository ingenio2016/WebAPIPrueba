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
    public class VotingPlacesController : Controller
    {
        private WebApiPruebaContext db = new WebApiPruebaContext();

        // GET: VotingPlaces
        public ActionResult Index(int? page = null)
        {
            page = (page ?? 1);
            var puestos = db.VotingPlaces.Include(c => c.Department).Include(c => c.Country).Include(c => c.City);
            return View(puestos.OrderBy(x => x.Country.Name).ThenBy(x => x.Department.Name).ThenBy(x=>x.City.Name).ThenBy(x => x.Name).ToPagedList((int)page, 6));
        }

        // GET: VotingPlaces/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var votingPlace = db.VotingPlaces.Find(id);
            if (votingPlace == null)
            {
                return HttpNotFound();
            }
            return View(votingPlace);
        }

        // GET: VotingPlaces/Create
        public ActionResult Create()
        {
            ViewBag.CountryId = new SelectList(
               CombosHelper.GetCountries(),
               "CountryId",
               "Name");

            ViewBag.DepartmentId = new SelectList(
                CombosHelper.GetDepartments(),
                "DepartmentId",
                "Name");

            ViewBag.CityId = new SelectList(
                CombosHelper.GetCities(),
                "CityId",
                "Name");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VotingPlace votingPlace)
        {
            if (ModelState.IsValid)
            {
                db.VotingPlaces.Add(votingPlace);
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
               votingPlace.CountryId);

            ViewBag.DepartmentId = new SelectList(
                CombosHelper.GetDepartments(),
                "DepartmentId",
                "Name",
                votingPlace.DepartmentId);

            ViewBag.CityId = new SelectList(
                CombosHelper.GetCities(),
                "CityId",
                "Name",
                votingPlace.CityId);

            return View(votingPlace);
        }

        // GET: VotingPlaces/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var votingPlace = db.VotingPlaces.Find(id);
            if (votingPlace == null)
            {
                return HttpNotFound();
            }

            ViewBag.CountryId = new SelectList(
               CombosHelper.GetCountries(),
               "CountryId",
               "Name",
               votingPlace.CountryId);

            ViewBag.DepartmentId = new SelectList(
                CombosHelper.GetDepartments(),
                "DepartmentId",
                "Name",
                votingPlace.DepartmentId);

            ViewBag.CityId = new SelectList(
                CombosHelper.GetCities(),
                "CityId",
                "Name",
                votingPlace.CityId);

            return View(votingPlace);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VotingPlace votingPlace)
        {
            if (ModelState.IsValid)
            {
                db.Entry(votingPlace).State = EntityState.Modified;
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
               votingPlace.CountryId);

            ViewBag.DepartmentId = new SelectList(
                CombosHelper.GetDepartments(),
                "DepartmentId",
                "Name",
                votingPlace.DepartmentId);

            ViewBag.CityId = new SelectList(
                CombosHelper.GetCities(),
                "CityId",
                "Name",
                votingPlace.CityId);

            return View(votingPlace);
        }

        // GET: VotingPlaces/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var votingPlace = db.VotingPlaces.Find(id);
            if (votingPlace == null)
            {
                return HttpNotFound();
            }
            return View(votingPlace);
        }

        // POST: VotingPlaces/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var votingPlace = db.VotingPlaces.Find(id);
            db.VotingPlaces.Remove(votingPlace);
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

            ViewBag.CountryId = new SelectList(
               CombosHelper.GetCountries(),
               "CountryId",
               "Name",
               votingPlace.CountryId);

            ViewBag.DepartmentId = new SelectList(
                CombosHelper.GetDepartments(),
                "DepartmentId",
                "Name",
                votingPlace.DepartmentId);

            ViewBag.CityId = new SelectList(
                CombosHelper.GetCities(),
                "CityId",
                "Name",
                votingPlace.CityId);

            return View(votingPlace);

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
    }
}
