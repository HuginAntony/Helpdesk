﻿using System.Web;
using System.Web.Mvc;

namespace Helpdesk.Website
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new AuthAttribute());
            filters.Add(new HandleErrorAttribute());
        }
    }
}
