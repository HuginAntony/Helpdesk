using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Helpdesk.Repository.CustomModels;
using log4net;
using log4net.Config;

namespace Helpdesk.Repository
{
    public class HelpdeskRepository
    {
        private readonly HelpdeskModels _helpdeskContext = new HelpdeskModels();
        public int SaveRequest(Request requst)
        {
                var savedRequest = _helpdeskContext.Requests.Add(requst);
                _helpdeskContext.SaveChanges();
                return savedRequest.Id;
        }

        public int SaveUser(User user)
        {
            var savedUser = _helpdeskContext.Users.Add(user);
            _helpdeskContext.SaveChanges();
            return savedUser.Id;
        }

        public int UpdateRequest(Request request)
        {
            return _helpdeskContext.SaveChanges();
        }

        public Request FindRequest(int id)
        {
            return _helpdeskContext.Requests.Find(id);
        }

        public User FindUser(int id)
        {
            return _helpdeskContext.Users.Find(id);
        }    
        
        public User FindUserByCredentials(string username, string password)
        {
            return _helpdeskContext.Users.SingleOrDefault(u => u.Username == username && u.Password == password);
        }

        public List<Request> GetRequestsByDate(DateTime startDate, DateTime endDate)
        {
            return _helpdeskContext.Requests.Where(r => r.DateCreated >= startDate && r.DateCreated <= endDate).OrderByDescending(r => r.DateCreated).ToList();
        }

        public List<Request> GetRequestsByDateBrand(DateTime startDate, DateTime endDate, int brandId)
        {
            return _helpdeskContext.Requests.Where(r => (r.DateCreated >= startDate && r.DateCreated <= endDate) && r.BrandId == brandId).OrderByDescending(r => r.DateCreated).ToList();
        }

        public List<GenericReport> GetReport(string reportName, DateTime startDate, DateTime endDate)
        {
            var start = new SqlParameter("@StartDate", startDate);
            var end = new SqlParameter("@EndDate", endDate);

            var report = _helpdeskContext.Database.SqlQuery<GenericReport>(string.Format("{0} @StartDate, @EndDate", reportName), start, end).ToList();
            return report;
        } 
        
        public List<GenericReport> GetReportByApplication(string reportName, DateTime startDate, DateTime endDate, string applicationId)
        {
            var start = new SqlParameter("@StartDate", startDate);
            var end = new SqlParameter("@EndDate", endDate);
            var appId = new SqlParameter("@ApplicationId", applicationId);
            
            var report = _helpdeskContext.Database.SqlQuery<GenericReport>(string.Format("{0} @StartDate, @EndDate, @ApplicationId", reportName), start, end, appId).ToList();
            return report;
        }

        public static object ToDBNull(object value)
        {
            if (null != value)
                return value;
            return DBNull.Value;
        }

        public List<Application> GetApplications()
        {
            return _helpdeskContext.Applications.ToList();
        }

        public List<Developer> GetDevelopers()
        {
            return _helpdeskContext.Developers.ToList();
        }

        public List<User> GetUsers()
        {
            return _helpdeskContext.Users.ToList();
        }
        
        public List<string> GetRequestTypes()
        {
            return _helpdeskContext.RequestTypes.Select(r => r.Name).ToList();
        }

        public RequestMessage GetRequestMessage(int id)
        {
            return _helpdeskContext.RequestMessages.Find(id);
        }

        public List<Role> GetRoles()
        {
            return _helpdeskContext.Roles.ToList();
        }  
        
        public List<Brand> GetBrands()
        {
            return _helpdeskContext.Brands.ToList();
        }
    }
}
