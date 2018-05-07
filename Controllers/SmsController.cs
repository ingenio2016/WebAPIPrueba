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

namespace WebAPIPrueba.Controllers
{
    public class SmsController : Controller
    {
        private WebApiPruebaContext db = new WebApiPruebaContext();

        // GET: Sms
        public ActionResult Index(int? page = null)
        {
            page = (page ?? 1);
            return View(db.Sms.OrderByDescending(x => x.SmsDate).ToPagedList((int)page, 6));
        }

        // GET: Sms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var sms = db.Sms.Find(id);
            if (sms == null)
            {
                return HttpNotFound();
            }
            return View(sms);
        }

        // GET: Sms/Create
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

        // POST: Sms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Sms sms)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //ACA VA LA LOGICA DE ENVIO Y EL MENSAJE DE EXITO O FALLO EN EL WEBSERVICE
                    db.Sms.Add(sms);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Se presentó un error al enviar el conjunto de SMS. Por favor verifique el mensaje y los destinatarios");
                }
            }

            return View(sms);
        }

        // GET: Sms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var sms = db.Sms.Find(id);
            if (sms == null)
            {
                return HttpNotFound();
            }
            return View(sms);
        }

        // POST: Sms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Sms sms)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Logica similar al envío para reenviar un mensaje nuevamente
                    db.Entry(sms).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Se presentó un error al enviar el conjunto de SMS. Por favor verifique el mensaje y los destinatarios");
                }
            }
            return View(sms);
        }

        // GET: Sms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var sms = db.Sms.Find(id);
            if (sms == null)
            {
                return HttpNotFound();
            }
            return View(sms);
        }

        // POST: Sms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sms sms = db.Sms.Find(id);
            db.Sms.Remove(sms);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult GetPerfil(int userId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var refer = db.Refers.Where(r => r.ReferType == userId);
            return Json(refer);
        }

        public JsonResult SearchVoters(string user, string refer, string comuna, string votacion)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Voter> voters = new List<Voter>();

            if (user != "0" && refer != "0")
            {
                var referUser = db.Refers.Find(Convert.ToInt32(refer));

                var voters1 = db.Voters.Where(v => v.ReferId == referUser.ReferId).ToList();
                voters.AddRange(voters1);

                if(comuna != "0")
                {
                    voters = new List<Voter>();
                    var voters2 = db.Voters.Where(v => v.ReferId == referUser.ReferId && v.CommuneId == comuna).ToList();
                    voters.AddRange(voters2);
                }

                if(votacion != "[Seleccione un Lugar de Votación]")
                {
                    voters = new List<Voter>();
                    var voters3 = db.Voters.Where(v => v.ReferId == referUser.ReferId && v.CommuneId == comuna && v.VotingPlaceId == votacion).ToList();
                    voters.AddRange(voters3);
                }
            }
            else
            {
                if(comuna != "0")
                {
                    var voters1 = db.Voters.Where(v => v.CommuneId == comuna).ToList();
                    voters.AddRange(voters1);

                    if (votacion != "[Seleccione un Lugar de Votación]")
                    {
                        voters = new List<Voter>();
                        var voters2 = db.Voters.Where(v => v.CommuneId == comuna && v.VotingPlaceId == votacion).ToList();
                        voters.AddRange(voters2);
                    }
                }
                else
                {
                    if(votacion != "[Seleccione un Lugar de Votación]")
                    {
                        var voters1 = db.Voters.Where(v => v.VotingPlaceId == votacion).ToList();
                        voters.AddRange(voters1);
                    }
                    else
                    {
                        voters = new List<Voter>();
                        var voters1 = db.Voters.ToList();
                        voters.AddRange(voters1);
                    }
                }
            }

            var jsonResult = Json(voters, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
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
