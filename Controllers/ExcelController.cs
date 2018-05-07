using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAPIPrueba.Models;

namespace WebAPIPrueba.Controllers
{
    [Authorize(Roles = "User, Digitador, Secretario")]
    public class ExcelController : Controller
    {
        private WebApiPruebaContext db = new WebApiPruebaContext();

        // GET: Excel
        public ActionResult Index()
        {
            return View();
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
        public ActionResult Importexcel1(int userId, string ReferId)
        {
            if(ReferId == "" || ReferId == string.Empty)
            {
                return RedirectToAction("ImportExcel", "Excel");
            }

            int referId = Convert.ToInt32(ReferId);
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
                var refer = db.Refers.Find(referId);
                if (userId == 1) boss = refer.UserId;
                if (userId == 2)
                {
                    link = refer.UserId;
                    var l = db.Links.Find(refer.UserId);
                    boss = l.BossId;
                    
                }
                if (userId == 3)
                {
                    coordinator = refer.UserId;
                    var c = db.Coordinators.Find(refer.UserId);
                    if (c.userId == 1) boss = c.BossId;
                    if (c.userId == 2) link = c.LinkId;
                }
                if (userId == 4)
                {
                    leader = refer.UserId;
                    var l = db.Leaders.Find(refer.UserId);
                    if (l.userId == 1) boss = l.BossId;
                    if (l.userId == 2) link = l.LinkId;
                    if (l.userId == 3) coordinator = l.CoordinatorId;
                }

                string extension = System.IO.Path.GetExtension(Request.Files["FileUpload1"].FileName).ToLower();
                string query = null;
                string connString = "";
                string[] validFileTypes = { ".xlsx" };

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
                        connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path1 + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                        DataTable dt = Utility.ConvertXSLXtoDataTable(path1, connString);
                        int contador = 0;
                        int mistakes = 0;
                        int existentes = 0;
                        ViewBag.Message = string.Empty;
                        ViewBag.Message1 = string.Empty;
                        ViewBag.Message2 = string.Empty;
                        foreach (DataRow row in dt.Rows)
                        {
                            if (row[15].ToString() == "JEFE" || row[15].ToString() == "jefe")
                            {
                                if (Utilidades.isNumeric(row[2].ToString()))
                                {
                                    double document = Convert.ToDouble(row[2].ToString());
                                    var Boss = db.Bosses.Where(v => v.Document == document).ToList();
                                    var vLink = db.Links.Where(v => v.Document == document).ToList();
                                    var vCoor = db.Coordinators.Where(v => v.Document == document).ToList();
                                    var vLeader = db.Leaders.Where(v => v.Document == document).ToList();
                                    if (Boss.Count == 0 && vLink.Count == 0 && vCoor.Count == 0 && vLeader.Count == 0)
                                    {
                                        string pais = "Colombia";
                                        string departmento = "Norte de Santander";
                                        string ciudad = "Cúcuta";
                                        var Country = db.Countries.Where(c => c.Name == pais).ToList();
                                        var Department = db.Departments.Where(c => c.Name == departmento).ToList();
                                        var City = db.Cities.Where(c => c.Name == ciudad).ToList();
                                        if (Country.Count > 0)
                                        {
                                            var IdPais = db.Countries.Where(c => c.Name == pais).FirstOrDefault();
                                            if (Department.Count > 0)
                                            {
                                                var IdDepartamento = db.Departments.Where(c => c.Name == departmento).FirstOrDefault();
                                                if (City.Count > 0)
                                                {
                                                    var IdCiudad = db.Cities.Where(c => c.Name == ciudad).FirstOrDefault();
                                                    if (Utilidades.isNumeric(row[8].ToString()))
                                                    {
                                                        string comune = row[8].ToString();
                                                        string votacion = row[7].ToString();
                                                        var comuna = db.Communes.Where(c => c.Name == comune).ToList();
                                                        var votingplace = db.VotingPlaces.Where(v => v.Name == votacion).ToList();
                                                        if (comuna.Count > 0)
                                                        {
                                                            var IdComuna = db.Communes.Where(c => c.Name == comune).FirstOrDefault();
                                                            if (votingplace.Count > 0)
                                                            {
                                                                var IdVotacion = db.VotingPlaces.Where(c => c.Name == votacion).FirstOrDefault();
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

                                                                string wP = row[13].ToString();
                                                                if (string.IsNullOrEmpty(wP))
                                                                {
                                                                    wP = "Ninguno";
                                                                }
                                                                var workPlace = db.WorkPlaces.Where(w => w.Name == wP).FirstOrDefault();
                                                                int workID = 0;
                                                                if (workPlace == null)
                                                                {
                                                                    workPlace = db.WorkPlaces.Where(w => w.Name == "Ninguno").FirstOrDefault();
                                                                    workID = workPlace.WorkPlaceId;
                                                                }
                                                                else
                                                                {
                                                                    workID = workPlace.WorkPlaceId;
                                                                }
                                                                using (var transaction = db.Database.BeginTransaction())
                                                                {
                                                                    try
                                                                    {
                                                                        var NewBoss = new Boss
                                                                        {
                                                                            FirstName = row[0].ToString(),
                                                                            LastName = row[1].ToString(),
                                                                            Document = document,
                                                                            DateBorn = dateBorn,
                                                                            CountryId = IdPais.CountryId,
                                                                            DepartmentId = IdDepartamento.DepartmentId,
                                                                            CityId = IdCiudad.CityId,
                                                                            Address = row[4].ToString(),
                                                                            UserName = row[14].ToString().ToLower(),
                                                                            Phone = row[6].ToString(),
                                                                            CommuneId = IdComuna.CommuneId,
                                                                            Barrio = row[8].ToString(),
                                                                            Profesion = row[10].ToString(),
                                                                            VotingPlaceId = IdVotacion.VotingPlaceId,
                                                                            CompanyId = user.CompanyId,
                                                                            Associacion = row[9].ToString(),
                                                                            Date = DateTime.Now,
                                                                            Especialidad = row[11].ToString(),
                                                                            TiempoExperiencia = row[12].ToString(),
                                                                            WorkPlaceId = workID,
                                                                        };
                                                                        db.Bosses.Add(NewBoss);
                                                                        db.SaveChanges();

                                                                        var voter = db.Voters.Where(v => v.Document == document).ToList();
                                                                        if (voter.Count == 0)
                                                                        {
                                                                            var Voter = new Voter
                                                                            {
                                                                                FirstName = row[0].ToString(),
                                                                                LastName = row[1].ToString(),
                                                                                Document = document,
                                                                                DateBorn = dateBorn,
                                                                                CountryId = pais,
                                                                                DepartmentId = departmento,
                                                                                CityId = ciudad,
                                                                                Address = row[4].ToString(),
                                                                                UserName = row[14].ToString().ToLower(),
                                                                                Phone = row[6].ToString(),
                                                                                CommuneId = row[8].ToString(),
                                                                                Barrio = row[5].ToString(),
                                                                                Profesion = row[10].ToString(),
                                                                                VotingPlaceId = row[7].ToString(),
                                                                                BossId = boss,
                                                                                LinkId = link,
                                                                                CoordinatorId = coordinator,
                                                                                LeaderId = leader,
                                                                                CompanyId = user.CompanyId,
                                                                                Fname = string.Format("{0} {1}", row[0].ToString(), row[1].ToString()),
                                                                                ReferId = refer.ReferId,
                                                                                PerfilId = userId,
                                                                                userId = 1,

                                                                            };
                                                                            db.Voters.Add(Voter);
                                                                            db.SaveChanges();

                                                                            var Refer = new Refer
                                                                            {
                                                                                ReferType = 1,
                                                                                UserId = NewBoss.BossId,
                                                                                FullName = NewBoss.FullName,
                                                                                Active = 1
                                                                            };
                                                                            db.Refers.Add(Refer);
                                                                            db.SaveChanges();
                                                                            transaction.Commit();

                                                                        }

                                                                        contador += 1;
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
                                                                mistakeDescription = "No se suministro Comúna"
                                                            });
                                                        }
                                                    }
                                                    else
                                                    {
                                                        mistakes += 1;
                                                        errores.Add(new Mistakes
                                                        {
                                                            document = row[2].ToString(),
                                                            mistakeDescription = "La comuna es incorrecta"
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
                                                    mistakeDescription = "No se suministro Lugar de Departamento"
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
                                        mistakes += 1;
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
                                        mistakeDescription = "Número de documento inválido"
                                    });
                                }
                            }
                            if (row[15].ToString() == "ENLACE" || row[15].ToString() == "enlace")
                            {
                                if (Utilidades.isNumeric(row[2].ToString()))
                                {
                                    double document = Convert.ToDouble(row[2].ToString());
                                    var Link = db.Links.Where(v => v.Document == document).ToList();
                                    var vBoss = db.Bosses.Where(v => v.Document == document).ToList();
                                    var vCoor = db.Coordinators.Where(v => v.Document == document).ToList();
                                    var vLeader = db.Leaders.Where(v => v.Document == document).ToList();
                                    if (Link.Count == 0 && vBoss.Count == 0 && vCoor.Count == 0 && vLeader.Count == 0)
                                    {
                                        string pais = "Colombia";
                                        string departmento = "Norte de Santander";
                                        string ciudad = "Cúcuta";
                                        var Country = db.Countries.Where(c => c.Name == pais).ToList();
                                        var Department = db.Departments.Where(c => c.Name == departmento).ToList();
                                        var City = db.Cities.Where(c => c.Name == ciudad).ToList();
                                        if (Country.Count > 0)
                                        {
                                            var IdPais = db.Countries.Where(c => c.Name == pais).FirstOrDefault();
                                            if (Department.Count > 0)
                                            {
                                                var IdDepartamento = db.Departments.Where(c => c.Name == departmento).FirstOrDefault();
                                                if (City.Count > 0)
                                                {
                                                    var IdCiudad = db.Cities.Where(c => c.Name == ciudad).FirstOrDefault();
                                                    if (Utilidades.isNumeric(row[8].ToString()))
                                                    {
                                                        string comune = row[8].ToString();
                                                        string votacion = row[7].ToString();
                                                        var comuna = db.Communes.Where(c => c.Name == comune).ToList();
                                                        var votingplace = db.VotingPlaces.Where(v => v.Name == votacion).ToList();
                                                        if (comuna.Count > 0)
                                                        {
                                                            var IdComuna = db.Communes.Where(c => c.Name == comune).FirstOrDefault();
                                                            if (votingplace.Count > 0)
                                                            {
                                                                var IdVotacion = db.VotingPlaces.Where(c => c.Name == votacion).FirstOrDefault();
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
                                                                string wP = row[13].ToString();
                                                                if (string.IsNullOrEmpty(wP))
                                                                {
                                                                    wP = "Ninguno";
                                                                }
                                                                var workPlace = db.WorkPlaces.Where(w => w.Name == wP).FirstOrDefault();
                                                                int workID = 0;
                                                                if (workPlace == null)
                                                                {
                                                                    workPlace = db.WorkPlaces.Where(w => w.Name == "Ninguno").FirstOrDefault();
                                                                    workID = workPlace.WorkPlaceId;
                                                                }
                                                                else
                                                                {
                                                                    workID = workPlace.WorkPlaceId;
                                                                }
                                                                using (var transaction = db.Database.BeginTransaction())
                                                                    {
                                                                        try
                                                                        {
                                                                            var NewLink = new Link
                                                                            {
                                                                                FirstName = row[0].ToString(),
                                                                                LastName = row[1].ToString(),
                                                                                Document = document,
                                                                                DateBorn = dateBorn,
                                                                                CountryId = IdPais.CountryId,
                                                                                DepartmentId = IdDepartamento.DepartmentId,
                                                                                CityId = IdCiudad.CityId,
                                                                                Address = row[4].ToString(),
                                                                                UserName = row[14].ToString().ToLower(),
                                                                                Phone = row[6].ToString(),
                                                                                CommuneId = IdComuna.CommuneId,
                                                                                Barrio = row[5].ToString(),
                                                                                Profesion = row[10].ToString(),
                                                                                VotingPlaceId = IdVotacion.VotingPlaceId,
                                                                                CompanyId = user.CompanyId,
                                                                                Asociacion = row[9].ToString(),
                                                                                Date = DateTime.Now,
                                                                                Especialidad = row[11].ToString(),
                                                                                TiempoExperiencia = row[12].ToString(),
                                                                                WorkPlaceId = workID,
                                                                                BossId = boss,
                                                                            };
                                                                            db.Links.Add(NewLink);
                                                                            db.SaveChanges();

                                                                            var voter = db.Voters.Where(v => v.Document == document).ToList();
                                                                            if (voter.Count == 0)
                                                                            {

                                                                                var Voter = new Voter
                                                                                {
                                                                                    FirstName = row[0].ToString(),
                                                                                    LastName = row[1].ToString(),
                                                                                    Document = document,
                                                                                    DateBorn = dateBorn,
                                                                                    CountryId = pais,
                                                                                    DepartmentId = departmento,
                                                                                    CityId = ciudad,
                                                                                    Address = row[4].ToString(),
                                                                                    UserName = row[14].ToString().ToLower(),
                                                                                    Phone = row[6].ToString(),
                                                                                    CommuneId = row[8].ToString(),
                                                                                    Barrio = row[5].ToString(),
                                                                                    Profesion = row[10].ToString(),
                                                                                    VotingPlaceId = row[7].ToString(),
                                                                                    BossId = boss,
                                                                                    LinkId = link,
                                                                                    CoordinatorId = coordinator,
                                                                                    LeaderId = leader,
                                                                                    CompanyId = user.CompanyId,
                                                                                    Fname = string.Format("{0} {1}", row[0].ToString(), row[1].ToString()),
                                                                                    ReferId = refer.ReferId,
                                                                                    PerfilId = userId,
                                                                                    userId = 2,

                                                                                };
                                                                                db.Voters.Add(Voter);
                                                                                db.SaveChanges();

                                                                                var Refer = new Refer
                                                                                {
                                                                                    ReferType = 2,
                                                                                    UserId = NewLink.LinkId,
                                                                                    FullName = NewLink.FullName,
                                                                                    Active = 1
                                                                                };
                                                                                db.Refers.Add(Refer);
                                                                                db.SaveChanges();
                                                                                transaction.Commit();

                                                                            }

                                                                            contador += 1;
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
                                                                mistakeDescription = "No se suministro Comúna"
                                                            });
                                                        }
                                                    }
                                                    else
                                                    {
                                                        mistakes += 1;
                                                        errores.Add(new Mistakes
                                                        {
                                                            document = row[2].ToString(),
                                                            mistakeDescription = "La comuna es incorrecta"
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
                                                    mistakeDescription = "No se suministro Lugar de Departamento"
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
                                        mistakes += 1;
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
                                        mistakeDescription = "Número de documento inválido"
                                    });
                                }
                            }
                            if (row[15].ToString() == "COORDINADOR" || row[15].ToString() == "coordinador")
                            {
                                if (Utilidades.isNumeric(row[2].ToString()))
                                {
                                    double document = Convert.ToDouble(row[2].ToString());
                                    var Coordinator = db.Coordinators.Where(v => v.Document == document).ToList();
                                    var vBoss = db.Bosses.Where(v => v.Document == document).ToList();
                                    var vLink = db.Links.Where(v => v.Document == document).ToList();
                                    var vLeader = db.Leaders.Where(v => v.Document == document).ToList();
                                    if (Coordinator.Count == 0 && vBoss.Count == 0 && vLink.Count == 0 && vLeader.Count == 0)
                                    {
                                        string pais = "Colombia";
                                        string departmento = "Norte de Santander";
                                        string ciudad = "Cúcuta";
                                        var Country = db.Countries.Where(c => c.Name == pais).ToList();
                                        var Department = db.Departments.Where(c => c.Name == departmento).ToList();
                                        var City = db.Cities.Where(c => c.Name == ciudad).ToList();
                                        if (Country.Count > 0)
                                        {
                                            var IdPais = db.Countries.Where(c => c.Name == pais).FirstOrDefault();
                                            if (Department.Count > 0)
                                            {
                                                var IdDepartamento = db.Departments.Where(c => c.Name == departmento).FirstOrDefault();
                                                if (City.Count > 0)
                                                {
                                                    var IdCiudad = db.Cities.Where(c => c.Name == ciudad).FirstOrDefault();
                                                    if (Utilidades.isNumeric(row[8].ToString()))
                                                    {
                                                        string comune = row[8].ToString();
                                                        string votacion = row[7].ToString();
                                                        var comuna = db.Communes.Where(c => c.Name == comune).ToList();
                                                        var votingplace = db.VotingPlaces.Where(v => v.Name == votacion).ToList();
                                                        if (comuna.Count > 0)
                                                        {
                                                            var IdComuna = db.Communes.Where(c => c.Name == comune).FirstOrDefault();
                                                            if (votingplace.Count > 0)
                                                            {
                                                                var IdVotacion = db.VotingPlaces.Where(c => c.Name == votacion).FirstOrDefault();
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
                                                                string wP = row[13].ToString();
                                                                if (string.IsNullOrEmpty(wP))
                                                                {
                                                                    wP = "Ninguno";
                                                                }
                                                                var workPlace = db.WorkPlaces.Where(w => w.Name == wP).FirstOrDefault();
                                                                int workID = 0;
                                                                if (workPlace == null)
                                                                {
                                                                    workPlace = db.WorkPlaces.Where(w => w.Name == "Ninguno").FirstOrDefault();
                                                                    workID = workPlace.WorkPlaceId;
                                                                }
                                                                else
                                                                {
                                                                    workID = workPlace.WorkPlaceId;
                                                                }
                                                                using (var transaction = db.Database.BeginTransaction())
                                                                    {
                                                                        try
                                                                        {
                                                                            var NewCoor = new Coordinator
                                                                            {
                                                                                FirstName = row[0].ToString(),
                                                                                LastName = row[1].ToString(),
                                                                                Document = document,
                                                                                DateBorn = dateBorn,
                                                                                CountryId = IdPais.CountryId,
                                                                                DepartmentId = IdDepartamento.DepartmentId,
                                                                                CityId = IdCiudad.CityId,
                                                                                Address = row[4].ToString(),
                                                                                UserName = row[14].ToString().ToLower(),
                                                                                Phone = row[6].ToString(),
                                                                                CommuneId = IdComuna.CommuneId,
                                                                                Barrio = row[5].ToString(),
                                                                                Profesion = row[10].ToString(),
                                                                                VotingPlaceId = IdVotacion.VotingPlaceId,
                                                                                CompanyId = user.CompanyId,
                                                                                Associacion = row[9].ToString(),
                                                                                Date = DateTime.Now,
                                                                                Especialidad = row[11].ToString(),
                                                                                TiempoExperiencia = row[12].ToString(),
                                                                                WorkPlaceId = workPlace.WorkPlaceId,
                                                                                BossId = boss,
                                                                                LinkId = link,
                                                                                ReferId = refer.ReferId,
                                                                                userId = userId,
                                                                            };
                                                                            db.Coordinators.Add(NewCoor);

                                                                            var voter = db.Voters.Where(v => v.Document == document).ToList();
                                                                            if (voter.Count == 0)
                                                                            {

                                                                                var Voter = new Voter
                                                                                {
                                                                                    FirstName = row[0].ToString(),
                                                                                    LastName = row[1].ToString(),
                                                                                    Document = document,
                                                                                    DateBorn = dateBorn,
                                                                                    CountryId = pais,
                                                                                    DepartmentId = departmento,
                                                                                    CityId = ciudad,
                                                                                    Address = row[4].ToString(),
                                                                                    UserName = row[14].ToString().ToLower(),
                                                                                    Phone = row[6].ToString(),
                                                                                    CommuneId = row[8].ToString(),
                                                                                    Barrio = row[5].ToString(),
                                                                                    Profesion = row[10].ToString(),
                                                                                    VotingPlaceId = row[7].ToString(),
                                                                                    BossId = boss,
                                                                                    LinkId = link,
                                                                                    CoordinatorId = coordinator,
                                                                                    LeaderId = leader,
                                                                                    CompanyId = user.CompanyId,
                                                                                    Fname = string.Format("{0} {1}", row[0].ToString(), row[1].ToString()),
                                                                                    ReferId = refer.ReferId,
                                                                                    userId = 3,
                                                                                    PerfilId = userId,

                                                                                };
                                                                                db.Voters.Add(Voter);
                                                                                db.SaveChanges();

                                                                                var Refer = new Refer
                                                                                {
                                                                                    ReferType = 3,
                                                                                    UserId = NewCoor.CoordinatorId,
                                                                                    FullName = NewCoor.FullName,
                                                                                    Active = 1
                                                                                };
                                                                                db.Refers.Add(Refer);
                                                                                db.SaveChanges();
                                                                                transaction.Commit();

                                                                            }

                                                                            contador += 1;
                                                                        }
                                                                        catch (Exception ex)
                                                                        {
                                                                            string doc = document.ToString();
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
                                                                mistakeDescription = "No se suministro Comúna"
                                                            });
                                                        }
                                                    }
                                                    else
                                                    {
                                                        mistakes += 1;
                                                        errores.Add(new Mistakes
                                                        {
                                                            document = row[2].ToString(),
                                                            mistakeDescription = "La comuna es incorrecta"
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
                                                    mistakeDescription = "No se suministro Lugar de Departamento"
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
                                        mistakes += 1;
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
                                        mistakeDescription = "Número de documento inválido"
                                    });
                                }
                            }
                            if (row[15].ToString() == "LIDER" || row[15].ToString() == "lider")
                            {
                                if (Utilidades.isNumeric(row[2].ToString()))
                                {
                                    double document = Convert.ToDouble(row[2].ToString());
                                    var Lider = db.Leaders.Where(v => v.Document == document).ToList();
                                    var vBoss = db.Bosses.Where(v => v.Document == document).ToList();
                                    var vLink = db.Links.Where(v => v.Document == document).ToList();
                                    var vCoor = db.Coordinators.Where(v => v.Document == document).ToList();
                                    if (Lider.Count == 0 && vBoss.Count == 0 && vLink.Count == 0 && vCoor.Count == 0)
                                    {
                                        string pais = "Colombia";
                                        string departmento = "Norte de Santander";
                                        string ciudad = "Cúcuta";
                                        var Country = db.Countries.Where(c => c.Name == pais).ToList();
                                        var Department = db.Departments.Where(c => c.Name == departmento).ToList();
                                        var City = db.Cities.Where(c => c.Name == ciudad).ToList();
                                        if (Country.Count > 0)
                                        {
                                            var IdPais = db.Countries.Where(c => c.Name == pais).FirstOrDefault();
                                            if (Department.Count > 0)
                                            {
                                                var IdDepartamento = db.Departments.Where(c => c.Name == departmento).FirstOrDefault();
                                                if (City.Count > 0)
                                                {
                                                    var IdCiudad = db.Cities.Where(c => c.Name == ciudad).FirstOrDefault();
                                                    if (Utilidades.isNumeric(row[8].ToString()))
                                                    {
                                                        string comune = row[8].ToString();
                                                        string votacion = row[7].ToString();
                                                        var comuna = db.Communes.Where(c => c.Name == comune).ToList();
                                                        var votingplace = db.VotingPlaces.Where(v => v.Name == votacion).ToList();
                                                        if (comuna.Count > 0)
                                                        {
                                                            var IdComuna = db.Communes.Where(c => c.Name == comune).FirstOrDefault();
                                                            if (votingplace.Count > 0)
                                                            {
                                                                var IdVotacion = db.VotingPlaces.Where(c => c.Name == votacion).FirstOrDefault();
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
                                                                string wP = row[13].ToString();
                                                                if (string.IsNullOrEmpty(wP))
                                                                {
                                                                    wP = "Ninguno";
                                                                }
                                                                var workPlace = db.WorkPlaces.Where(w => w.Name == wP).FirstOrDefault();
                                                                int workID = 0;
                                                                if (workPlace == null)
                                                                {
                                                                    workPlace = db.WorkPlaces.Where(w => w.Name == "Ninguno").FirstOrDefault();
                                                                    workID = workPlace.WorkPlaceId;
                                                                }
                                                                else
                                                                {
                                                                    workID = workPlace.WorkPlaceId;
                                                                }
                                                                using (var transaction = db.Database.BeginTransaction())
                                                                    {
                                                                        try
                                                                        {
                                                                            var NewLeader = new Leader
                                                                            {
                                                                                FirstName = row[0].ToString(),
                                                                                LastName = row[1].ToString(),
                                                                                Document = document,
                                                                                DateBorn = dateBorn,
                                                                                CountryId = IdPais.CountryId,
                                                                                DepartmentId = IdDepartamento.DepartmentId,
                                                                                CityId = IdCiudad.CityId,
                                                                                Address = row[4].ToString(),
                                                                                UserName = row[14].ToString().ToLower(),
                                                                                Phone = row[6].ToString(),
                                                                                CommuneId = IdComuna.CommuneId,
                                                                                Barrio = row[5].ToString(),
                                                                                Profesion = row[10].ToString(),
                                                                                VotingPlaceId = IdVotacion.VotingPlaceId,
                                                                                CompanyId = user.CompanyId,
                                                                                Associacion = row[9].ToString(),
                                                                                Date = DateTime.Now,
                                                                                Especialidad = row[11].ToString(),
                                                                                TiempoExperiencia = row[12].ToString(),
                                                                                WorkPlaceId = workPlace.WorkPlaceId,
                                                                                BossId = boss,
                                                                                LinkId = link,
                                                                                ReferId = refer.ReferId,
                                                                                userId = userId,
                                                                                CoordinatorId = coordinator
                                                                            };
                                                                            db.Leaders.Add(NewLeader);

                                                                            var voter = db.Voters.Where(v => v.Document == document).ToList();
                                                                            if (voter.Count == 0)
                                                                            {

                                                                                var Voter = new Voter
                                                                                {
                                                                                    FirstName = row[0].ToString(),
                                                                                    LastName = row[1].ToString(),
                                                                                    Document = document,
                                                                                    DateBorn = dateBorn,
                                                                                    CountryId = pais,
                                                                                    DepartmentId = departmento,
                                                                                    CityId = ciudad,
                                                                                    Address = row[4].ToString(),
                                                                                    UserName = row[14].ToString().ToLower(),
                                                                                    Phone = row[6].ToString(),
                                                                                    CommuneId = row[8].ToString(),
                                                                                    Barrio = row[5].ToString(),
                                                                                    Profesion = row[10].ToString(),
                                                                                    VotingPlaceId = row[7].ToString(),
                                                                                    BossId = boss,
                                                                                    LinkId = link,
                                                                                    CoordinatorId = coordinator,
                                                                                    LeaderId = leader,
                                                                                    CompanyId = user.CompanyId,
                                                                                    Fname = string.Format("{0} {1}", row[0].ToString(), row[1].ToString()),
                                                                                    ReferId = refer.ReferId,
                                                                                    userId = 4,
                                                                                    PerfilId = userId,

                                                                                };
                                                                                db.Voters.Add(Voter);
                                                                                db.SaveChanges();

                                                                                var Refer = new Refer
                                                                                {
                                                                                    ReferType = 4,
                                                                                    UserId = NewLeader.LeaderId,
                                                                                    FullName = NewLeader.FullName,
                                                                                    Active = 1
                                                                                };
                                                                                db.Refers.Add(Refer);
                                                                                db.SaveChanges();
                                                                                transaction.Commit();

                                                                            }

                                                                            contador += 1;
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
                                                                mistakeDescription = "No se suministro Comúna"
                                                            });
                                                        }
                                                    }
                                                    else
                                                    {
                                                        mistakes += 1;
                                                        errores.Add(new Mistakes
                                                        {
                                                            document = row[2].ToString(),
                                                            mistakeDescription = "La comuna es incorrecta"
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
                                                    mistakeDescription = "No se suministro Lugar de Departamento"
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
                                        mistakes += 1;
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
                                        mistakeDescription = "Número de documento inválido"
                                    });
                                }
                            }
                        }
                        if (contador > 0)
                        {
                            ViewBag.Message = "Se han ingresado correctamente " + contador + " Registros";
                        }
                        if (mistakes > 0)
                        {
                            ViewBag.Message1 = errores;
                        }
                        if (existentes > 0)
                        {
                            ViewBag.Message2 = "Se han encontrado " + existentes + " usuarios que ya habían sido registrados recientemente";
                        }

                        ViewBag.userId = new SelectList(
                           CombosHelper.GetUser(),
                           "userId",
                           "name");

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
                "name", 0);

            ViewBag.ReferId = new SelectList(
               CombosHelper.GetRefer(),
               "ReferId",
               "FullName");
            return View();
        }

        public JsonResult GetPerfil(int userId)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var refer = db.Refers.Where(r => r.ReferType == userId && r.Active == 1);
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

    }
}