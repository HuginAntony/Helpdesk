using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Helpdesk.DataAccess;
using Helpdesk.DataAccess.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Helpdesk.CoreWebsite.Helpers
{
    public class HelpdeskRepository
    {
        private readonly HelpdeskContext _helpdeskContext = new HelpdeskContext();


        public int SaveRequest(Request requst)
        {
            var savedRequest = _helpdeskContext.Request.Add(requst);
            _helpdeskContext.SaveChanges();
            return savedRequest.Entity.Id;
        }

        public int SaveUser(User user)
        {
            var savedUser = _helpdeskContext.User.Add(user);
            _helpdeskContext.SaveChanges();
            return savedUser.Entity.Id;
        }

        public int UpdateRequest(Request request)
        {
            return _helpdeskContext.SaveChanges();
        }

        public Request FindRequest(int id)
        {
            return _helpdeskContext.Request.Find(id);
        }

        public User FindUser(int id)
        {
            return _helpdeskContext.User.Find(id);
        }

        public User FindUserByCredentials(string username, string password)
        {
            return _helpdeskContext.User.SingleOrDefault(u => u.Username == username && u.Password == password);
        }

        public List<Request> GetRequestsByDate(DateTime startDate, DateTime endDate)
        {
            return _helpdeskContext.Request.Where(r => r.DateCreated >= startDate && r.DateCreated <= endDate)
                                   .Include(r=>r.Developer)
                                   .Include(r=>r.Application)
                                   .Include(r=>r.AppSpecialist)
                                   .Include(r=>r.User)
                                   .OrderByDescending(r => r.DateCreated).ToList();
        }

        public List<Request> GetRequestsByDateBrand(DateTime startDate, DateTime endDate, int brandId)
        {
            return _helpdeskContext.Request.Where(r => (r.DateCreated >= startDate && r.DateCreated <= endDate) && r.BrandId == brandId).OrderByDescending(r => r.DateCreated).ToList();
        }

        public List<GenericReport> GetReport(string reportName, DateTime startDate, DateTime endDate)
        {
            var start = new SqlParameter("@StartDate", startDate);
            var end = new SqlParameter("@EndDate", endDate);

            var report = _helpdeskContext.Query<GenericReport>().FromSql(string.Format("{0} @StartDate, @EndDate", reportName), start, end).ToList();
            return report;
        }

        public List<GenericReport> GetReportByApplication(string reportName, DateTime startDate, DateTime endDate, string applicationId)
        {
            var start = new SqlParameter("@StartDate", startDate);
            var end = new SqlParameter("@EndDate", endDate);
            var appId = new SqlParameter("@ApplicationId", applicationId);

            var report = _helpdeskContext.Query<GenericReport>().FromSql(string.Format("{0} @StartDate, @EndDate, @ApplicationId", reportName), start, end, appId).ToList();
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
            return _helpdeskContext.Application.ToList();
        }

        public List<Developer> GetDevelopers()
        {
            return _helpdeskContext.Developer.ToList();
        }

        public List<User> GetUsers()
        {
            return _helpdeskContext.User.ToList();
        }

        public List<string> GetRequestTypes()
        {
            return _helpdeskContext.RequestType.Select(r => r.Name).ToList();
        }

        public RequestMessage GetRequestMessage(int id)
        {
            return _helpdeskContext.RequestMessage.Find(id);
        }

        public List<Role> GetRoles()
        {
            return _helpdeskContext.Role.ToList();
        }

        public List<Brand> GetBrands()
        {
            return _helpdeskContext.Brand.ToList();
        }
    }
}
