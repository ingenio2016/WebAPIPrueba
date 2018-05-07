using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAPIPrueba.Models;
using OfficeOpenXml;
using System.IO;
using System.Data;
using Microsoft.Reporting.WebForms;
using System.Data.Entity;

namespace WebAPIPrueba.Controllers
{
    public class HomeController : Controller
    {

        private WebApiPruebaContext db = new WebApiPruebaContext();
        [Authorize(Roles = "User, Admin, Reunion, Digitador, Secretario")]
        public ActionResult Index()
        {
            //lOGICA temporal

            //var boses = db.Bosses.ToList();
            //foreach (var boss in boses)
            //{
            //    //reparo referidos
            //    var refer = db.Refers.Where(r => r.ReferType == 1 && r.UserId == boss.BossId).FirstOrDefault();
            //    if (refer == null)
            //    {
            //        var reff = new Refer
            //        {
            //            FullName = boss.FirstName + " " + boss.LastName,
            //            ReferType = 1,
            //            UserId = boss.BossId,
            //            Active = 1
            //        };
            //        db.Refers.Add(reff);
            //        db.SaveChanges();
            //    }
            //    else
            //    {
            //        refer.Active = 1;
            //        db.Entry(refer).State = EntityState.Modified;
            //        db.SaveChanges();
            //    }
            //}

            //var links = db.Links.ToList();
            //foreach (var link in links)
            //{
            //    //reparo referidos
            //    var refer = db.Refers.Where(r => r.ReferType == 2 && r.UserId == link.LinkId).FirstOrDefault();
            //    if (refer == null)
            //    {
            //        var reff = new Refer
            //        {
            //            FullName = link.FirstName + " " + link.LastName,
            //            ReferType = 2,
            //            UserId = link.LinkId,
            //            Active = 1
            //        };
            //        db.Refers.Add(reff);
            //        db.SaveChanges();
            //    }
            //    else
            //    {
            //        refer.Active = 1;
            //        db.Entry(refer).State = EntityState.Modified;
            //        db.SaveChanges();
            //    }
            //}

            //var coors = db.Coordinators.ToList();
            //foreach (var coor in coors)
            //{
            //    //reparo referidos
            //    var refer = db.Refers.Where(r => r.ReferType == 3 && r.UserId == coor.CoordinatorId).FirstOrDefault();
            //    if (refer == null)
            //    {
            //        var reff = new Refer
            //        {
            //            FullName = coor.FirstName + " " + coor.LastName,
            //            ReferType = 3,
            //            UserId = coor.CoordinatorId,
            //            Active = 1
            //        };
            //        db.Refers.Add(reff);
            //        db.SaveChanges();
            //    }
            //    else
            //    {
            //        refer.Active = 1;
            //        db.Entry(refer).State = EntityState.Modified;
            //        db.SaveChanges();
            //    }
            //}

            //var leaders = db.Leaders.ToList();
            //foreach (var leader in leaders)
            //{
            //    //reparo referidos
            //    var refer = db.Refers.Where(r => r.ReferType == 4 && r.UserId == leader.LeaderId).FirstOrDefault();
            //    if (refer == null)
            //    {
            //        var reff = new Refer
            //        {
            //            FullName = leader.FirstName + " " + leader.LastName,
            //            ReferType = 4,
            //            UserId = leader.LeaderId,
            //            Active = 1
            //        };
            //        db.Refers.Add(reff);
            //        db.SaveChanges();
            //    }
            //    else
            //    {
            //        refer.Active = 1;
            //        db.Entry(refer).State = EntityState.Modified;
            //        db.SaveChanges();
            //    }
            //}

            var user = db.Users.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            return View(user);
        }

        [Authorize(Roles = "User, Admin, Reunion, Digitador, Secretario")]
        public ActionResult Search(string Document)
        {
            if(string.IsNullOrEmpty(Document))
            {
                return RedirectToAction("Index");
            }
            if (Utilidades.isNumeric(Document))
            {
                double doc = Convert.ToDouble(Document);
                var voter = db.Voters.Where(v => v.Document == doc).FirstOrDefault();
               
                return View(voter);
            }
            else
            {
                var voter = db.Voters.Where(v => v.Fname == Document).FirstOrDefault();
               
                return View(voter);
            }

            
        }

        [HttpPost]
        public JsonResult Buscar(string Prefix)
        {
            var voters = (from voter in db.Voters
                          where voter.Fname.Contains(Prefix)
                          select new
                          {
                              label = voter.Fname,
                              val = voter.VoterId
                          }).ToList();

            return Json(voters);
        }

