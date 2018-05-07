using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAPIPrueba.Models;

namespace WebAPIPrueba.Controllers
{
    public class ReportsController : Controller
    {
        WebApiPruebaContext db = new WebApiPruebaContext();
        // GET: Reports
        //public ActionResult Index(int? userId, int? FilterId, int? VotingPlaceId, int? page = null)
        //{
        //    page = (page ?? 1);

        //    if (userId > 0)
        //    {
        //        if (userId == 1)//JEFE
        //        {
        //            var boss = db.Bosses.OrderBy(b => b.FirstName).ToList();
        //            ViewBag.userId = new SelectList(
        //            CombosHelper.GetUser(),
        //            "userId",
        //            "name",
        //            userId);

        //            ViewBag.FilterId = new SelectList(
        //                CombosHelper.GetFilters(),
        //                "FiltersId",
        //                "name");

        //            ViewBag.VotingPlaceId = new SelectList(
        //                CombosHelper.GetVotingPlaces(),
        //                "VotingPlaceId",
        //                "Name");

        //            return View(boss.OrderBy(b => b.FirstName).ToPagedList((int)page, 6));
        //        }
        //    }

        //    return View(boss.OrderBy(b => b.FirstName).ToPagedList((int)page, 6));

        //}
    }
}