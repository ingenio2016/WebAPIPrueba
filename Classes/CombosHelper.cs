using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAPIPrueba.Models;

namespace WebAPIPrueba.Models
{
    public class CombosHelper : IDisposable
    {
        private static WebApiPruebaContext db = new WebApiPruebaContext();

        public static List<Country> GetCountries()
        {
            var country = db.Countries.ToList();
            country.Add(new Country
            {
                CountryId = 0,
                Name = "[Seleccione un País]",

            });
            return country.OrderBy(x => x.Name).ToList();
        }

        public static List<Filters> GetFilters()
        {
            var filter = db.Filters.ToList();
            filter.Add(new Filters
            {
                FiltersId = 0,
                name = "[Seleccione un Filtro]",

            });
            return filter.OrderBy(x => x.name).ToList();
        }

        public static List<Filters> GetVoterFilters()
        {
            var filter = db.Filters.Where(f=>f.name != "Lugar de Trabajo").ToList();
            filter.Add(new Filters
            {
                FiltersId = 0,
                name = "[Seleccione un Filtro]",

            });
            return filter.OrderBy(x => x.name).ToList();
        }

        public static List<UserId> GetUserNoVoters()
        {
            var user = db.UserIds.Where(u => u.userId < 5).ToList();
            user.Add(new UserId
            {
                userId = 0,
                name = "[Seleccione un Tipo de Usuario]",

            });
            return user.OrderBy(x => x.name).ToList();
        }

        public static List<UserId> GetTypeUser()
        {
            var user = db.UserIds.Where(u=>u.userId < 3).ToList();
            user.Add(new UserId
            {
                userId = 0,
                name = "[Seleccione un Tipo de Usuario]",

            });
            return user.OrderBy(x => x.name).ToList();
        }

        public static List<UserId> GetUser()
        {
            var user = db.UserIds.ToList();
            user.Add(new UserId
            {
                userId = 0,
                name = "[Seleccione un Tipo de Usuario]",

            });
            return user.OrderBy(x => x.name).ToList();
        }

        public static List<UserId> GetTypeUserCoordinator()
        {
            var user = db.UserIds.Where(u => u.userId < 4).ToList();
            user.Add(new UserId
            {
                userId = 0,
                name = "[Seleccione un Tipo de Usuario]",

            });
            return user.OrderBy(x => x.name).ToList();
        }

        public static List<Refer> GetRefer()
        {
            var refer = db.Refers.Where(r=>r.Active == 1).OrderBy(r=>r.FullName).ToList();
            refer.Add(new Refer
            {
                ReferId = 0,
                FullName = "[Seleccione un Usuario]",

            });
            return refer.OrderBy(x => x.FullName).ToList();
        }

        public static List<Refer> GetStandaloneRefer()
        {
            List<Refer> refer = new List<Refer>();
            refer.Add(new Refer
            {
                ReferId = 0,
                FullName = "[Seleccione un Usuario]",

            });
            return refer.OrderBy(x => x.FullName).ToList();
        }

        public static List<VotingPlace> GetVotingPlaces()
        {
            var voting = db.VotingPlaces.ToList();
            voting.Add(new VotingPlace
            {
                VotingPlaceId = 0,
                Name = "[Seleccione un Lugar de Votación]",

            });
            return voting.OrderBy(x => x.Name).ToList();
        }

        public static List<WorkPlace> GetWorkPlaces()
        {
            var workplace = db.WorkPlaces.ToList();
            workplace.Add(new WorkPlace
            {
                WorkPlaceId = 0,
                Name = "[Seleccione un Lugar de Trabajo]",

            });
            workplace.Add(new WorkPlace
            {
                WorkPlaceId = 9999,
                Name = "Otro",

            });
            return workplace.OrderBy(x => x.WorkPlaceId).ToList();
        }

        public static List<Commune> GetCommunes()
        {
            var commune = db.Communes.ToList();
            commune.Add(new Commune
            {
                CommuneId = 0,
                Name = "[Seleccione una Comuna]",

            });
            return commune.OrderBy(x => x.CommuneId).ToList();
        }

        public static List<Association> GetAssociations()
        {
            var associations = db.Associations.ToList();
            associations.Add(new Association
            {
                AssociationId = 0,
                Name = "[Seleccione una Asociación]",

            });
            return associations.OrderBy(x => x.AssociationId).ToList();
        }

        public static List<Department> GetDepartments()
        {
            var departments = db.Departments.ToList();
            departments.Add(new Department
            {
                DepartmentId = 0,
                Name = "[Seleccione un Departamento]",

            });
            return departments.OrderBy(x => x.Name).ToList();
        }

        public static List<City> GetCities()
        {
            var cities = db.Cities.ToList();
            cities.Add(new City
            {
                CityId = 0,
                Name = "[Seleccione una Ciudad]",

            });
            return cities.OrderBy(x => x.Name).ToList();
        }

        public static List<Company> GetCompanies()
        {
            var company = db.Companies.ToList();
            company.Add(new Company
            {
                CompanyId = 0,
                Name = "[Seleccione una Compañía]",

            });
            return company.OrderBy(x => x.Name).ToList();
        }

        public static List<Boss> GetBosses(int CompanyId)
        {
            var boss = db.Bosses.Where(b=>b.CompanyId == CompanyId).ToList();
            boss.Add(new Boss
            {
                BossId = 0,
                FirstName = "[Seleccione un Jefe]",

            });
            return boss.OrderBy(x => x.FirstName).ToList();
        }

        public static List<Link> GetLinks(int CompanyId)
        {
            var link = db.Links.Where(b => b.CompanyId == CompanyId).ToList();
            link.Add(new Link
            {
                LinkId = 0,
                FirstName = "[Seleccione un Enlace]",

            });
            return link.OrderBy(x => x.FirstName).ToList();
        }

        public static List<Coordinator> GetCoordinator(int CompanyId)
        {
            var coordinator = db.Coordinators.Where(b => b.CompanyId == CompanyId).ToList();
            coordinator.Add(new Coordinator
            {
                CoordinatorId = 0,
                FirstName = "[Seleccione un Coordinador]",

            });
            return coordinator.OrderBy(x => x.FirstName).ToList();
        }

        public void Dispose()
        {
            db.Dispose();
        }

        
    }
}