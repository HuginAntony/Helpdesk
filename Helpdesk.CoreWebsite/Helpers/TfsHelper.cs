using System;
using System.Net;
using System.Reflection;

namespace Helpdesk.CoreWebsite.Helpers
{
    public class TfsHelper
    {
        //private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        //public static string CreateTask(string taskTitle, string taskDescription, string ticketType, string developer, string ticketId, string priority)
        //{
        //    string taskId = "";
        //    try
        //    {
        //        NetworkCredential networkCredential = new NetworkCredential(ConfigurationManager.AppSettings["TFSUsername"], ConfigurationManager.AppSettings["TFSPassword"]);
        //        var tfs = new TfsTeamProjectCollection(new Uri(ConfigurationManager.AppSettings["TFSServerURL"]), networkCredential);
        //        tfs.EnsureAuthenticated();

        //        //Get data store that contains all workitems on a particular server
        //        WorkItemStore store = tfs.GetService<WorkItemStore>();

        //        //Get particular Team Project
        //        Project project = store.Projects[ConfigurationManager.AppSettings["TFSProjectName"]];
        //        string workItemType = ConfigurationManager.AppSettings["WorkItemType"].ToString();
        //        WorkItem task = project.WorkItemTypes[workItemType].NewWorkItem();

        //        //Set properties of task like title, iteration, activity, assigned to
        //        task.Title = taskTitle;
        //        task.Description = taskDescription;
        //        task.Fields["Reason"].Value = "New";
        //        task.Fields["Priority"].Value = priority.Substring(0, 1);
        //        task.Save();
        //        taskId = task.Id.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        _log.ErrorFormat("Error creating a TFS Task for Helpdesk ID {0}. Exception : {1}\r\n{2}", ticketId, ex.Message, ex.InnerException);
        //    }
        //    return taskId;
        //}

        //public static bool UpdateTask(int taskId, string developer)
        //{
        //    try
        //    {
        //        NetworkCredential networkCredential = new NetworkCredential(ConfigurationManager.AppSettings["TFSUsername"], ConfigurationManager.AppSettings["TFSPassword"]);
        //        var tfs = new TfsTeamProjectCollection(new Uri(ConfigurationManager.AppSettings["TFSServerURL"]), networkCredential);
        //        tfs.EnsureAuthenticated();

        //        //Get data store that contains all workitems on a particular server
        //        WorkItemStore store = tfs.GetService<WorkItemStore>();
        //        WorkItem task = store.GetWorkItem(taskId);

        //        task.Fields["Assigned To"].Value = developer;
        //        task.Save();
        //    }
        //    catch (Exception ex)
        //    {
        //        _log.ErrorFormat("Error creating a TFS Task {0}. Exception : {1}\r\n{2}", taskId, ex.Message, ex.InnerException);
        //        return false;
        //    }
        //    return true;
        //}
    }
}