        public JsonResult BuscarJefe(string Prefix)
        {
            var jefes = (from boss in db.Bosses
                          where boss.FirstName.Contains(Prefix) || boss.LastName.Contains(Prefix)
                          select new
                          {
                              label = boss.FirstName + " " + boss.LastName,
                              val = boss.BossId
                          }).ToList();

            return Json(jefes);
        }

        public JsonResult BuscarEnlace(string Prefix)
        {
            var enlace = (from link in db.Links
                         where link.FirstName.Contains(Prefix) || link.LastName.Contains(Prefix)
                         select new
                         {
                             label = link.FirstName + " " + link.LastName,
                             val = link.LinkId
                         }).ToList();

            return Json(enlace);
        }

        public JsonResult BuscarCoordinador(string Prefix)
        {
            var coordinador = (from link in db.Coordinators
                          where link.FirstName.Contains(Prefix) || link.LastName.Contains(Prefix)
                          select new
                          {
                              label = link.FirstName + " " + link.LastName,
                              val = link.CoordinatorId
                          }).ToList();

            return Json(coordinador);
        }

        public JsonResult BuscarLider(string Prefix)
        {
            var lider = (from link in db.Leaders
                               where link.FirstName.Contains(Prefix) || link.LastName.Contains(Prefix)
                               select new
                               {
                                   label = link.FirstName + " " + link.LastName,
                                   val = link.LeaderId
                               }).ToList();

            return Json(lider);
        }

        public JsonResult BuscarUsuario(string Prefix, string userId)
        {
            if (userId != "")
            {
                int Id = Convert.ToInt32(userId);
                if(Id == 0)
                {
                    var user = (from u in db.Refers
                                where u.FullName.ToUpper().Contains(Prefix.ToUpper())
                                orderby u.FullName
                                select new
                                {
                                    label = u.FullName,
                                    val = u.ReferId
                                }).ToList();

                    return Json(user);
                }
                else
                {
                    var user = (from u in db.Refers
                                where u.FullName.ToUpper().Contains(Prefix.ToUpper()) && u.ReferType == Id
                                orderby u.FullName
                                select new
                                {
                                    label = u.FullName,
                                    val = u.ReferId
                                }).ToList();

                    return Json(user);
                }
                
            }else
            {
                return null;
            }      
        }

        public ActionResult VotantesReport(int? boss, int? link, int? coor, int? leader, int? type)
        {
            var inc = new object();
            if(boss > 0){
                int value = Convert.ToInt32(boss);
                inc = (from v in db.Voters
                       join r in db.Refers on v.ReferId equals r.ReferId
                       where r.UserId == value && r.ReferType == 1
                       orderby v.userId, v.FirstName
                       select new { v.FirstName, v.LastName, v.Document, v.DateBorn, v.Address, v.UserName, v.Phone, v.CommuneId, v.Barrio, v.Profesion, v.VotingPlaceId, v.userId, referido = r.FullName }).ToList();
            }
            if (link > 0)
            {
                int value = Convert.ToInt32(link);
                inc = (from v in db.Voters
                       join r in db.Refers on v.ReferId equals r.ReferId
                       where r.UserId == value && r.ReferType == 2
                       orderby v.userId, v.FirstName
                       select new { v.FirstName, v.LastName, v.Document, v.DateBorn, v.Address, v.UserName, v.Phone, v.CommuneId, v.Barrio, v.Profesion, v.VotingPlaceId, v.userId, referido = r.FullName }).ToList();
            }
            if (coor > 0)
            {
                int value = Convert.ToInt32(coor);
                inc = (from v in db.Voters
                       join r in db.Refers on v.ReferId equals r.ReferId
                       where r.UserId == value && r.ReferType == 3
                       orderby v.userId, v.FirstName
                       select new { v.FirstName, v.LastName, v.Document, v.DateBorn, v.Address, v.UserName, v.Phone, v.CommuneId, v.Barrio, v.Profesion, v.VotingPlaceId, v.userId, referido = r.FullName }).ToList();
            }
            if (leader > 0)
            {
                int value = Convert.ToInt32(leader);
                inc = (from v in db.Voters
                       join r in db.Refers on v.ReferId equals r.ReferId
                       where r.UserId == value && r.ReferType == 4
                       orderby v.userId, v.FirstName
                       select new { v.FirstName, v.LastName, v.Document, v.DateBorn, v.Address, v.UserName, v.Phone, v.CommuneId, v.Barrio, v.Profesion, v.VotingPlaceId, v.userId, referido = r.FullName }).ToList();
            }


            string path = Path.Combine(Server.MapPath("~/Reports"), "VotersList.rdlc");

            LocalReport lr = new LocalReport();
            lr.EnableExternalImages = true;
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            var company = db.Companies.Find(1);

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
            "  <OutputFormat>" + "ListadoVotantes" + "</OutputFormat>" +
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

        public ActionResult RedReport(string document, int? type)
        {
            var inc = new object();
            double newDocument = Convert.ToDouble(document);
            List<Voter> listaVotantes = new List<Voter>();

            //Busco el votante
            var voter = db.Voters.Where(v => v.Document == newDocument).FirstOrDefault();
            if(voter != null)
            {

                listaVotantes.Add(voter);

                var Boss = db.Bosses.Where(b => b.Document == voter.Document).FirstOrDefault();
                var Link = db.Links.Where(b => b.Document == voter.Document).FirstOrDefault();
                var Coordinator = db.Coordinators.Where(b => b.Document == voter.Document).FirstOrDefault();
                var Leader = db.Leaders.Where(b => b.Document == voter.Document).FirstOrDefault();
                int cont1 = 0;
                int cont2 = 0;
                int cont3 = 0;
                int cont4 = 0;

                if (Boss != null)//JEFE
                {
                    var referboss = db.Refers.Where(r => r.ReferType == 1 && r.UserId == Boss.BossId).FirstOrDefault();
                    if (referboss != null)
                    {
                        cont1 = Boss.BossId;
                        //0 - Votantes del Jefe
                        var voterBoss = db.Voters.Where(v => v.ReferId == referboss.ReferId).ToList();
                        foreach(var vb in voterBoss)
                        {
                            listaVotantes.Add(vb);
                        }                        
                        
                        //1 - Enlaces del jefe
                        var linksToCount = db.Links.Where(l => l.BossId == Boss.BossId).ToList();
                        //recorro los enlaces
                        foreach (var item in linksToCount)
                        {
                            //BUSCO EL REFERIDO DE CADA ENLACE
                            var referItem = db.Refers.Where(r => r.ReferType == 2 && r.UserId == item.LinkId).FirstOrDefault();
                            if (referItem != null)
                            {
                                //2 - Coordinadores del enlace
                                var linkCoorsCount = db.Coordinators.Where(c => c.ReferId == referItem.ReferId).ToList();
                                foreach (var linkCoorItem in linkCoorsCount)
                                {
                                    //BUSCO EL REFERIDO DE CADA COORDINADOR
                                    var linkCoorItemRefer = db.Refers.Where(r => r.ReferType == 3 && r.UserId == linkCoorItem.CoordinatorId).FirstOrDefault();
                                    if (linkCoorItemRefer != null)
                                    {
                                        //3 - Lideres del coordinador
                                        var linkCoorLeadersCount = db.Leaders.Where(c => c.ReferId == linkCoorItemRefer.ReferId).ToList();
                                        foreach (var linkCoorLeadersItem in linkCoorLeadersCount)
                                        {
                                            //busco los referidos de cada lider
                                            var linkCoorLeadersRefer = db.Refers.Where(r => r.ReferType == 4 && r.UserId == linkCoorLeadersItem.LeaderId).FirstOrDefault();
                                            if (linkCoorLeadersRefer != null)
                                            {
                                                //4 - votantes de cada lider
                                                var voterLeaders = db.Voters.Where(v => v.ReferId == linkCoorLeadersRefer.ReferId).ToList();
                                                foreach(var vl in voterLeaders)
                                                {
                                                    listaVotantes.Add(vl);
                                                }
                                            }
                                        }
                                        //votantes de cada coordinador
                                        var votersCoor = db.Voters.Where(v => v.ReferId == linkCoorItemRefer.ReferId).ToList();
                                        foreach(var vc in votersCoor)
                                        {
                                            listaVotantes.Add(vc);
                                        }
                                    }
                                }
                                //lideres del enlace
                                var linkLeadersCount = db.Leaders.Where(c => c.ReferId == referItem.ReferId).ToList();
                                foreach (var linkLeaderItem in linkLeadersCount)
                                {
                                    var linkLeaderRefer = db.Refers.Where(r => r.ReferType == 4 && r.UserId == linkLeaderItem.LeaderId).FirstOrDefault();
                                    if (linkLeaderRefer != null)
                                    {
                                        //votantes del lider
                                        var voterlider = db.Voters.Where(v => v.ReferId == linkLeaderRefer.ReferId).ToList();
                                        foreach(var voterL in voterlider)
                                        {
                                            listaVotantes.Add(voterL);
                                        }
                                    }
                                }
                                //votantes de cada enlace
                                var voterlink = db.Voters.Where(v => v.ReferId == referItem.ReferId).ToList();
                                foreach(var vlink in voterlink)
                                {
                                    listaVotantes.Add(vlink);
                                }
                            }
                        }
                        //coordinadores del jefe
                        var coorToCount = db.Coordinators.Where(l => l.ReferId == referboss.ReferId).ToList();
                        foreach (var item in coorToCount)
                        {
                            var itemRefer = db.Refers.Where(r => r.ReferType == 3 && r.UserId == item.CoordinatorId).FirstOrDefault();
                            if (itemRefer != null)
                            {
                                var coorLeadersCount = db.Leaders.Where(c => c.ReferId == itemRefer.ReferId).ToList();
                                foreach (var coorLeader in coorLeadersCount)
                                {
                                    //lideres de cada coordinador
                                    var coorLeaderRefer = db.Refers.Where(r => r.ReferType == 4 && r.UserId == coorLeader.LeaderId).FirstOrDefault();
                                    if (coorLeaderRefer != null)
                                    {
                                        //votantes de cada lider
                                        var voterleaders = db.Voters.Where(v => v.ReferId == coorLeaderRefer.ReferId).ToList();
                                        foreach(var vl in voterleaders)
                                        {
                                            listaVotantes.Add(vl);
                                        }
                                    }
                                }
                                //votantes de cada coordinador
                                var votercoor = db.Voters.Where(v => v.ReferId == itemRefer.ReferId).ToList();
                                foreach(var vc in votercoor)
                                {
                                    listaVotantes.Add(vc);
                                }
                            }
                        }
                        //lideres del jefe
                        var leaderToCount = db.Leaders.Where(l => l.ReferId == referboss.ReferId).ToList();
                        foreach (var item in leaderToCount)
                        {
                            var itemleaderRefer = db.Refers.Where(r => r.ReferType == 4 && r.UserId == item.LeaderId).FirstOrDefault();
                            if (itemleaderRefer != null)
                            {
                                //votantes de cada lider
                                var voterleader = db.Voters.Where(v => v.ReferId == itemleaderRefer.ReferId).ToList();
                                foreach(var vl in voterleader)
                                {
                                    listaVotantes.Add(vl);
                                }
                            }
                        }
                    }
                }

                if (Link != null)//ENLACE
                {
                    var referlink = db.Refers.Where(r => r.ReferType == 2 && r.UserId == Link.LinkId).FirstOrDefault();
                    if (referlink != null)
                    {
                        cont2 = Link.LinkId;
                        var voterLinks = db.Voters.Where(v => v.ReferId == referlink.ReferId).ToList();
                        foreach(var vl in voterLinks)
                        {
                            listaVotantes.Add(vl);
                        }
                        

                        //coordinadores
                        var coorToCount = db.Coordinators.Where(l => l.ReferId == referlink.ReferId).ToList();
                        //obtengo los votantes de cada coordinador
                        foreach (var item in coorToCount)
                        {
                            var referItem = db.Refers.Where(r => r.ReferType == 3 && r.UserId == item.CoordinatorId).FirstOrDefault();
                            if (referItem != null)
                            {
                                var coorLeadersCount = db.Leaders.Where(c => c.ReferId == referItem.ReferId).ToList();
                                foreach (var coorLeader in coorLeadersCount)
                                {
                                    var coorLeaderRefer = db.Refers.Where(r => r.ReferType == 4 && r.UserId == coorLeader.LeaderId).FirstOrDefault();
                                    if (coorLeaderRefer != null)
                                    {
                                        var voterleaders = db.Voters.Where(v => v.ReferId == coorLeaderRefer.ReferId).ToList();
                                        foreach(var vl in voterleaders)
                                        {
                                            listaVotantes.Add(vl);
                                        }
                                    }
                                }
                                var votercoors = db.Voters.Where(v => v.ReferId == referItem.ReferId).ToList();
                                foreach(var vc in votercoors)
                                {
                                    listaVotantes.Add(vc);
                                }
                            }
                        }
                        //lideres
                        var leaderToCount = db.Leaders.Where(l => l.ReferId == referlink.ReferId).ToList();
                        //obtengo los votantes de cada lider
                        foreach (var item in leaderToCount)
                        {
                            var itemRefer = db.Refers.Where(r => r.ReferType == 4 && r.UserId == item.LeaderId).FirstOrDefault();
                            if (itemRefer != null)
                            {
                                var voterleads = db.Voters.Where(v => v.ReferId == itemRefer.ReferId).ToList();
                                foreach(var vli in voterleads)
                                {
                                    listaVotantes.Add(vli);
                                }
                            }
                        }
                    }
                }
                if (Coordinator != null)//COORDINADOR
                {
                    var refercoor = db.Refers.Where(r => r.ReferType == 3 && r.UserId == Coordinator.CoordinatorId).FirstOrDefault();
                    if (refercoor != null)
                    {
                        cont3 = Coordinator.CoordinatorId;
                        var voterCoors = db.Voters.Where(v => v.ReferId == refercoor.ReferId).ToList();
                        foreach(var vco in voterCoors)
                        {
                            listaVotantes.Add(vco);
                        }
                        

                        //lideres
                        var leaderToCount = db.Leaders.Where(l => l.ReferId == refercoor.ReferId).ToList();
                        //obtengo los votantes de cada lider
                        foreach (var item in leaderToCount)
                        {
                            var itemReferl = db.Refers.Where(r => r.ReferType == 4 && r.UserId == item.LeaderId).FirstOrDefault();
                            if (itemReferl != null)
                            {
                                var vleaders = db.Voters.Where(v => v.ReferId == itemReferl.ReferId).ToList();
                                foreach(var vli in vleaders)
                                {
                                    listaVotantes.Add(vli);
                                }
                            }
                        }
                       
                    }
                }
                if (Leader != null)//LIDER
                {
                    var referleader = db.Refers.Where(r => r.ReferType == 4 && r.UserId == Leader.LeaderId).FirstOrDefault();
                    if (referleader != null)
                    {
                        cont4 = Leader.LeaderId;
                        var vLeader = db.Voters.Where(v => v.ReferId == referleader.ReferId).ToList();
                        foreach(var vlead in vLeader)
                        {
                            listaVotantes.Add(vlead);
                        }
                        ViewBag.asociacion = Leader.Associacion;
                        ViewBag.lugarTrabajo = Leader.WorkPlaceId;
                        ViewBag.perfil = "Líder";
                        //sumo los votantes del lider
                    }
                }
            }


            string path = Path.Combine(Server.MapPath("~/Reports"), "VotersList.rdlc");

            LocalReport lr = new LocalReport();
            lr.EnableExternalImages = true;
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            var company = db.Companies.Find(1);

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

            ReportDataSource rd = new ReportDataSource("DataSet1", listaVotantes);
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
            "  <OutputFormat>" + "ListadoVotantes" + "</OutputFormat>" +
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

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult ImportExcel()
        {


            return View();
        }
        [ActionName("Importexcel")]
        [HttpPost]
        public ActionResult Importexcel1()
        {


            if (Request.Files["FileUpload1"].ContentLength > 0)
            {
                string extension = System.IO.Path.GetExtension(Request.Files["FileUpload1"].FileName).ToLower();
                string connString = "";




                string[] validFileTypes = { ".xls", ".xlsx", ".csv" };

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
                    if (extension == ".csv")
                    {
                        DataTable dt = Utility.ConvertCSVtoDataTable(path1);
                        ViewBag.Data = dt;
                    }
                    //Connection String to Excel Workbook  
                    else if (extension.Trim() == ".xls")
                    {
                        connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path1 + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                        DataTable dt = Utility.ConvertXSLXtoDataTable(path1, connString);
                        int contador = 0;
                        foreach (DataRow row in dt.Rows)
                        {
                            var products = new Product
                            {
                                IdProducto = Convert.ToInt32(row[0].ToString()),
                                NombreProducto = row[1].ToString(),
                                Proveedor = row[2].ToString(),
                                Categoria = row[3].ToString(),
                                CantidadPorUnidad = row[4].ToString(),
                                PrecioUnidad = row[5].ToString(),
                                UnidadesEnExistencia = Convert.ToInt32(row[6].ToString()),
                            };

                            db.Products.Add(products);
                            db.SaveChanges();
                            contador = dt.Rows.Count;
                        }
                        if(contador > 0)
                        {
                            ViewBag.Message = "Se han ingresado correctamente " + contador + " Registros";
                        }
                        ViewBag.Data = dt;
                    }
                    else if (extension.Trim() == ".xlsx")
                    {
                        connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path1 + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                        DataTable dt = Utility.ConvertXSLXtoDataTable(path1, connString);
                        ViewBag.Data = dt;
                    }

                }
                else
                {
                    ViewBag.Error = "Please Upload Files in .xls, .xlsx or .csv format";

                }

            }

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


    }
}